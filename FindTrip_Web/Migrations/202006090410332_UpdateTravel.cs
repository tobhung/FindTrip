namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTravel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelPlans", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Orders", "TravelPlan_id1", "dbo.TravelPlans");
            DropForeignKey("dbo.TravelPlans", "Rating_id", "dbo.Ratings");
            DropIndex("dbo.TravelPlans", new[] { "CountryId" });
            DropIndex("dbo.TravelPlans", new[] { "Rating_id" });
            DropIndex("dbo.Orders", new[] { "TravelPlan_id1" });
            AddColumn("dbo.TravelPlans", "country", c => c.String());
            AddColumn("dbo.TravelPlans", "city", c => c.String());
            AlterColumn("dbo.Orders", "CreateOn", c => c.DateTime());
            DropColumn("dbo.TravelPlans", "CountryId");
            DropColumn("dbo.TravelPlans", "Rating_id");
            DropColumn("dbo.Orders", "TravelPlan_id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "TravelPlan_id1", c => c.Int());
            AddColumn("dbo.TravelPlans", "Rating_id", c => c.Int());
            AddColumn("dbo.TravelPlans", "CountryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "CreateOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.TravelPlans", "city");
            DropColumn("dbo.TravelPlans", "country");
            CreateIndex("dbo.Orders", "TravelPlan_id1");
            CreateIndex("dbo.TravelPlans", "Rating_id");
            CreateIndex("dbo.TravelPlans", "CountryId");
            AddForeignKey("dbo.TravelPlans", "Rating_id", "dbo.Ratings", "id");
            AddForeignKey("dbo.Orders", "TravelPlan_id1", "dbo.TravelPlans", "id");
            AddForeignKey("dbo.TravelPlans", "CountryId", "dbo.Countries", "id", cascadeDelete: true);
        }
    }
}
