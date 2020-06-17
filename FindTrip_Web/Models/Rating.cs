using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Rating
    {
        [Key]
       
        public int id { get; set; }

        public int OrderId { get; set; }
        public virtual  Order MyOrder { get; set; }
        public string RatingContent { get; set; }

        public int MemberId { get; set; }

        public int TravelId { get; set; }

        //[ForeignKey("TravelId")]
        //public virtual TravelPlan MyTravelPlan { get; set; }

        public DateTime? CreateOn { get; set; }

        public double StarAmount { get; set; }
        public int Ratingtotal { get; set; }

        public double? star{ get; set; }//average

        public int? rating { get; set; }//total

        //public virtual ICollection<TravelPlan> TravelPlans { get; set; }
      
    }
}