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
        public int id { get; set; }

        public int MemberId { get; set; }
        //public virtual Member MyMember { get; set; }

        public string MessageContent { get; set;  }

        public DateTime CreateOn { get; set; }

    }
}