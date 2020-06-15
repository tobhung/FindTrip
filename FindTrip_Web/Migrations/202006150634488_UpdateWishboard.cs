namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWishboard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishBoardReplies", "Rid", c => c.Int(nullable: false));
            AlterColumn("dbo.WishBoardReplies", "CreateOn", c => c.DateTime());
            CreateIndex("dbo.WishBoardReplies", "Rid");
            AddForeignKey("dbo.WishBoardReplies", "Rid", "dbo.WishBoardReplies", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishBoardReplies", "Rid", "dbo.WishBoardReplies");
            DropIndex("dbo.WishBoardReplies", new[] { "Rid" });
            AlterColumn("dbo.WishBoardReplies", "CreateOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.WishBoardReplies", "Rid");
        }
    }
}
