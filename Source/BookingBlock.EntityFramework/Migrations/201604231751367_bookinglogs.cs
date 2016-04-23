namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookinglogs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceLogs", "SeviceId", "dbo.Services");
            DropForeignKey("dbo.Reviews", "Booking_Id", "dbo.Bookings");
            DropIndex("dbo.ServiceLogs", new[] { "SeviceId" });
            DropIndex("dbo.Reviews", new[] { "Booking_Id" });
            CreateTable(
                "dbo.BookingLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BookingId = c.Guid(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        Entry = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
            DropColumn("dbo.Reviews", "Booking_Id");
            DropTable("dbo.ServiceLogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SeviceId = c.Guid(nullable: false),
                        EntryDateTime = c.DateTime(nullable: false),
                        Entry = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Reviews", "Booking_Id", c => c.Guid());
            DropForeignKey("dbo.BookingLogs", "BookingId", "dbo.Bookings");
            DropIndex("dbo.BookingLogs", new[] { "BookingId" });
            DropTable("dbo.BookingLogs");
            CreateIndex("dbo.Reviews", "Booking_Id");
            CreateIndex("dbo.ServiceLogs", "SeviceId");
            AddForeignKey("dbo.Reviews", "Booking_Id", "dbo.Bookings", "Id");
            AddForeignKey("dbo.ServiceLogs", "SeviceId", "dbo.Services", "Id", cascadeDelete: true);
        }
    }
}
