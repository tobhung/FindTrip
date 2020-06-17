namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRating2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Ratings", "RatingContent", c => c.String());
            CreateIndex("dbo.Ratings", "OrderId");
            AddForeignKey("dbo.Ratings", "OrderId", "dbo.Orders", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "OrderId", "dbo.Orders");
            DropIndex("dbo.Ratings", new[] { "OrderId" });
            AlterColumn("dbo.Ratings", "RatingContent", c => c.String(maxLength: 200));
            DropColumn("dbo.Ratings", "OrderId");
        }
    }
}
