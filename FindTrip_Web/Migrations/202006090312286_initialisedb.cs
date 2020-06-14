namespace FindTrip_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialisedb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        country = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        city = c.String(maxLength: 50),
                        Cid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Countries", t => t.Cid, cascadeDelete: true)
                .Index(t => t.Cid);
            
            CreateTable(
                "dbo.TravelPlans",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        TravelPlanIntro = c.String(maxLength: 200),
                        TPBGImg = c.String(maxLength: 200),
                        Cpicture = c.String(maxLength: 200),
                        CountryId = c.Int(nullable: false),
                        District = c.String(),
                        Act = c.Boolean(nullable: false),
                        Secret = c.Boolean(nullable: false),
                        Culture = c.Boolean(nullable: false),
                        Food = c.Boolean(nullable: false),
                        Shopping = c.Boolean(nullable: false),
                        Religion = c.Boolean(nullable: false),
                        TPPrice = c.Int(nullable: false),
                        TPExperience = c.String(maxLength: 200),
                        CreateOn = c.DateTime(),
                        Rating_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.Ratings", t => t.Rating_id)
                .Index(t => t.MemberId)
                .Index(t => t.CountryId)
                .Index(t => t.Rating_id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 50),
                        Account = c.String(maxLength: 100),
                        Password = c.String(maxLength: 50),
                        PasswordSalt = c.String(),
                        Email = c.String(),
                        Tel = c.String(maxLength: 50),
                        manpic = c.String(maxLength: 200),
                        BGImg = c.String(maxLength: 200),
                        TravelPlanLike = c.String(),
                        points = c.Int(nullable: false),
                        PlannerSocial1 = c.String(maxLength: 200),
                        PlannerSocial2 = c.String(maxLength: 200),
                        RatingSum = c.Int(nullable: false),
                        RatingAvg = c.Int(nullable: false),
                        CreateOn = c.DateTime(),
                        MemberIntro = c.String(maxLength: 50),
                        AuthCode = c.String(maxLength: 36),
                        CheckAuth = c.String(),
                        Permission = c.String(),
                        PlannerName = c.String(),
                        PlannerSocial3 = c.String(),
                        PlannerSocial4 = c.String(),
                        PlannerTel = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        DepartureTime = c.String(maxLength: 200),
                        Budget = c.Int(nullable: false),
                        Adult = c.Int(nullable: false),
                        Children = c.Int(nullable: false),
                        Remark = c.String(maxLength: 200),
                        PointsLeft = c.Int(),
                        CreateOn = c.DateTime(nullable: false),
                        MemberId = c.Int(nullable: false),
                        country = c.String(),
                        city = c.String(),
                        TravelPlan_id = c.Int(nullable: false),
                        TravelPlan_id1 = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.TravelPlans", t => t.TravelPlan_id1)
                .Index(t => t.MemberId)
                .Index(t => t.TravelPlan_id1);
            
            CreateTable(
                "dbo.PointsHistories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        Product = c.String(maxLength: 50),
                        CreateOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.WishBoardReplies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        NewComment = c.String(maxLength: 200),
                        CreateOn = c.DateTime(nullable: false),
                        MemberId = c.Int(nullable: false),
                        WishBoard_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.WishBoards", t => t.WishBoard_id)
                .Index(t => t.MemberId)
                .Index(t => t.WishBoard_id);
            
            CreateTable(
                "dbo.WishBoards",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Img = c.String(maxLength: 200),
                        Comment1 = c.String(maxLength: 200),
                        Comment2 = c.String(maxLength: 200),
                        CommentTotal = c.Int(nullable: false),
                        LikeTotal = c.Int(nullable: false),
                        CreateOn = c.DateTime(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MessageLists",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        MessageContent = c.String(maxLength: 200),
                        CreateOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RatingContent = c.String(maxLength: 200),
                        CreateOn = c.DateTime(),
                        star = c.Int(nullable: false),
                        rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelPlans", "Rating_id", "dbo.Ratings");
            DropForeignKey("dbo.MessageLists", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Orders", "TravelPlan_id1", "dbo.TravelPlans");
            DropForeignKey("dbo.WishBoardReplies", "WishBoard_id", "dbo.WishBoards");
            DropForeignKey("dbo.WishBoards", "MemberId", "dbo.Members");
            DropForeignKey("dbo.WishBoardReplies", "MemberId", "dbo.Members");
            DropForeignKey("dbo.TravelPlans", "MemberId", "dbo.Members");
            DropForeignKey("dbo.PointsHistories", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Orders", "MemberId", "dbo.Members");
            DropForeignKey("dbo.TravelPlans", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Districts", "Cid", "dbo.Countries");
            DropIndex("dbo.MessageLists", new[] { "MemberId" });
            DropIndex("dbo.WishBoards", new[] { "MemberId" });
            DropIndex("dbo.WishBoardReplies", new[] { "WishBoard_id" });
            DropIndex("dbo.WishBoardReplies", new[] { "MemberId" });
            DropIndex("dbo.PointsHistories", new[] { "MemberId" });
            DropIndex("dbo.Orders", new[] { "TravelPlan_id1" });
            DropIndex("dbo.Orders", new[] { "MemberId" });
            DropIndex("dbo.TravelPlans", new[] { "Rating_id" });
            DropIndex("dbo.TravelPlans", new[] { "CountryId" });
            DropIndex("dbo.TravelPlans", new[] { "MemberId" });
            DropIndex("dbo.Districts", new[] { "Cid" });
            DropTable("dbo.Ratings");
            DropTable("dbo.MessageLists");
            DropTable("dbo.WishBoards");
            DropTable("dbo.WishBoardReplies");
            DropTable("dbo.PointsHistories");
            DropTable("dbo.Orders");
            DropTable("dbo.Members");
            DropTable("dbo.TravelPlans");
            DropTable("dbo.Districts");
            DropTable("dbo.Countries");
        }
    }
}
