namespace Deadfile.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemVatRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "VatRate", c => c.Double(nullable: false));
            AddColumn("dbo.InvoiceItems", "IncludeVat", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "IncludeVat");
            DropColumn("dbo.InvoiceItems", "VatRate");
        }
    }
}
