namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WishBoardReplies", "Rid", "dbo.WishBoardReplies");
            DropForeignKey("dbo.WishBoardReplies", "WishBoard_id", "dbo.WishBoards");
            DropForeignKey("dbo.MessageReplies", "MessageId", "dbo.Messages");
            DropIndex("dbo.WishBoardReplies", new[] { "Rid" });
            DropIndex("dbo.WishBoardReplies", new[] { "WishBoard_id" });
            DropIndex("dbo.MessageReplies", new[] { "MessageId" });
            AddColumn("dbo.MessageReplies", "MemberId", c => c.Int(nullable: false));
            AlterColumn("dbo.WishBoards", "LikeTotal", c => c.Int(nullable: false));
            CreateIndex("dbo.MessageReplies", "MemberId");
            CreateIndex("dbo.Messages", "MemberId");
            AddForeignKey("dbo.MessageReplies", "MemberId", "dbo.Members", "id", cascadeDelete: true);
            AddForeignKey("dbo.Messages", "MemberId", "dbo.Members", "id", cascadeDelete: true);
            DropColumn("dbo.WishBoardReplies", "WishBoard_id");
            DropColumn("dbo.MessageReplies", "BuyerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MessageReplies", "BuyerId", c => c.Int(nullable: false));
            AddColumn("dbo.WishBoardReplies", "WishBoard_id", c => c.Int());
            DropForeignKey("dbo.Messages", "MemberId", "dbo.Members");
            DropForeignKey("dbo.MessageReplies", "MemberId", "dbo.Members");
            DropIndex("dbo.Messages", new[] { "MemberId" });
            DropIndex("dbo.MessageReplies", new[] { "MemberId" });
            AlterColumn("dbo.WishBoards", "LikeTotal", c => c.Int());
            DropColumn("dbo.MessageReplies", "MemberId");
            CreateIndex("dbo.MessageReplies", "MessageId");
            CreateIndex("dbo.WishBoardReplies", "WishBoard_id");
            CreateIndex("dbo.WishBoardReplies", "Rid");
            AddForeignKey("dbo.MessageReplies", "MessageId", "dbo.Messages", "id", cascadeDelete: true);
            AddForeignKey("dbo.WishBoardReplies", "WishBoard_id", "dbo.WishBoards", "id");
            AddForeignKey("dbo.WishBoardReplies", "Rid", "dbo.WishBoardReplies", "id");
        }
    }
}
