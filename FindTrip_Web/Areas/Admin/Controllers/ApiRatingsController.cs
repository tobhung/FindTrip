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
using System.Web.Security;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Rating = FindTrip_Web.Models.Rating;
using FindTrip_Web.Filters;
using JWT;


namespace FindTrip_Web.Areas.Admin.Controllers
{

    [RoutePrefix("api/rating")]
    public class ApiRatingsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ApiRatings
        public IQueryable<Rating> GetRatings()
        {
            return db.Ratings;
        }

        // GET: api/ApiRatings/5
        [ResponseType(typeof(Rating))]
        public IHttpActionResult GetRating(int id)
        {
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        // PUT: api/ApiRatings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRating(int id, Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rating.id)
            {
                return BadRequest();
            }

            db.Entry(rating).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingExists(id))
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
        [Route("test/single")]
        public HttpResponseMessage PostRatings(Rating rating)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));


            rating.MemberId = Mid;
            rating.CreateOn = DateTime.Now;
            //rating.Ratingtotal = db.Ratings.Where(x => x.id == OrderComment.TravelPlan_id).Sum(x => x.rating);
            //rating.StarAmount = db.Ratings.Where(x => x.id == OrderComment.TravelPlan_id).Average(x => x.star);

            db.Ratings.Add(rating);
            db.SaveChanges();

            var result = db.Ratings.Where(x => x.id == rating.id).Select(x =>  new
            {
                x.id,
                x.TravelId,
                x.rating,
                x.RatingContent
            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "成功評論", result });
        }


        [JwtAuthFilter]
        [Route("single/{OrderId}")]
        public HttpResponseMessage PostRating(int OrderId, Rating rating)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var orderCheck = db.Orders.Find(OrderId);

            if (OrderId != orderCheck.id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { success = false, message = "not buyer cannot leave comment" });
            }

            if (orderCheck.Status != 3)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { success = false, message = "訂單尚未完成，無法評價" });
            }

            rating.TravelId = orderCheck.TravelPlan_id;
            rating.OrderId = orderCheck.id;
            rating.MemberId = Mid;
            rating.CreateOn = DateTime.Now;
            //rating.Ratingtotal = db.Ratings.Where(x => x.id == OrderComment.TravelPlan_id).Sum(x => x.rating);
            //rating.StarAmount = db.Ratings.Where(x => x.id == OrderComment.TravelPlan_id).Average(x => x.star);

            db.Ratings.Add(rating);
            db.SaveChanges();

            var result = db.Ratings.Where(x => x.id == rating.id).Select(x => new
            {
                x.id,
                x.TravelId,
                x.rating,
                x.star,
                x.RatingContent
            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "成功評論", result });
        }


        // DELETE: api/ApiRatings/5
        [ResponseType(typeof(Rating))]
        public IHttpActionResult DeleteRating(int id)
        {
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return NotFound();
            }

            db.Ratings.Remove(rating);
            db.SaveChanges();

            return Ok(rating);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RatingExists(int id)
        {
            return db.Ratings.Count(e => e.id == id) > 0;
        }
    }
}