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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using FindTrip_Web.Models;
using FindTrip_Web.Security;
using Microsoft.Ajax.Utilities;
using System.Web.Http.Results;
using System.Web.Security;
using Newtonsoft.Json;
using Message = FindTrip_Web.Models.Message;
using FindTrip_Web.Filters;
using JWT;

namespace FindTrip_Web.Areas.Admin.Controllers
{

    [RoutePrefix("api/msg")]
    public class ApiMessagesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ApiMessages
        public IQueryable<Message> GetMessages()
        {
            return db.Messages;
        }

        //// GET: api/ApiMessages/5
        //[ResponseType(typeof(Message))]
        [JwtAuthFilter]
     
        public HttpResponseMessage GetMessage (Message message)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var member = db.Members.Find(Mid);

            //if (message.TravelPlanId == 0)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest,
            //        new {success = false, message = "無此計畫訊息內容"});
            //}

            var plan = db.Messages.Find(Mid);



            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/ApiMessages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.id)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        //// POST: api/ApiMessages
        //[ResponseType(typeof(Message))]
        //this is when buyer click on product to send msg to seller
        [JwtAuthFilter]
        [Route("send")]
        public HttpResponseMessage PostMessage(Message message)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            if (message.TravelPlanId == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new {success = false, message = "no such plan exists"});
            }
            TravelPlan travelPlan = new TravelPlan();
            message.CreateOn = DateTime.Now;
            var planner = db.TravelPlans.Find(message.TravelPlanId);
            message.PlannerId = planner.MemberId;
            message.MemberId = Mid; 


            db.Messages.Add(message);
            db.SaveChanges();

            var result = db.Messages.Where(x => x.TravelPlanId == planner.id).Select(x => new
            {
                x.id,
                x.TravelPlanId,
                x.Body,
                x.CreateOn,
                buyer = message.MemberId,
                seller = message.PlannerId
                
            });

            return Request.CreateResponse(HttpStatusCode.OK, new {success= true, message="訊息傳送成功", result});
        }

        [JwtAuthFilter]
        [Route("receive")]
        public HttpResponseMessage GetBuyerMessage(Message message)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var sellerCheck = db.Messages.Find(Mid);
            message.PlannerId = Mid;

            var result = db.Messages.Where(x => x.PlannerId == Mid).Select(x => new
            {
                x.id,
                x.TravelPlanId,
                x.Body,
                x.CreateOn,
                buyer = x.MemberId
            });

            return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message = "賣家獲得所有訊息", result});

        }


        // DELETE: api/ApiMessages/5
        [ResponseType(typeof(Message))]
        public IHttpActionResult DeleteMessage(int id)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            db.Messages.Remove(message);
            db.SaveChanges();

            return Ok(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.id == id) > 0;
        }
    }
}