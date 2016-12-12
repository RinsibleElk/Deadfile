namespace Deadfile.Importer

open System
open System.IO
open System.Text.RegularExpressions
open Deadfile.Entity
open Deadfile.Model
open Deadfile.Model.Interfaces

[<Sealed>]
type Importer(repository:IDeadfileRepository, jobsFile, quotationsFile, localAuthoritiesFile) =
    member __.PerformImport() =
        let jobsFromFile =
            jobsFile
            |> File.ReadAllLines
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
            quotationsFile
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
            localAuthoritiesFile
            |> File.ReadAllLines
            |> Array.filter (String.IsNullOrWhiteSpace >> not)
            |> Array.map
                (fun a ->
                    repository.SaveLocalAuthority(new LocalAuthorityModel(Name = a))
                    a)
        ()