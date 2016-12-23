namespace Deadfile.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnbilledJobs : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BillableHours", "JobId");
            CreateIndex("dbo.Expenses", "JobId");
            AddForeignKey("dbo.BillableHours", "JobId", "dbo.Jobs", "JobId", cascadeDelete: true);
            AddForeignKey("dbo.Expenses", "JobId", "dbo.Jobs", "JobId", cascadeDelete: true);
            DropColumn("dbo.JobTasks", "SomeRandomField");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobTasks", "SomeRandomField", c => c.Int(nullable: false));
            DropForeignKey("dbo.Expenses", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.BillableHours", "JobId", "dbo.Jobs");
            DropIndex("dbo.Expenses", new[] { "JobId" });
            DropIndex("dbo.BillableHours", new[] { "JobId" });
        }
    }
}
