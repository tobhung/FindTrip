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
using TravelPlan = FindTrip_Web.Models.TravelPlan;
using FindTrip_Web.Filters;
using JWT;
using Member = FindTrip_Web.Models.Member;
using Order = FindTrip_Web.Models.Order;
using PointsHistory = FindTrip_Web.Models.PointsHistory ;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/points")]

    public class ApiPointsHistoriesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/PointsHistories
        public IQueryable<PointsHistory> GetPointsHistories()
        {
            return db.PointsHistories;
        }

        //// GET: api/PointsHistories/5
        //[ResponseType(typeof(PointsHistory))]
        [JwtAuthFilter]
        [Route("history")]
        public HttpResponseMessage GetPointsHistory()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
           

            var result = db.PointsHistories.Where(x => x.MemberId == Mid).Select(x => new
            {
                
                x.Product,
                x.CreateOn
           
            });

            return Request.CreateResponse(HttpStatusCode.OK, new {success = true, result});
        }

        // PUT: api/PointsHistories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPointsHistory(int id, PointsHistory pointsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pointsHistory.id)
            {
                return BadRequest();
            }

            db.Entry(pointsHistory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PointsHistoryExists(id))
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

        // POST: api/PointsHistories
        [ResponseType(typeof(PointsHistory))]
        public IHttpActionResult PostPointsHistory(PointsHistory pointsHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PointsHistories.Add(pointsHistory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pointsHistory.id }, pointsHistory);
        }

        // DELETE: api/PointsHistories/5
        [ResponseType(typeof(PointsHistory))]
        public IHttpActionResult DeletePointsHistory(int id)
        {
            PointsHistory pointsHistory = db.PointsHistories.Find(id);
            if (pointsHistory == null)
            {
                return NotFound();
            }

            db.PointsHistories.Remove(pointsHistory);
            db.SaveChanges();

            return Ok(pointsHistory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PointsHistoryExists(int id)
        {
            return db.PointsHistories.Count(e => e.id == id) > 0;
        }
    }
}