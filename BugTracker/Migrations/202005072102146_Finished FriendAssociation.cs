namespace BugTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinishedFriendAssociation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FriendAssociations", "FriendId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FriendAssociations", "FriendId");
        }
    }
}
