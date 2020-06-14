using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Message
    {
        [Key]
       
        public int id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int MemberId { get; set; }

        public int TravelPlanId { get; set; }
        public int PlannerId { get; set; }

        public string Destination { get; set;  }

        public DateTime CreateOn { get; set;  }
        //private ICollection<Member> Members { get; set; }

        public virtual ICollection<MessageReply> MessageReplies { get; set; }


    }
}