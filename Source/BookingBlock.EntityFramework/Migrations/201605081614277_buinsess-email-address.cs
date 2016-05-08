namespace BookingBlock.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buinsessemailaddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "EmailAddress");
        }
    }
}
