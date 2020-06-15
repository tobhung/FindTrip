using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FindTrip_Web.common;

namespace FindTrip_Web.Models
{

    public class PatchMember : AbstractPatchStateRequest<PatchMember, Member>
    {

        public PatchMember()
        {
            AddPatchStateMapping(x => x.name);
            AddPatchStateMapping(x => x.Password);
            AddPatchStateMapping(x => x.MemberIntro);
            AddPatchStateMapping(x => x.PlannerIntro);
            AddPatchStateMapping(x => x.PlannerName);
            AddPatchStateMapping(x => x.PlannerSocial1);
            AddPatchStateMapping(x => x.PlannerSocial2);
            AddPatchStateMapping(x => x.PlannerSocial3);
            AddPatchStateMapping(x => x.PlannerSocial4);
            AddPatchStateMapping(x => x.Tel);
            AddPatchStateMapping(x => x.PlannerTel);


        }
        //public int id{ get; set; }
        public string name { get; set; }

        public string Password { get; set; }

        public string MemberIntro { get; set; }

        public string PlannerName { get; set; }

        public string PlannerSocial1 { get; set; }

        public string PlannerSocial2 { get; set; }

        public string PlannerSocial3 { get; set; }

        public string PlannerSocial4 { get; set; }

        public string PlannerIntro { get; set; }

        public string Tel { get; set; }

        public string PlannerTel { get; set; }

        public string Permission { get; set; }

    }

}
