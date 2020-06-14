using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class MessageList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int MemberId { get; set; }

        public virtual Member MyMember { get; set; }

        [MaxLength(200)]

        public string MessageContent { get; set;  }

        public DateTime CreateOn { get; set; }

    }
}