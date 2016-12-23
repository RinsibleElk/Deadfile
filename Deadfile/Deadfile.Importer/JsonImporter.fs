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

type ClientJson =
    {
        ClientId : int
        Title : string
        FirstName : string
        MiddleNames : string
        LastName : string
        Company : string
        AddressFirstLine : string
        AddressSecondLine : string
        AddressThirdLine : string
        AddressPostCode : string
        PhoneNumber1 : string
        PhoneNumber2 : string
        PhoneNumber3 : string
        EmailAddress : string
        Status : ClientStatus
        Notes : string
    }

type JobJson =
    {
        JobId : int
        JobNumber : int
        AddressFirstLine : string
        AddressSecondLine : string
        AddressThirdLine : string
        AddressPostCode : string
        Status : JobStatus
        Notes : string
        Description : string
        ClientId : int
    }

type InvoiceJson =
    {
        InvoiceId : int
        CreatedDate : DateTime
        GrossAmount : double
        NetAmount : double
        Status : InvoiceStatus
        InvoiceReference : int
        Company : Company
        CreationState : InvoiceCreationState
        ClientName : string
        ClientAddressFirstLine : string
        ClientAddressSecondLine : string
        ClientAddressThirdLine : string
        ClientAddressPostCode : string
        Project : string
        Description : string
        VatRate : double
        VatValue : double
        ClientId : int
    }

type JobTaskJson =
    {
        ClientId : int
        JobId : int
        JobTaskId : int
        ClientFullName : string
        Property : string
        Description : string
        Notes : string
        DueDate : DateTime
        State : JobTaskState
        Priority : JobTaskPriority
    }

type QuotationJson =
    {
        QuotationId : int
        Phrase : string
        Author : string
    }

type ApplicationJson =
    {
        ClientId : int
        JobId : int
        ApplicationId : int
        LocalAuthority : string
        LocalAuthorityReference : string
        CreationDate : DateTime
        Type : ApplicationType
        EstimatedDecisionDate : DateTime
        State : ApplicationState
    }

type ExpenseJson =
    {
        ClientId : int
        JobId : int
        ExpenseId : int
        Description : string
        NetAmount : double
        Type : ExpenseType
        Notes : string
        CreationDate : DateTime
        State : BillableState
        InvoiceId : int option
    }

type BillableHourJson =
    {
        ClientId : int
        JobId : int
        BillableHourId : int
        Description : string
        Person : string
        HoursWorked : int
        NetAmount : double
        Notes : string
        CreationDate : DateTime
        State : BillableState
        InvoiceId : int option
    }

type InvoiceItemJson =
    {
        InvoiceItemId : int
        Description : string
        NetAmount : double
        InvoiceId : int
    }

type LocalAuthorityJson =
    {
        LocalAuthorityId : int
        Name : string
        Url : string
    }

type internal IdCacheBehaviour =
    | RetrieveAll
    | LearnClientId
    | LearnJobId
    | LearnInvoiceId

[<Sealed>]
type internal IdCache() =
    let clientIdCache = new Dictionary<int,int>()
    let jobIdCache = new Dictionary<int,int>()
    let invoiceIdCache = new Dictionary<int,int>()
    member __.Map(fieldName, behaviour, value) =
        if fieldName = "ClientId" then
            if behaviour = LearnClientId then
                box ModelBase.NewModelId
            else
                box (clientIdCache.[(unbox<int>(value))])
        else if fieldName = "JobId" then
            if behaviour = LearnJobId then
                box ModelBase.NewModelId
            else
                box (jobIdCache.[(unbox<int>(value))])
        else if fieldName = "InvoiceId" then
            if behaviour = LearnInvoiceId then
                box ModelBase.NewModelId
            else
                box (invoiceIdCache.[(unbox<int>(value))])
        else
            value
    member __.AddClientId(clientId, newClientId) = clientIdCache.Add(clientId, newClientId)
    member __.AddJobId(jobId, newJobId) = jobIdCache.Add(jobId, newJobId)
    member __.AddInvoiceId(invoiceId, newInvoiceId) = invoiceIdCache.Add(invoiceId, newInvoiceId)

[<RequireQualifiedAccess>]
module internal ToFromRecord =
    /// Map from some POCO to a record.
    let mapToRecord<'recordType, 'classType> (c:'classType) =
        FSharpType.GetRecordFields(typeof<'recordType>)
        |> Array.map
            (fun pi ->
                let propertyInfo = typeof<'classType>.GetProperty(pi.Name, Reflection.BindingFlags.Public ||| Reflection.BindingFlags.Instance)
                if propertyInfo.PropertyType = typeof<int option> then
                    box (propertyInfo.GetMethod.Invoke(c, [||]) |> unbox<Nullable<int>> |> fun ni -> if ni.HasValue then Some ni.Value else None)
                else
                    propertyInfo.GetMethod.Invoke(c, [||]))
        |> fun a -> FSharp.Reflection.FSharpValue.MakeRecord(typeof<'recordType>, a)
        |> unbox<'recordType>
    /// Map from some record to an entity.
    let mapToEntity<'recordType, 'classType> (idCache:IdCache) behaviour (r:'recordType) =
        let c = Activator.CreateInstance<'classType>()
        Array.zip
            (FSharpType.GetRecordFields(typeof<'recordType>))
            (FSharpValue.GetRecordFields(r))
        |> Array.iter
            (fun (pi, value) ->
                let propertyInfo = typeof<'classType>.GetProperty(pi.Name, Reflection.BindingFlags.Public ||| Reflection.BindingFlags.Instance)
                let setMethod =
                    if isNull propertyInfo then
                        eprintfn "Did you mean to try setting %s?" pi.Name
                        null
                    else
                        propertyInfo.SetMethod
                if pi.Name = "InvoiceId" && pi.PropertyType = typeof<int option> then
                    let invoiceId = unbox<int option> value
                    if invoiceId.IsNone then
                        setMethod.Invoke(c, [|new Nullable<int>()|]) |> ignore
                    else
                        setMethod.Invoke(c, [|new Nullable<int>(unbox<int>(idCache.Map(pi.Name, behaviour, box invoiceId.Value)))|]) |> ignore
                else
                    if (isNull (setMethod)) then
                        eprintfn "Did you mean to try setting %s?" pi.Name
                        ()
                    else
                        setMethod.Invoke(c, [|(idCache.Map(pi.Name, behaviour, value))|]) |> ignore)
        c

type EntireDbVersion1 =
    {
        Clients : ClientJson list
        Jobs : JobJson list
        Invoices : InvoiceJson list
        InvoiceItems : InvoiceItemJson list
        Quotations : QuotationJson list
        JobTasks : JobTaskJson list
        Applications : ApplicationJson list
        Expenses : ExpenseJson list
        BillableHours : BillableHourJson list
        LocalAuthorities : LocalAuthorityJson list
    }

type EntireDb() =
    let mutable version1Data : EntireDbVersion1 option = None
    let mutable version : int = 1
    member __.Version1Data with get() = version1Data and set v = version1Data <- v
    member __.SerializeVersion1Data() = version1Data.IsSome
    member __.Version with get() = version and set v = version <- v
    static member Make data = EntireDb(Version1Data = Some data)

[<Sealed>]
type JsonImporter(repository:IDeadfileRepository) =
    let cache = IdCache()
    let saveChanges (context:DeadfileContext) =
        try
            context.SaveChanges() |> ignore
        with e ->
            match (box e) with
            | :? System.Data.Entity.Validation.DbEntityValidationException as validationEx ->
                validationEx.EntityValidationErrors
                |> Seq.iter
                    (fun v ->
                        v.ValidationErrors
                        |> Seq.iter
                            (fun ve ->
                                failwithf "%s: %s" ve.PropertyName ve.ErrorMessage))
            failwithf "%A" e
    let saveClient clientJson =
        use context = new DeadfileContext()
        let client = ToFromRecord.mapToEntity<ClientJson, Client> cache LearnClientId clientJson
        context.Clients.Add(client) |> ignore
        context |> saveChanges
        cache.AddClientId(clientJson.ClientId, client.ClientId)
    let saveJob jobJson =
        use context = new DeadfileContext()
        let job = ToFromRecord.mapToEntity<JobJson, Job> cache LearnJobId jobJson
        context.Jobs.Add(job) |> ignore
        context |> saveChanges
        cache.AddJobId(jobJson.JobId, job.JobId)
    let saveInvoice invoiceJson =
        use context = new DeadfileContext()
        let invoice = ToFromRecord.mapToEntity<InvoiceJson, Invoice> cache LearnInvoiceId invoiceJson
        context.Invoices.Add(invoice) |> ignore
        context |> saveChanges
        cache.AddInvoiceId(invoiceJson.InvoiceId, invoice.InvoiceId)
    let saveInvoiceItem json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<InvoiceItemJson, InvoiceItem> cache RetrieveAll json
        context.InvoiceItems.Add(entity) |> ignore
        context |> saveChanges
    let saveQuotation json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<QuotationJson, Quotation> cache RetrieveAll json
        context.Quotations.Add(entity) |> ignore
        context |> saveChanges
    let saveJobTask json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<JobTaskJson, JobTask> cache RetrieveAll json
        context.JobTasks.Add(entity) |> ignore
        context |> saveChanges
    let saveApplication json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<ApplicationJson, Application> cache RetrieveAll json
        context.Applications.Add(entity) |> ignore
        context |> saveChanges
    let saveExpense json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<ExpenseJson, Expense> cache RetrieveAll json
        context.Expenses.Add(entity) |> ignore
        context |> saveChanges
    let saveBillableHour json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<BillableHourJson, BillableHour> cache RetrieveAll json
        context.BillableHours.Add(entity) |> ignore
        context |> saveChanges
    let saveLocalAuthority json =
        use context = new DeadfileContext()
        let entity = ToFromRecord.mapToEntity<LocalAuthorityJson, LocalAuthority> cache RetrieveAll json
        context.LocalAuthorities.Add(entity) |> ignore
        context |> saveChanges
    member __.ExportToJsonFile(jsonFile:FileInfo) =
        let clients =
            repository.GetBrowserItems(BrowserSettings(IncludeInactiveEnabled = true, Mode = BrowserMode.Client))
            |> Seq.map (fun j -> repository.GetClientById j.Id)
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<ClientJson, ClientModel>)
        let jobs =
            repository.GetBrowserItems(BrowserSettings(IncludeInactiveEnabled = true, Mode = BrowserMode.Job))
            |> Seq.map (fun j -> repository.GetJobById j.Id)
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<JobJson, JobModel>)
        let invoices, invoiceItems =
            repository.GetBrowserItems(BrowserSettings(IncludeInactiveEnabled = true, Mode = BrowserMode.Invoice))
            |> Seq.map (fun j -> repository.GetInvoiceById j.Id)
            |> Seq.toList
            |> List.map
                (fun invoice ->
                    let invoiceItems = invoice.ChildrenList |> Seq.toList |> List.map (ToFromRecord.mapToRecord<InvoiceItemJson, InvoiceItemModel>)
                    let i = ToFromRecord.mapToRecord<InvoiceJson, InvoiceModel> invoice
                    i, invoiceItems)
            |> List.unzip
            |> fun (invoices,items) -> (invoices, (items |> Seq.concat |> Seq.toList))
        let quotations =
            repository.GetQuotations("")
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<QuotationJson, QuotationModel>)
        let jobTasks =
            jobs
            |> List.map (fun j -> repository.GetJobTasksForJob(j.JobId, ""))
            |> Seq.concat
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<JobTaskJson, JobTaskModel>)
        let applications =
            jobs
            |> List.map (fun j -> repository.GetApplicationsForJob(j.JobId, ""))
            |> Seq.concat
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<ApplicationJson, ApplicationModel>)
        let expenses =
            jobs
            |> List.map (fun j -> repository.GetExpensesForJob(j.JobId, ""))
            |> Seq.concat
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<ExpenseJson, ExpenseModel>)
        let billableHours =
            jobs
            |> List.map (fun j -> repository.GetBillableHoursForJob(j.JobId, ""))
            |> Seq.concat
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<BillableHourJson, BillableHourModel>)
        let localAuthorities =
            repository.GetLocalAuthorities("")
            |> Seq.toList
            |> List.map (ToFromRecord.mapToRecord<LocalAuthorityJson, LocalAuthorityModel>)
        let entireDb =
            {
                Clients = clients
                Jobs = jobs
                Invoices = invoices
                InvoiceItems = invoiceItems
                Quotations = quotations
                JobTasks = jobTasks
                Applications = applications
                Expenses = expenses
                BillableHours = billableHours
                LocalAuthorities = localAuthorities
            }
            |> EntireDb.Make
        let serializer = JsonSerializer()
        serializer.Formatting <- Formatting.Indented
        use stream = new FileStream(jsonFile.FullName, FileMode.Create, FileAccess.Write)
        use writer = new StreamWriter(stream)
        serializer.Serialize(writer, entireDb)
    member __.ImportFromJsonFile(jsonFile:FileInfo) =
        let serializer = JsonSerializer()
        serializer.Formatting <- Formatting.Indented
        use stream = new FileStream(jsonFile.FullName, FileMode.Open, FileAccess.Read)
        use reader = new StreamReader(stream)
        let entireDb = serializer.Deserialize(reader, typeof<EntireDb>) |> unbox<EntireDb>
        let data = entireDb.Version1Data.Value
        data.Clients |> Seq.iter saveClient
        data.Jobs |> Seq.iter saveJob
        data.Invoices |> Seq.iter saveInvoice
        data.InvoiceItems |> Seq.iter saveInvoiceItem
        data.Quotations |> Seq.iter saveQuotation
        data.JobTasks |> Seq.iter saveJobTask
        data.Applications |> Seq.iter saveApplication
        data.Expenses |> Seq.iter saveExpense
        data.BillableHours |> Seq.iter saveBillableHour
        data.LocalAuthorities |> Seq.iter saveLocalAuthority



