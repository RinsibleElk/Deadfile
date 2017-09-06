namespace Deadfile.Importer

open System
open System.IO
open System.Text.RegularExpressions
open Deadfile.Entity
open Deadfile.Model
open Deadfile.Model.Browser
open Deadfile.Model.Interfaces
open FSharp.Reflection
open Newtonsoft.Json
open System.Collections.Generic

[<RequireQualifiedAccess>]
module FromCsv =
    let convertString s =
        if s = "NULL" then null else s
    let conv i (a:string[]) =
        convertString a.[i]
    let tonum i (a:string[]) =
        Int32.Parse(a.[i])
    let tobool i (a:string[]) =
        Boolean.Parse(a.[i])
    let tonumo i (a:string[]) =
        let b = a.[i]
        if b = "NULL" then None
        else b |> Int32.Parse |> Some
    let todate i (a:string[]) =
        a.[i] |> DateTime.Parse
    let tofloat i (a:string[]) = a.[i] |> Double.Parse

    let clientLength = 15
    let makeClient (a:string[]) =
        let clientId = Int32.Parse(a.[0])
        let title = a |> conv 1
        let firstName = a |> conv 2
        let middleNames = a |> conv 3
        let lastName = a |> conv 4
        let addressFirstLine = a |> conv 5
        let addressSecondLine = a |> conv 6
        let addressThirdLine = a |> conv 7
        let addressPostCode = a |> conv 8
        let phoneNumber1 = a |> conv 9
        let phoneNumber2 = a |> conv 10
        let phoneNumber3 = a |> conv 11
        let emailAddress = a |> conv 12
        let status = unbox<ClientStatus>(box (Int32.Parse(a.[13])))
        let notes = a |> conv 14
        {
            ClientId            = clientId
            Title               = title
            FirstName           = firstName
            MiddleNames         = middleNames
            LastName            = lastName
            AddressFirstLine    = addressFirstLine
            AddressSecondLine   = addressSecondLine
            AddressThirdLine    = addressThirdLine
            AddressPostCode     = addressPostCode
            PhoneNumber1        = phoneNumber1
            PhoneNumber2        = phoneNumber2
            PhoneNumber3        = phoneNumber3
            EmailAddress        = emailAddress
            Status              = status
            Notes               = notes
            Company             = null
        }

    let jobLength = 10
    let makeJob (a:string[]) =
        {
            JobId = a |> tonum 0
            JobNumber = a |> tonum 1
            AddressFirstLine = a |> conv 2
            AddressSecondLine = a |> conv 3
            AddressThirdLine = a |> conv 4
            AddressPostCode = a |> conv 5
            Status = a |> tonum 6 |> box |> unbox<JobStatus>
            Description = a |> conv 7
            Notes = a |> conv 8
            ClientId = a |> tonum 9
        }

    let invoiceLength = 17
    let makeInvoice a =
        {
            InvoiceId = a |> tonum 0
            CreatedDate = a |> todate 1
            GrossAmount = a |> tofloat 2
            NetAmount = a |> tofloat 3
            VatRate = a |> tofloat 4
            VatValue = a |> tofloat 5
            Status = a |> tonum 6 |> box |> unbox<InvoiceStatus>
            InvoiceReference = a |> tonum 7
            Company = a |> tonum 8 |> box |> unbox<Company>
            ClientName = a |> conv 9
            ClientAddressFirstLine = a |> conv 10
            ClientAddressSecondLine = a |> conv 11
            ClientAddressThirdLine = a |> conv 12
            ClientAddressPostCode = a |> conv 13
            Project = a |> conv 14
            Description = a |> conv 15
            ClientId = a |> tonum 16
            CreationState = InvoiceCreationState.DefineInvoice
        }

    let invoiceItemLength = 7
    let makeInvoiceItem a =
        {
            InvoiceItemId = a |> tonum 0
            Description = a |> conv 1
            NetAmount = a |> tofloat 2
            VatValue = a |> tofloat 3
            VatRate = a |> tofloat 4
            IncludeVat = a |> tobool 5
            InvoiceId = a |> tonum 6
        }

    let localAuthorityLength = 3
    let makeLocalAuthority a =
        {
            LocalAuthorityId = a |> tonum 0
            Name = a |> conv 1
            Url = a |> conv 2
        }

    let quotationLength = 3
    let makeQuotation a =
        {
            QuotationId = a |> tonum 0
            Phrase = a |> conv 1
            Author = a |> conv 2
        }

    let billableHourLength = 8
    let makeBillableHour a =
        let invoiceId = a |> tonumo 6
        {
            BillableHourId = a |> tonum 0
            Description = a |> conv 1
            NetAmount = a |> tofloat 2
            Notes = a |> conv 3
            CreationDate = a |> todate 4
            JobId = a |> tonum 5
            InvoiceId = invoiceId
            Person = null
            HoursWorked = 5
            State = if invoiceId.IsSome then BillableState.Billed else BillableState.Active
            ClientId = a |> tonum 7
        }

    let expenseLength = 8
    let makeExpense a =
        let invoiceId = a |> tonumo 6
        {
            ExpenseId = a |> tonum 0
            Description = a |> conv 1
            NetAmount = a |> tofloat 2
            Notes = a |> conv 3
            CreationDate = a |> todate 4
            JobId = a |> tonum 5
            InvoiceId = invoiceId
            ClientId = a |> tonum 7
            State = if invoiceId.IsSome then BillableState.Billed else BillableState.Active
            Type = ExpenseType.ApplicationFees
        }
