namespace Deadfile.DataImporter

open System

type JobState =
    | Dead
    | Current

type Job =
    {
        Property : string
        State : JobState
        JobNumber : int
        ClientFullName : string
        ClientAddressFirstLine : string
        ClientAddressSecondLine : string
        ClientAddressThirdLine : string
    }

[<RequireQualifiedAccess>]
module JobConverter =
    let convert (a:string array) =
        let property = a.[0]
        let (ok, jobNumber) = Int32.TryParse(a.[1])
        let state =
            match a.[2] with
            | "Dead" -> Dead
            | "Current" -> Current
            | _ -> failwithf "Unrecognised job state %s" a.[2]
        {
            Property = property
            State = state
            JobNumber = if ok then jobNumber else Int32.MinValue
            ClientFullName = a.[3]
            ClientAddressFirstLine = a.[4]
            ClientAddressSecondLine = a.[5]
            ClientAddressThirdLine = a.[6]
        }
