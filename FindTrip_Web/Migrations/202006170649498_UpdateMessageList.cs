namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessageList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageLists", "MessageId", c => c.Int(nullable: false));
            AlterColumn("dbo.MessageLists", "CreateOn", c => c.DateTime());
            CreateIndex("dbo.MessageLists", "MemberId");
            AddForeignKey("dbo.MessageLists", "MemberId", "dbo.Members", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageLists", "MemberId", "dbo.Members");
            DropIndex("dbo.MessageLists", new[] { "MemberId" });
            AlterColumn("dbo.MessageLists", "CreateOn", c => c.DateTime(nullable: false));
            DropColumn("dbo.MessageLists", "MessageId");
        }
    }
}
