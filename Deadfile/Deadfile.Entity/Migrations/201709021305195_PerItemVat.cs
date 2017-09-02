namespace Deadfile.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PerItemVat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "VatValue", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceItems", "VatValue");
        }
    }
}
