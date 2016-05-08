namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeddummyflag : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Businesses", "IsDummy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Businesses", "IsDummy", c => c.Boolean(nullable: false));
        }
    }
}
