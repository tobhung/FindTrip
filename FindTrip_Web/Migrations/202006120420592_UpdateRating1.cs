namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRating1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "StarAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Ratings", "Ratingtotal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "Ratingtotal");
            DropColumn("dbo.Ratings", "StarAmount");
        }
    }
}
