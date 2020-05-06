namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoooraaayIrealizedthepoint2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FriendAssociations", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.FriendAssociations", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.FriendAssociations", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.FriendAssociations", name: "ApplicationUserId", newName: "ApplicationUser_Id");
        }
    }
}
