namespace Deadfile.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomeRandomField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobTasks", "SomeRandomField", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobTasks", "SomeRandomField");
        }
    }
}
