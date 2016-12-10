namespace Deadfile.DataImporter

open Deadfile.Model
open Deadfile.Model.Interfaces
open NDesk.Options
open System
open System.IO

[<Sealed>]
type ImporterArgs() =
    let mutable jobs = ""
    let mutable applicationFees = ""
    let mutable quotations = ""
    member __.Jobs with get() = jobs and set v = jobs <- v
    member __.ApplicationFees with get() = applicationFees and set v = applicationFees <- v
    member __.Quotations with get() = quotations and set v = quotations <- v

[<Sealed>]
type Importer(args) =
    let arguments = new ImporterArgs()
    let mutable isValid = true
    let optionSet =
        let os = new OptionSet()
        os.Add("jobs=", "Full path to a tab separated file specifying the jobs", (fun a -> arguments.Jobs <- a)) |> ignore
        os.Add("application-fees=", "Full path to a tab separated file specifying the application fees", (fun a -> arguments.ApplicationFees <- a)) |> ignore
        os.Add("quotations=", "Full path to a tab separated file specifying the quotations", (fun a -> arguments.Quotations <- a)) |> ignore
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
            ("Application Fees", arguments.ApplicationFees)
            ("Quotations", arguments.Quotations)
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

            0
        else
            1
