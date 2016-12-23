#I @"./bin/Debug"
#r @"Deadfile.Core.dll"
#r @"Newtonsoft.Json.dll"
#r @"Deadfile.Entity.dll"
#r @"Deadfile.Model.dll"
#r @"Deadfile.Importer.dll"
#r @"Prism.dll"
#r "EntityFramework.dll"
#r "EntityFramework.SqlServer.dll"
#r "System.ComponentModel.DataAnnotations.dll"

open System
open System.IO
open Deadfile.Core
open Deadfile.Importer
open Deadfile.Entity
open Newtonsoft.Json

let clients =
    @"D:\Documents\Deadfile\Mass Import\clients.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.clientLength)
    |> Seq.map FromCsv.makeClient
    |> Seq.toList
let jobs =
    @"D:\Documents\Deadfile\Mass Import\jobs.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.jobLength)
    |> Seq.map FromCsv.makeJob
    |> Seq.toList
let invoices =
    @"D:\Documents\Deadfile\Mass Import\invoices.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.invoiceLength)
    |> Seq.map FromCsv.makeInvoice
    |> Seq.toList
let invoiceItems =
    @"D:\Documents\Deadfile\Mass Import\invoiceItems.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.invoiceItemLength)
    |> Seq.map FromCsv.makeInvoiceItem
    |> Seq.toList
let quotations =
    @"D:\Documents\Deadfile\Mass Import\quotations.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.quotationLength)
    |> Seq.map FromCsv.makeQuotation
    |> Seq.toList
let expenses =
    @"D:\Documents\Deadfile\Mass Import\expenses.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.expenseLength)
    |> Seq.map FromCsv.makeExpense
    |> Seq.toList
let billableHours =
    @"D:\Documents\Deadfile\Mass Import\billableHours.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.billableHourLength)
    |> Seq.map FromCsv.makeBillableHour
    |> Seq.toList
let localAuthorities =
    @"D:\Documents\Deadfile\Mass Import\localAuthorities.csv"
    |> File.ReadAllLines
    |> Seq.skip 1
    |> Seq.map (fun s -> s.Split([|'\t'|]))
    |> Seq.filter (fun a -> a.Length = FromCsv.localAuthorityLength)
    |> Seq.map FromCsv.makeLocalAuthority
    |> Seq.toList

let entireDb =
    {
        Clients = clients
        Jobs = jobs
        Invoices = invoices
        InvoiceItems = invoiceItems
        Quotations = quotations
        JobTasks = []
        Applications = []
        Expenses = expenses
        BillableHours = billableHours
        LocalAuthorities = localAuthorities
    }
    |> EntireDb.Make
let jsonFile = new FileInfo(@"D:\Documents\Blah.json")

let doSerialize (jsonFile:FileInfo) =
    let serializer = JsonSerializer()
    serializer.Formatting <- Formatting.Indented
    use stream = new FileStream(jsonFile.FullName, FileMode.Create, FileAccess.Write)
    use writer = new StreamWriter(stream)
    serializer.Serialize(writer, entireDb)

doSerialize jsonFile


