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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(200)]
        public string RatingContent { get; set; }

        //public int TravelId { get; set; }

        //[ForeignKey("TravelId")]
        //public virtual TravelPlan MyTravelPlan { get; set; }


        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Computed)]
        public DateTime? CreateOn { get; set; }

        public int star{ get; set; }

        public int rating { get; set; }

        //public virtual ICollection<TravelPlan> TravelPlans { get; set; }
    }
}