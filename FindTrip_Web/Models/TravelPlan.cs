using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class TravelPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member MyMember { get; set; }

        [MaxLength(200)]
        public string TravelPlanIntro { get; set; }

        [MaxLength(200)]
        public string TPBGImg { get; set; }

        [MaxLength(200)]
        public string Cpicture { get; set; }

        //FK Country
        //public int CountryId { get; set; }

        //[ForeignKey("CountryId")]
        //public virtual Countries MyCountry { get; set; }

        //FK Country

        public string country { get; set; }
        public string city { get; set; }
        public string District { get; set; }

        //[ForeignKey("DistrictId")]
        //public virtual District MyDistrict { get; set; }

        //public int RatingId { get; set; }
        //[ForeignKey("RatingId")]
        //public virtual Rating MyRating { get; set; }


        public bool Act { get; set;  }
        public bool Secret { get; set;  }
        public bool Culture { get; set; }
        public bool Food { get; set;  }
        public bool Shopping { get; set; }

        public bool Religion { get; set; }

        public int points { get; set;  }
        public int TPPrice { get; set; }
        [MaxLength(200)]
        public string TPExperience { get; set; }

        //[DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Computed)]
        public DateTime? CreateOn { get; set; }


        //public virtual ICollection<Order> Orders { get; set; }

        //public virtual ICollection<Rating> Ratings { get; set; }

        

    }
}