using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Countries
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(50)]
        public string country { get; set; }

        //public virtual ICollection<TravelPlan> TravelPlans { get; set; }

        public virtual ICollection<District> Districts { get; set; }

    }
}