namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessUsers", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessUsers", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.BusinessUsers", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessUsers", "Deleted");
            DropColumn("dbo.BusinessUsers", "Modified");
            DropColumn("dbo.BusinessUsers", "Created");
        }
    }
}
