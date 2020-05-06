namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Smthwrong : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Friends", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Friends", new[] { "ApplicationUserId" });
            CreateTable(
                "dbo.FriendAssociations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsAFriend = c.Boolean(nullable: false),
                        ApplicationUserIdRequest = c.String(),
                        ApplicationUserIdAnswer = c.String(),
                        ApplicationUserAnswer_Id = c.String(maxLength: 128),
                        ApplicationUserRequest_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserAnswer_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserRequest_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUserAnswer_Id)
                .Index(t => t.ApplicationUserRequest_Id)
                .Index(t => t.ApplicationUser_Id);
            
            DropTable("dbo.Friends");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsFriend = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.FriendAssociations", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendAssociations", "ApplicationUserRequest_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendAssociations", "ApplicationUserAnswer_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FriendAssociations", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FriendAssociations", new[] { "ApplicationUserRequest_Id" });
            DropIndex("dbo.FriendAssociations", new[] { "ApplicationUserAnswer_Id" });
            DropTable("dbo.FriendAssociations");
            CreateIndex("dbo.Friends", "ApplicationUserId");
            AddForeignKey("dbo.Friends", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
