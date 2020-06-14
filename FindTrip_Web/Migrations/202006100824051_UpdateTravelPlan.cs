namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTravelPlan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelPlans", "star", c => c.Int(nullable: false));
            AddColumn("dbo.TravelPlans", "RatingContent", c => c.String());
            AddColumn("dbo.TravelPlans", "rating", c => c.Int(nullable: false));
            AlterColumn("dbo.Members", "CreateOn", c => c.DateTime());
            AlterColumn("dbo.TravelPlans", "CreateOn", c => c.DateTime());
            AlterColumn("dbo.Ratings", "CreateOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "CreateOn", c => c.DateTime());
            AlterColumn("dbo.TravelPlans", "CreateOn", c => c.DateTime());
            AlterColumn("dbo.Members", "CreateOn", c => c.DateTime());
            DropColumn("dbo.TravelPlans", "rating");
            DropColumn("dbo.TravelPlans", "RatingContent");
            DropColumn("dbo.TravelPlans", "star");
        }
    }
}
