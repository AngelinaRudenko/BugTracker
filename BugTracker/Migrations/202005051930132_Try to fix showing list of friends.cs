namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Trytofixshowinglistoffriends : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FriendAssociations", "ApplicationUserIdRequest");
            DropColumn("dbo.FriendAssociations", "ApplicationUserIdAnswer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FriendAssociations", "ApplicationUserIdAnswer", c => c.String());
            AddColumn("dbo.FriendAssociations", "ApplicationUserIdRequest", c => c.String());
        }
    }
}
