namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessTypes", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessTypes", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessTypes", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessTypes", "Deleted");
            DropColumn("dbo.BusinessTypes", "Modified");
            DropColumn("dbo.BusinessTypes", "Created");
        }
    }
}
