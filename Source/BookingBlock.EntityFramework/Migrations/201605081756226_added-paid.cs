namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedpaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Paid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Paid");
        }
    }
}
