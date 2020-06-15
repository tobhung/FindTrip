namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrdersss : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "StatusCheck");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "StatusCheck", c => c.Int(nullable: false));
        }
    }
}
