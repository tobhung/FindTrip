namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "StatusCheck", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "StatusCheck");
        }
    }
}
