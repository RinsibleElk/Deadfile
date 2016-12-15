namespace Deadfile.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationId = c.Int(nullable: false, identity: true),
                        LocalAuthority = c.String(nullable: false),
                        LocalAuthorityReference = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        EstimatedDecisionDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicationId);
            
            CreateTable(
                "dbo.BillableHours",
                c => new
                    {
                        BillableHourId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        Person = c.String(maxLength: 100),
                        HoursWorked = c.Int(nullable: false),
                        NetAmount = c.Double(nullable: false),
                        Notes = c.String(maxLength: 500),
                        CreationDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        InvoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.BillableHourId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 20),
                        FirstName = c.String(maxLength: 50),
                        MiddleNames = c.String(maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Company = c.String(maxLength: 100),
                        AddressFirstLine = c.String(nullable: false, maxLength: 200),
                        AddressSecondLine = c.String(maxLength: 200),
                        AddressThirdLine = c.String(maxLength: 200),
                        AddressPostCode = c.String(maxLength: 20),
                        PhoneNumber1 = c.String(),
                        PhoneNumber2 = c.String(),
                        PhoneNumber3 = c.String(),
                        EmailAddress = c.String(),
                        Status = c.Int(nullable: false),
                        Notes = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        GrossAmount = c.Double(nullable: false),
                        NetAmount = c.Double(nullable: false),
                        VatRate = c.Double(nullable: false),
                        VatValue = c.Double(nullable: false),
                        Status = c.Int(nullable: false),
                        InvoiceReference = c.Int(nullable: false),
                        Company = c.Int(nullable: false),
                        ClientName = c.String(nullable: false, maxLength: 100),
                        ClientAddressFirstLine = c.String(nullable: false, maxLength: 200),
                        ClientAddressSecondLine = c.String(maxLength: 200),
                        ClientAddressThirdLine = c.String(maxLength: 200),
                        ClientAddressPostCode = c.String(maxLength: 20),
                        Project = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 200),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceItemId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                        NetAmount = c.Double(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceItemId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobNumber = c.Int(nullable: false),
                        AddressFirstLine = c.String(nullable: false, maxLength: 200),
                        AddressSecondLine = c.String(maxLength: 200),
                        AddressThirdLine = c.String(maxLength: 200),
                        AddressPostCode = c.String(maxLength: 20),
                        Status = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 50),
                        Notes = c.String(maxLength: 500),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ExpenseId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        NetAmount = c.Double(nullable: false),
                        Notes = c.String(maxLength: 500),
                        CreationDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        InvoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.ExpenseId);
            
            CreateTable(
                "dbo.JobTasks",
                c => new
                    {
                        JobTaskId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 100),
                        Notes = c.String(maxLength: 500),
                        DueDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobTaskId);
            
            CreateTable(
                "dbo.LocalAuthorities",
                c => new
                    {
                        LocalAuthorityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.LocalAuthorityId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        GrossAmount = c.Double(nullable: false),
                        NetAmount = c.Double(nullable: false),
                        JobId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.JobId);
            
            CreateTable(
                "dbo.Quotations",
                c => new
                    {
                        QuotationId = c.Int(nullable: false, identity: true),
                        Phrase = c.String(nullable: false, maxLength: 200),
                        Author = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.QuotationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Jobs", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropIndex("dbo.Payments", new[] { "JobId" });
            DropIndex("dbo.Jobs", new[] { "ClientId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            DropTable("dbo.Quotations");
            DropTable("dbo.Payments");
            DropTable("dbo.LocalAuthorities");
            DropTable("dbo.JobTasks");
            DropTable("dbo.Expenses");
            DropTable("dbo.Employees");
            DropTable("dbo.Jobs");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.Clients");
            DropTable("dbo.BillableHours");
            DropTable("dbo.Applications");
        }
    }
}
