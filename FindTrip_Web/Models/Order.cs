using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(200)]
        public string DepartureTime1 { get; set; }

        public string DepartureTime2 { get; set; }


        public string Budget { get; set; }

        public int Adult { get; set; }

        public int Children { get; set; }

        [MaxLength(200)]
        public string Remark { get; set; }

        public int? PointsLeft { get; set; }

        public DateTime? CreateOn { get; set; }

        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member MyMember { get; set; }

        public string country { get; set; }
        public string city { get; set; }

        public int TravelPlan_id { get; set; }

        public int PlannerId { get; set; }
        public int Status { get; set;  }
        //public EnumList.OrderStatus Status { get; set; }

        public bool Act { get; set; }
        public bool Secret { get; set; }
        public bool Culture { get; set; }
        public bool Food { get; set; }
        public bool Shopping { get; set; }

        public bool Religion { get; set; }


    }
}