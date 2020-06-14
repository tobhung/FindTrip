namespace FindTrip_Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Model1 : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'Model1' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'FindTrip_Web.Models.Model1' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'Model1' 連接字串。
        public Model1()
            : base("name=Model1")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<TravelPlan> TravelPlans { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<WishBoard> WishBoards { get; set; }
        public virtual DbSet<WishBoardReply> WishBoardReplies { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }

        public virtual DbSet<MessageList> MessageLists { get; set; }

        public virtual DbSet<PointsHistory> PointsHistories { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<MessageReply> MessageReplies { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}