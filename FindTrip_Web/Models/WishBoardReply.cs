using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class WishBoardReply
    {
        [Key]
        
        public int id { get; set; }
        [MaxLength(200)]
        public string NewComment { get; set; }
      
        public int Rid { get; set; }
        //[ForeignKey("Rid")]
        //public virtual WishBoard MyWishBoard { get; set; }

        public DateTime? CreateOn { get; set; }
        public int MemberId { get; set; }
        public virtual Member MyMember { get; set; }

        public int? Like { get; set; }

    }
}