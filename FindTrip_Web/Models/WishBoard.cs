using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class WishBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [MaxLength(200)]
        public string Img { get; set; }

        [MaxLength(200)]
        public string Comment1 { get; set; }
        [MaxLength(200)]
        public string Comment2 { get; set; }


        public int CommentTotal { get; set; }

        public int LikeTotal { get; set; }

   
        public DateTime? CreateOn { get; set; }

        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual Member MyMember { get; set; }

        //public virtual ICollection<WishBoardReply> WishBoardReplies { get; set; }
    }
}