using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using FindTrip_Web.Models;
using FindTrip_Web.Security;
using Microsoft.Ajax.Utilities;
using System.Web.Security;
using Newtonsoft.Json;
using TravelPlan = FindTrip_Web.Models.TravelPlan;
using FindTrip_Web.Filters;
using JWT;
using Member = FindTrip_Web.Models.Member;
using Order = FindTrip_Web.Models.Order;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/order")]
    public class ApiOrdersController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ApiOrders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/ApiOrders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [JwtAuthFilter]
        [Route("load")]
        public IHttpActionResult GetMemberOrder()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var result = db.Orders.Where(x => x.MemberId == Mid).Select(x => new
            {
                x.id,
                x.MemberId,
                x.MyMember.name,
                x.MyMember.Tel,
                x.DepartureTime1,
                x.DepartureTime2,
                x.Budget,
                x.Adult,
                x.Children,
                x.country,
                x.city,
                x.TravelPlan_id,
                x.Status,
                x.Remark,
                x.Act,
                x.Culture,
                x.Food,
                x.Secret,
                x.Shopping,
                x.Religion,
                PlanPic = x.MyMember.TravelPlans.Where(z=>z.id==x.TravelPlan_id).Select(z=>new
                {
                    z.Cpicture
                }),

                PlannerName = db.TravelPlans.Where(y => y.id == x.TravelPlan_id).Select(y => new
                {
                    y.MyMember.PlannerName
                })

                //Plan = db.TravelPlans.Where(y=>y.id== x.TravelPlan_id).Select(y=> new
                //    {
                //        y.Cpicture
                //    })

            });

            return Ok(new { success = true, result });
        }


        //旅行家檢視單筆訂單 帶token 帶訂單ID
        [JwtAuthFilter]
        [Route("loadsingle/{id}")]
        public IHttpActionResult GetMemberSingleOrder(int id)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));


            var result = db.Orders.Where(x => x.id == id).Select(x => new
            {
                x.id,
                x.MemberId,
                x.MyMember.Tel,
                x.MyMember.name,
                x.MyMember.manpic,
                x.MyMember.Email,
                x.DepartureTime1,
                x.DepartureTime2,
                x.Budget,
                x.Adult,
                x.Children,
                x.country,
                x.city,
                x.TravelPlan_id,
                x.PlannerId,
                x.CreateOn,
               x.Status,
               x.Remark,
               x.Act,
               x.Culture,
               x.Food,
               x.Secret,
               x.Shopping,
               x.Religion,

               PlannerName = db.TravelPlans.Where(y=>y.id == x.TravelPlan_id).Select(y=>new
               {
                   y.MyMember.PlannerName
               })

               //PlanPic = x.MyMember.TravelPlans.Where(z => z.id == x.TravelPlan_id).Select(z => new
               //{
               //    z.Cpicture
               //}),

               // Plan = db.TravelPlans.Where(y=>y.id == x.TravelPlan_id).Select(y=>new{
               //    y.Cpicture
               //})

            }).ToList();

            return Ok(new { success = true, result });
        }


        [JwtAuthFilter]
        [Route("seller")]
        public IHttpActionResult GetSellerOrder()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            

            var result = db.Orders.Where(x => x.PlannerId == Mid).Select(x =>
                new
                {
                    x.id,
                    x.MyMember.Email,
                    x.MyMember.Tel,
                    x.MyMember.name,
                    x.Adult,
                    x.Children,
                    x.DepartureTime1,
                    x.DepartureTime2,
                    x.country,
                    x.city,
                    x.CreateOn,
                    x.Status,
                    x.Remark,
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Shopping,
                    x.Secret,
                    x.Religion,

                    PlanPic = db.TravelPlans.Where(y => y.id == x.TravelPlan_id).Select(y => new
                    {
                        y.Cpicture

                    })

                });

            return Ok(new { success = true, result });
        }

        [JwtAuthFilter]
        [Route("sellersingle/{id}")]
        public IHttpActionResult GetSellerSingleOrder(int id)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var seller = db.Members.Find(Mid);
            if (seller.Permission != "02")
            {
                return BadRequest();
            }

            var result = db.Orders.Where(x => x.PlannerId == Mid && x.id ==id).Select(x => new
            {
                x.id,
                x.MemberId,
                x.MyMember.name, //get this as planner name?? 
                x.MyMember.Tel,
                x.MyMember.manpic,
                x.DepartureTime1,
                x.DepartureTime2,
                x.Budget,
                x.Adult,
                x.Children,
                x.country,
                x.city,
                x.TravelPlan_id,
                x.CreateOn,
                x.Status,

            }).ToList();

            return Ok(new { success = true, result });
        }

        // PUT: api/ApiOrders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        [JwtAuthFilter]
        [Route("update/{id}")] //planner send confirmation back
        public HttpResponseMessage PatchOrder(int id, Order order)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var seller = db.Members.Find(Mid);

            if (seller.Permission != "02")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var orderConfirm = db.Orders.Find(id);

            if (orderConfirm.Status == 1)
            {
                orderConfirm.Status = order.Status;

                db.SaveChanges();

                var result = db.Orders.Where(x => x.id == id).Select(x => new
                {
                    x.id,
                    x.MemberId,
                    x.MyMember.name,
                    x.MyMember.Tel,
                    x.MyMember.Email,
                    x.DepartureTime1,
                    x.DepartureTime2,
                    x.Budget,
                    x.Adult,
                    x.Children,
                    x.country,
                    x.city,
                    x.TravelPlan_id,
                    x.PlannerId,
                    x.Status,
                    x.Remark,
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Shopping,
                    x.Secret,
                    x.Religion
                });

                return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message="委任中", result});
            }

            if (orderConfirm.Status == 2) 
            {

                orderConfirm.Status = order.Status;
                //var member = db.Members.Find(orderConfirm.PlannerId);
                //var planPoints = db.TravelPlans.Find(orderConfirm.PlannerId);
                //var newPoints = member.points + planPoints.points;

                //member.points = newPoints;
                //db.Entry(member).State = EntityState.Modified;

                db.SaveChanges();


                var result = db.Orders.Where(x => x.id == id).Select(x => new
                {
                    x.id,
                    x.MemberId,
                    x.MyMember.name,
                    x.MyMember.Tel,
                    x.DepartureTime1,
                    x.DepartureTime2,
                    x.Budget,
                    x.Adult,
                    x.Children,
                    x.country,
                    x.city,
                    x.TravelPlan_id,
                    x.Status,
                    x.Remark,
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Shopping,
                    x.Secret,
                    x.Religion
                });

                return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message="已完成", result});
            }

            if (orderConfirm.Status == 3)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new {success = false, message = "訂單已完成，無須任何動作"});
            }
            return Request.CreateResponse(HttpStatusCode.NoContent, new {message ="????????"});
        }

        //create travel plan
        [JwtAuthFilter]
        [Route("create")]
        public HttpResponseMessage PostOrder(Order order)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            

            if (order.TravelPlan_id == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new {success = false, message = "旅行計畫不得為空值"});
            }

            var member = db.Members.Find(Mid);
            TravelPlan travel = new TravelPlan();
            order.MemberId = Mid;
            order.CreateOn = DateTime.Now;
            order.Status = 1;


            var result1 = String.Join("", order.DepartureTime1); // why doesn't it split?, and this doesn't work

            var planner = db.TravelPlans.Find(order.TravelPlan_id);
            order.PlannerId = planner.MemberId;

            if (Mid == order.PlannerId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { success = false, message = "自己的計劃不能自己買RRRRRRRRR" });
            }


            var currentPoints = member.points - planner.points;

            order.PointsLeft = currentPoints;
            member.points = currentPoints;
            
            


            db.Orders.Add(order);
            db.Entry(member).State = EntityState.Modified;

            db.SaveChanges();

            var result = db.Orders.Where(x => x.id == order.id).Select(x => new
            {
                x.id,
                x.MemberId,
                x.MyMember.name,
                x.MyMember.Tel,
                x.MyMember.Email,
                x.DepartureTime1,
                x.DepartureTime2,
                x.Budget,
                x.Adult,
                x.Children,
                x.country,
                x.city,
                x.Remark,
                x.TravelPlan_id,
                x.PlannerId,
                x.Status,
                x.PointsLeft,
                x.Act,
                x.Culture,
                x.Food,
                x.Shopping,
                x.Secret,
                x.Religion

            });

            return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message = "旅行計畫建立成功", result});
           
        }
        


        // DELETE: api/ApiOrders/5
        //[ResponseType(typeof(Order))]
        [JwtAuthFilter]
        [Route("delete/{id}")]
        public HttpResponseMessage DeleteOrder(int id)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var seller = db.Members.Find(Mid);

            if (seller.Permission != "02")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new {success = false, message = "使用者權限非規劃師，無刪除權限"});
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new {success = false, message = "查無此訂單"});
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new{ success= true, message="成功刪除訂單"});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.id == id) > 0;
        }
    }
}