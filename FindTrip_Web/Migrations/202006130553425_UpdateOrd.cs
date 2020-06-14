namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DepartureTime1", c => c.String(maxLength: 200));
            AddColumn("dbo.Orders", "DepartureTime2", c => c.String());
            AddColumn("dbo.MessageReplies", "BuyerId", c => c.Int(nullable: false));
            AddColumn("dbo.MessageReplies", "PlannerId", c => c.Int(nullable: false));
            AddColumn("dbo.MessageReplies", "MessageContent", c => c.String());
            DropColumn("dbo.Orders", "DepartureTime");
            DropColumn("dbo.MessageReplies", "ReplyContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MessageReplies", "ReplyContent", c => c.String());
            AddColumn("dbo.Orders", "DepartureTime", c => c.String(maxLength: 200));
            DropColumn("dbo.MessageReplies", "MessageContent");
            DropColumn("dbo.MessageReplies", "PlannerId");
            DropColumn("dbo.MessageReplies", "BuyerId");
            DropColumn("dbo.Orders", "DepartureTime2");
            DropColumn("dbo.Orders", "DepartureTime1");
        }
    }
}
