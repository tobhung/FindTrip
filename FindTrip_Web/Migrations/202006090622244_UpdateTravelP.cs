namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTravelP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.TravelPlans", "points", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelPlans", "points");
            DropColumn("dbo.Orders", "Status");
        }
    }
}
