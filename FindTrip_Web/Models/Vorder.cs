using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindTrip_Web.Models
{
    public class Vorder
    {
        public int PlanId { get; set;  }

        public int Budget { get; set;  }

        public int Adult { get; set; }

        public int Children { get; set;  }

        public string Remark { get; set; }
    }
}