namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Thirdmigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Bugs", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Bugs", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Bugs", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Bugs", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
