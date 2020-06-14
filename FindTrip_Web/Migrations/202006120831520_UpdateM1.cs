namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateM1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageReplies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        TravelPlanId = c.Int(nullable: false),
                        ReplyContent = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            AddColumn("dbo.Messages", "CreateOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageReplies", "MessageId", "dbo.Messages");
            DropIndex("dbo.MessageReplies", new[] { "MessageId" });
            DropColumn("dbo.Messages", "CreateOn");
            DropTable("dbo.MessageReplies");
        }
    }
}
