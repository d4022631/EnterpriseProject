using System.Data.Entity.Migrations;

namespace BookingBlock.EntityFramework.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ServiceId = c.Guid(nullable: false),
                        CustomerId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Notes = c.String(),
                        Confirmed = c.Boolean(nullable: false),
                        Cancelled = c.Boolean(nullable: false),
                        Amended = c.Boolean(nullable: false),
                        Attended = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Postcode = c.String(),
                        Address = c.String(),
                        Location = c.Geography(),
                        Gender = c.Int(nullable: false),
                        IsDummy = c.Boolean(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.BusinessUsers",
                c => new
                    {
                        BusinessId = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusinessId, t.UserId })
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BusinessId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Postcode = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        FaxNumber = c.String(),
                        Website = c.String(),
                        Facebook = c.String(),
                        LinkedIn = c.String(),
                        GooglePlus = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        LogoUrl = c.String(),
                        Location = c.Geography(nullable: false),
                        BusinessTypeId = c.Guid(nullable: false),
                        IsDummy = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessTypes", t => t.BusinessTypeId, cascadeDelete: true)
                .Index(t => t.BusinessTypeId);
            
            CreateTable(
                "dbo.BusinessTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BusinessOpeningTimes",
                c => new
                    {
                        BusinessId = c.Guid(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        OpeningTime = c.Time(nullable: false, precision: 7),
                        ClosingTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.BusinessId, t.DayOfWeek })
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .Index(t => t.BusinessId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BusinessId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Duration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .Index(t => t.BusinessId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BusinessId = c.Guid(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comments = c.String(nullable: false),
                        Booking_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .ForeignKey("dbo.Bookings", t => t.Booking_Id)
                .Index(t => t.BusinessId)
                .Index(t => t.Booking_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.Reviews", "Booking_Id", "dbo.Bookings");
            DropForeignKey("dbo.Reviews", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.Bookings", "CustomerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BusinessUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BusinessUsers", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.Services", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.BusinessOpeningTimes", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.Businesses", "BusinessTypeId", "dbo.BusinessTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reviews", new[] { "Booking_Id" });
            DropIndex("dbo.Reviews", new[] { "BusinessId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Services", new[] { "BusinessId" });
            DropIndex("dbo.BusinessOpeningTimes", new[] { "BusinessId" });
            DropIndex("dbo.Businesses", new[] { "BusinessTypeId" });
            DropIndex("dbo.BusinessUsers", new[] { "UserId" });
            DropIndex("dbo.BusinessUsers", new[] { "BusinessId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", new[] { "CustomerId" });
            DropIndex("dbo.Bookings", new[] { "ServiceId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Reviews");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Services");
            DropTable("dbo.BusinessOpeningTimes");
            DropTable("dbo.BusinessTypes");
            DropTable("dbo.Businesses");
            DropTable("dbo.BusinessUsers");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bookings");
        }
    }
}
