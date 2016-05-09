namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reviewfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reviews", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reviews", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Deleted");
            DropColumn("dbo.Reviews", "Modified");
            DropColumn("dbo.Reviews", "Created");
        }
    }
}
