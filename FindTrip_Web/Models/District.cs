using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }


        [MaxLength(50)]
        public string city { get; set; }

        //FK
      
        public int Cid{ get; set; }
        [ForeignKey("Cid")]
        public virtual Countries MyCountry { get; set; }
      
    }
}