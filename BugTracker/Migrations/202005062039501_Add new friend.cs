namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addnewfriend : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.FriendAssociations", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.FriendAssociations", name: "IX_ApplicationUserId", newName: "IX_ApplicationUser_Id");
            AddColumn("dbo.FriendAssociations", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.FriendAssociations", "FriendId", c => c.String(maxLength: 128));
            CreateIndex("dbo.FriendAssociations", "UserId");
            CreateIndex("dbo.FriendAssociations", "FriendId");
            AddForeignKey("dbo.FriendAssociations", "FriendId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FriendAssociations", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendAssociations", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendAssociations", "FriendId", "dbo.AspNetUsers");
            DropIndex("dbo.FriendAssociations", new[] { "FriendId" });
            DropIndex("dbo.FriendAssociations", new[] { "UserId" });
            DropColumn("dbo.FriendAssociations", "FriendId");
            DropColumn("dbo.FriendAssociations", "UserId");
            RenameIndex(table: "dbo.FriendAssociations", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserId");
            RenameColumn(table: "dbo.FriendAssociations", name: "ApplicationUser_Id", newName: "ApplicationUserId");
        }
    }
}
