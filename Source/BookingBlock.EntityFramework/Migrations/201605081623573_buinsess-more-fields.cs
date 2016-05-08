namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buinsessmorefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Businesses", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Businesses", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "Deleted");
            DropColumn("dbo.Businesses", "Modified");
            DropColumn("dbo.Businesses", "Created");
        }
    }
}
