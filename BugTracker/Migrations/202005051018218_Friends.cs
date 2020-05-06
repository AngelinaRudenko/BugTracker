namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friends : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsFriend = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Friends", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Friends", new[] { "ApplicationUserId" });
            DropTable("dbo.Friends");
        }
    }
}
