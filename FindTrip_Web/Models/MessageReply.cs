using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class MessageReply
    {
        public int  id { get; set; }

        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public virtual Message MyMessage{ get; set; }

        public int BuyerId { get; set; }

        public int TravelPlanId { get; set;  }

        public int PlannerId { get; set;  }
        
        public string MessageContent { get; set; }




    }
}