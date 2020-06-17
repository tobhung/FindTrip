namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWishBoardReply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WishBoardReplies", "Like", c => c.Int());
            AlterColumn("dbo.WishBoards", "LikeTotal", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WishBoards", "LikeTotal", c => c.Int(nullable: false));
            DropColumn("dbo.WishBoardReplies", "Like");
        }
    }
}
