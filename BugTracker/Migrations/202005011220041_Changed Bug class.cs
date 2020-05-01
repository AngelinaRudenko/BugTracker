namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBugclass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bugs", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bugs", "Headline", c => c.String());
            AddColumn("dbo.Bugs", "Severity", c => c.String());
            AddColumn("dbo.Bugs", "ErrorType", c => c.String());
            AddColumn("dbo.Bugs", "Description", c => c.String());
            AddColumn("dbo.Bugs", "ActualResult", c => c.String());
            AddColumn("dbo.Bugs", "ExpectedResult", c => c.String());
            DropColumn("dbo.Bugs", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bugs", "Name", c => c.String());
            DropColumn("dbo.Bugs", "ExpectedResult");
            DropColumn("dbo.Bugs", "ActualResult");
            DropColumn("dbo.Bugs", "Description");
            DropColumn("dbo.Bugs", "ErrorType");
            DropColumn("dbo.Bugs", "Severity");
            DropColumn("dbo.Bugs", "Headline");
            DropColumn("dbo.Bugs", "Date");
        }
    }
}
