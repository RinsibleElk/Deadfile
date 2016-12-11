namespace Deadfile.DataImporter

open Deadfile.Model
open Deadfile.Model.Interfaces
open NDesk.Options
open System
open System.IO
open System.Text.RegularExpressions
open Deadfile.Entity

[<Sealed>]
type ImporterArgs() =
    let mutable jobs = ""
    let mutable quotations = ""
    let mutable localAuthorities = ""
    member __.Jobs with get() = jobs and set v = jobs <- v
    member __.Quotations with get() = quotations and set v = quotations <- v
    member __.LocalAuthorities with get() = localAuthorities and set v = localAuthorities <- v

[<Sealed>]
type Importer(args) =
    let arguments = new ImporterArgs()
    let mutable isValid = true
    let optionSet =
        let os = new OptionSet()
        os.Add("jobs=", "Full path to a tab separated file specifying the jobs", (fun a -> arguments.Jobs <- a)) |> ignore
        os.Add("quotations=", "Full path to a tab separated file specifying the quotations", (fun a -> arguments.Quotations <- a)) |> ignore
        os.Add("local-authorities=", "Full path to a file specifying the local authorities", (fun a -> arguments.LocalAuthorities <- a)) |> ignore
        os
    do
        optionSet.Parse(args)
        |> fun l ->
            if l.Count = 0 then ()
            else
                eprintfn "Unrecognised arguments provided:"
                l |> Seq.iter (eprintfn "%s")
                eprintfn "Arguments:"
                optionSet.WriteOptionDescriptions(Console.Error)
                isValid <- false
    do
        [
            ("Jobs", arguments.Jobs)
            ("Quotations", arguments.Quotations)
            ("Local Authorities", arguments.LocalAuthorities)
        ]
        |> Seq.filter (snd >> String.IsNullOrWhiteSpace)
        |> Seq.filter (snd >> File.Exists >> not)
        |> Seq.toArray
        |> fun a ->
            if a.Length = 0 then ()
            else
                eprintfn "Missing required arguments or file missing. Missing files:"
                a |> Array.iter (fun (a,b) -> eprintfn "%s: %s" a b)
                eprintfn "Arguments:"
                optionSet.WriteOptionDescriptions(Console.Error)
                isValid <- false
    member __.PerformImport() =
        if isValid then
            let modelEntityMapper = ModelEntityMapper()
            let rng = RandomNumberGenerator()
            let repository = DeadfileRepository(modelEntityMapper, rng)
            let jobsFromFile =
                File.ReadAllLines arguments.Jobs
                |> Array.map (fun a -> a.Split([|'\t'|]))
                |> Array.filter (fun a -> a.Length = 7)
                |> Array.map JobConverter.convert
            let allClients =
                jobsFromFile
                |> Array.map (fun job -> job.ClientFullName, job.State)
                |> Array.countBy id
                |> Seq.groupBy (fst >> fst)
                |> Seq.map
                    (fun (clientFullName, s) ->
                        (clientFullName, (s |> Seq.map (fst >> snd) |> Seq.fold (fun state clientState -> if clientState = Dead then state else clientState) Dead)))
                |> Seq.toArray
            let clientIndexes =
                allClients
                |> Array.map
                    (fun (clientFullName,state) ->
                        let clientModel = new ClientModel()
                        let (fl,sl,tl) = jobsFromFile |> Array.find (fun job -> job.ClientFullName = clientFullName) |> fun job -> job.ClientAddressFirstLine, job.ClientAddressSecondLine, job.ClientAddressThirdLine
                        let clientNameSplit = clientFullName.Split([|' '|], StringSplitOptions.RemoveEmptyEntries)
                        if clientNameSplit.[0] = "Mr" || clientNameSplit.[0] = "Mrs" || clientNameSplit.[0] = "Dr" || clientNameSplit.[0] = "Ms" then
                            printfn "Splitting %s with title" clientFullName
                            if clientNameSplit.Length >= 4 && clientNameSplit.[1] = "&" then
                                clientModel.Title <- clientNameSplit.[0] + " & " + clientNameSplit.[2]
                                clientModel.LastName <- clientNameSplit.[clientNameSplit.Length - 1]
                                if (clientNameSplit.Length = 4) then ()
                                else if (clientNameSplit.Length = 5) then
                                    clientModel.FirstName <- clientNameSplit.[3]
                                else
                                    clientModel.FirstName <- clientNameSplit.[3]
                                    clientModel.MiddleNames <- String.Join(" ", [ 4 .. (clientNameSplit.Length - 1) ] |> List.map (fun i -> clientNameSplit.[i]))
                            else
                                clientModel.Title <- clientNameSplit.[0]
                                clientModel.LastName <- clientNameSplit.[clientNameSplit.Length - 1]
                                if (clientNameSplit.Length = 2) then ()
                                else if (clientNameSplit.Length = 3) then
                                    clientModel.FirstName <- clientNameSplit.[1]
                                else
                                    clientModel.FirstName <- clientNameSplit.[1]
                                    clientModel.MiddleNames <- String.Join(" ", [ 2 .. (clientNameSplit.Length - 1) ] |> List.map (fun i -> clientNameSplit.[i]))
                        else
                            printfn "Splitting %s sans title" clientFullName
                            clientModel.LastName <- clientNameSplit.[clientNameSplit.Length - 1]
                            if (clientNameSplit.Length = 1) then ()
                            else if (clientNameSplit.Length = 2) then
                                clientModel.FirstName <- clientNameSplit.[0]
                            else
                                clientModel.FirstName <- clientNameSplit.[0]
                                clientModel.MiddleNames <- String.Join(" ", [ 1 .. (clientNameSplit.Length - 1) ] |> List.map (fun i -> clientNameSplit.[i]))
                        clientModel.AddressFirstLine <- if String.IsNullOrWhiteSpace fl then "Unknown" else fl
                        clientModel.AddressSecondLine <- sl
                        // Detect if the third line contains (or is) a postcode.
                        let addressWithPostCodeRegex = new Regex("^(\s+), ([A-Z0-9]+[A-Z0-9]+)$")
                        let m = addressWithPostCodeRegex.Match(tl)
                        if m.Success then
                            clientModel.AddressThirdLine <- m.Groups.[1].Value
                            clientModel.AddressPostCode <- m.Groups.[2].Value
                        else
                            clientModel.AddressThirdLine <- tl
                        clientModel.Status <- (if state = Dead then ClientStatus.Inactive else ClientStatus.Active)
                        repository.SaveClient(clientModel)
                        (clientFullName, clientModel.ClientId))
                |> Map.ofArray
            let jobIds =
                jobsFromFile
                |> Array.map
                    (fun job ->
                        let clientId = clientIndexes |> Map.find job.ClientFullName
                        let clientModel = repository.GetClientById(clientId)
                        let jobModel = new JobModel()
                        jobModel.AddressFirstLine <- job.Property
                        jobModel.Description <- "No description."
                        jobModel.ClientId <- clientId
                        jobModel.JobNumber <- job.JobNumber
                        jobModel.Status <- (if job.State = Current then JobStatus.Active else JobStatus.Completed)
                        repository.SaveJob(jobModel)
                        (job.JobNumber, jobModel.JobId))
                |> Map.ofArray
            let quotations =
                arguments.Quotations
                |> File.ReadAllLines
                |> Array.map
                    (fun line ->
                        line.Split([|'\t'|], StringSplitOptions.RemoveEmptyEntries))
                |> Array.filter (fun a -> a.Length = 2)
                |> Array.map
                    (fun a->
                        let author = a.[0]
                        let phrase = a.[1]
                        repository.SaveQuotation(new QuotationModel(Phrase=phrase, Author=author))
                        (author, phrase))
            let localAuthorities =
                arguments.LocalAuthorities
                |> File.ReadAllLines
                |> Array.filter (String.IsNullOrWhiteSpace >> not)
                |> Array.map
                    (fun a ->
                        repository.SaveLocalAuthority(new LocalAuthorityModel(Name = a))
                        a)
            0
        else
            1
