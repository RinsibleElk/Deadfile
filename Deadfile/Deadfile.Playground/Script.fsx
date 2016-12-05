#r @"System.ObjectModel"
#r @"bin\Debug\PdfSharp.dll"
#r @"bin\Debug\Prism.dll"
#r @"bin\Debug\Deadfile.Core.dll"
#r @"bin\Debug\Deadfile.Model.dll"
#r @"bin\Debug\Deadfile.Pdf.dll"
#r @"bin\Debug\Deadfile.Entity.dll"

open System
open System.IO
open Deadfile.Pdf
open Deadfile.Entity
open Deadfile.Model

let getInvoice company =
    let invoice = new InvoiceModel()
    invoice.Company <- company
    invoice.InvoiceId <- 1
    invoice.InvoiceReference <- 87
    invoice.ClientName <- "Mr Roger Rabbit"
    invoice.ClientAddressFirstLine <- "11 Haileybury Court"
    invoice.ClientAddressSecondLine <- "Enfield"
    invoice.ClientAddressPostCode <- "EN3 6FT"
    invoice.CreatedDate <- new DateTime(2016, 10, 15)
    invoice.Project <- "11 Hailebury Court"
    invoice.Description <- "Some work what we done"
    let children = new System.Collections.Generic.List<InvoiceItemModel>()
    let invoiceItem1 = new InvoiceItemModel()
    invoiceItem1.Context <- 0
    invoiceItem1.Description <- "Hello, world"
    invoiceItem1.NetAmount <- 100.0
    children.Add(invoiceItem1)
    let invoiceItem2 = new InvoiceItemModel()
    invoiceItem2.Context <- 0
    invoiceItem2.Description <- "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras vulputate fringilla est, id rutrum dui consectetur at. Mauris eget justo ac metus viverra convallis id id ligula. Quisque non blandit ex. Nam sagittis semper lobortis. Donec viverra, ex sed ornare interdum, ligula odio hendrerit erat, id sollicitudin nisl est eget quam. Aenean mollis tortor sit amet metus vestibulum condimentum. Nullam molestie ut justo vitae ornare."
    invoiceItem2.NetAmount <- 300.0
    children.Add(invoiceItem2)
    let invoiceItem3 = new InvoiceItemModel()
    invoiceItem3.Context <- 0
    invoiceItem3.Description <- "Hello, world"
    invoiceItem3.NetAmount <- 150.0
    children.Add(invoiceItem3)
    invoice.ChildrenList <- children
    invoice.NetAmount <- 550.0
    if company = Company.PaulSamsonCharteredSurveyorLtd then
        invoice.VatRate <- 20.0
        invoice.VatValue <- 0.2 * invoice.NetAmount
        invoice.GrossAmount <- 1.2 * invoice.NetAmount
    else
        invoice.VatRate <- 0.0
        invoice.VatValue <- 0.0
        invoice.GrossAmount <- invoice.NetAmount
    invoice
let generate invoiceModel =
    let invoiceGenerator = new CompanySwitchingInvoiceGenerator()
    invoiceGenerator.Generate(invoiceModel, sprintf @"D:\PdfSharpPlaypen\0\%sOutput.pdf" (if invoiceModel.Company = Company.Imagine3DLtd then "I3D" else "PKS"))
generate (getInvoice (Company.PaulSamsonCharteredSurveyorLtd))
