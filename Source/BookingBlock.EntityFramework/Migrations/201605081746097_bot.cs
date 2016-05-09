namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessOpeningTimes", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessOpeningTimes", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessOpeningTimes", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessOpeningTimes", "Deleted");
            DropColumn("dbo.BusinessOpeningTimes", "Modified");
            DropColumn("dbo.BusinessOpeningTimes", "Created");
        }
    }
}
