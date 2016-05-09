namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookingsfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "Modified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bookings", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Deleted");
            DropColumn("dbo.Bookings", "Modified");
            DropColumn("dbo.Bookings", "Created");
        }
    }
}
