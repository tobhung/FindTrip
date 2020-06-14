namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateMessages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MessageLists", "MemberId", "dbo.Members");
            DropIndex("dbo.MessageLists", new[] { "MemberId" });
            AlterColumn("dbo.PointsHistories", "CreateOn", c => c.DateTime());
            AlterColumn("dbo.MessageLists", "MessageContent", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MessageLists", "MessageContent", c => c.String(maxLength: 200));
            AlterColumn("dbo.PointsHistories", "CreateOn", c => c.DateTime(nullable: false));
            CreateIndex("dbo.MessageLists", "MemberId");
            AddForeignKey("dbo.MessageLists", "MemberId", "dbo.Members", "id", cascadeDelete: true);
        }
    }
}
