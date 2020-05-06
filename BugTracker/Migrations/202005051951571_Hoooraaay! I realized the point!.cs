namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HoooraaayIrealizedthepoint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FriendAssociations", "ApplicationUserAnswer_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendAssociations", "ApplicationUserRequest_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FriendAssociations", new[] { "ApplicationUserAnswer_Id" });
            DropIndex("dbo.FriendAssociations", new[] { "ApplicationUserRequest_Id" });
            DropColumn("dbo.FriendAssociations", "ApplicationUserAnswer_Id");
            DropColumn("dbo.FriendAssociations", "ApplicationUserRequest_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendAssociations", "ApplicationUserRequest_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.FriendAssociations", "ApplicationUserAnswer_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.FriendAssociations", "ApplicationUserRequest_Id");
            CreateIndex("dbo.FriendAssociations", "ApplicationUserAnswer_Id");
            AddForeignKey("dbo.FriendAssociations", "ApplicationUserRequest_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.FriendAssociations", "ApplicationUserAnswer_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
