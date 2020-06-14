using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FindTrip_Web.Models;
using FindTrip_Web.Security;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/points")]

    public class PointsHistoriesController : ApiController
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
            PointsHistory pointsHistory = db.PointsHistories.Find(Mid);

            pointsHistory.MemberId = Mid;
            if (pointsHistory.MemberId != Mid)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var result = db.PointsHistories.Where(x => x.id == Mid).Select(x => new
            {
                x.Product,
                x.CreateOn,
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