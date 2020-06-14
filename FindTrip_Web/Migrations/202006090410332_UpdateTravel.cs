namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTravel : DbMigration
    {
        public override void Up()
        {
  
            DropForeignKey("dbo.Orders", "TravelPlan_id1", "dbo.TravelPlans");
    
      
    
            DropIndex("dbo.Orders", new[] { "TravelPlan_id1" });
            AddColumn("dbo.TravelPlans", "country", c => c.String());
            AddColumn("dbo.TravelPlans", "city", c => c.String());
            AlterColumn("dbo.Orders", "CreateOn", c => c.DateTime());
  
          

        }
        
        public override void Down()
        {
 
            AlterColumn("dbo.Orders", "CreateOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.TravelPlans", "city");
            DropColumn("dbo.TravelPlans", "country");
            CreateIndex("dbo.Orders", "TravelPlan_id1");


    

        }
    }
}
