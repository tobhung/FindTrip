namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Act", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Secret", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Culture", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Food", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Shopping", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "Religion", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Ratings", "star", c => c.Double());
            AlterColumn("dbo.Ratings", "rating", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ratings", "rating", c => c.Int(nullable: false));
            AlterColumn("dbo.Ratings", "star", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Religion");
            DropColumn("dbo.Orders", "Shopping");
            DropColumn("dbo.Orders", "Food");
            DropColumn("dbo.Orders", "Culture");
            DropColumn("dbo.Orders", "Secret");
            DropColumn("dbo.Orders", "Act");
        }
    }
}
