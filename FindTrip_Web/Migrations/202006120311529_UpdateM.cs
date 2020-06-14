namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateM : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        MemberId = c.Int(nullable: false),
                        TravelPlanId = c.Int(nullable: false),
                        Destination = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
