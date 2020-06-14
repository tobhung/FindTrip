using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace FindTrip_Web.Models
{
    public class EnumList
    {
        public enum Authority
        {
            AllMember = 01,
            Traveller = 02,
            Planner=03

            
        }

        public enum OrderStatus
        {
            未確認 = 1,
            委任中 = 2,
            已完成 = 3
        }

        public enum TravelType
        {
            冒險 = 1,
            秘境 =2,
            文化 =3,
            吃貨 =4, 
            購物 = 5, 
            宗教 = 6
        }




    }
}