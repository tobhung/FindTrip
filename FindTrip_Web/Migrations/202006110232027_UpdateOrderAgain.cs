namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PlannerId", c => c.Int(nullable: false));
            AddColumn("dbo.Ratings", "TravelId", c => c.Int(nullable: false));
            DropColumn("dbo.TravelPlans", "star");
            DropColumn("dbo.TravelPlans", "RatingContent");
            DropColumn("dbo.TravelPlans", "rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TravelPlans", "rating", c => c.Int(nullable: false));
            AddColumn("dbo.TravelPlans", "RatingContent", c => c.String());
            AddColumn("dbo.TravelPlans", "star", c => c.Int(nullable: false));
            DropColumn("dbo.Ratings", "TravelId");
            DropColumn("dbo.Orders", "PlannerId");
        }
    }
}
