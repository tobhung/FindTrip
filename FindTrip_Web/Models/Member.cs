using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        [MaxLength(50)]
        public string name { get; set; }

        //login
        [MaxLength(100)]
        public string Account { get; set; }

        [MaxLength(50)]

        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        public string PasswordSalt { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Tel { get; set; }

        [MaxLength(200)]
        public string manpic { get; set; }

        [MaxLength(200)]
        public string BGImg { get; set; }

        public string TravelPlanLike { get; set; }

        public int points { get; set; }

        [MaxLength(200)]
        public string PlannerSocial1 { get; set; }
        [MaxLength(200)]
        public string PlannerSocial2 { get; set; }
        public int RatingSum { get; set; }
        public int RatingAvg { get; set; }
        
        [DatabaseGenerated(databaseGeneratedOption:DatabaseGeneratedOption.Computed)]
        public DateTime? CreateOn { get; set; }

        [MaxLength(50)]
        public string MemberIntro { get; set; }
        
        [MaxLength(36)]
        public string AuthCode { get; set; }


        public string CheckAuth { get; set; }

        public string Permission { get; set; }
  

        //above coloumns for general users

        public string PlannerName { get; set; }

        public string PlannerSocial3 { get; set; }

        public string PlannerSocial4 { get; set; }
        
        public string PlannerTel { get; set; }


        public virtual ICollection<TravelPlan> TravelPlans { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<WishBoard> WishBoards { get; set; }
        public virtual ICollection<WishBoardReply> WishBoardReplies { get; set; }

        public virtual ICollection<PointsHistory> PointsHistories { get; set;  }



    }
}