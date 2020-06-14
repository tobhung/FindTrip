namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "PlannerIntro", c => c.String());
            AlterColumn("dbo.WishBoards", "CreateOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WishBoards", "CreateOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.Members", "PlannerIntro");
        }
    }
}
