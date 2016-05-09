namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Deleted", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "IsDummy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "IsDummy", c => c.Boolean(nullable: false));
            DropColumn("dbo.AspNetUsers", "Deleted");
            DropColumn("dbo.AspNetUsers", "Modified");
        }
    }
}
