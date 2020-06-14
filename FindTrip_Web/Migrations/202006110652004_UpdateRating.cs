namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "MemberId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "MemberId");
        }
    }
}
