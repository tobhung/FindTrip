namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateor : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Budget", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Budget", c => c.Int(nullable: false));
        }
    }
}
