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
        public string DepartureTime { get; set; }
        public int Budget { get; set; }

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
      
    }
}