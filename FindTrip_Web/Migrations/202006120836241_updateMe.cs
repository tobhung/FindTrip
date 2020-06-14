namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "PlannerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "PlannerId");
        }
    }
}
