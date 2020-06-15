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
using WishBoard = FindTrip_Web.Models.WishBoard;
using WishBoardReply = FindTrip_Web.Models.WishBoardReply;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/wish")]
    public class ApiWishBoardsController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/WishBoards
        public IQueryable<WishBoard> GetWishBoards()
        {
            return db.WishBoards;
        }

        // GET: api/WishBoards/5
        //[ResponseType(typeof(WishBoard))]
        [Route("all")]
        public HttpResponseMessage GetWishBoard()
        {
            var result = db.WishBoards.OrderBy(x => x.CreateOn).Select(x => new
            {
                x.id,
                x.MyMember.name,
                x.MyMember.manpic,
                x.Comment1,
                x.Comment2,
            }).Take(5);

            return Request.CreateResponse(HttpStatusCode.OK,
                new { success = true, message = "can view all now", result });

        }

        // PUT: api/WishBoards/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWishBoard(int id, WishBoard wishBoard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wishBoard.id)
            {
                return BadRequest();
            }

            db.Entry(wishBoard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishBoardExists(id))
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

        // POST: api/WishBoards
        [JwtAuthFilter]
        [Route("make")]
        [ResponseType(typeof(WishBoard))]
        public HttpResponseMessage PostWishBoard(WishBoard wishBoard)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            wishBoard.MemberId = Mid;
            wishBoard.CreateOn = DateTime.Now;

            db.WishBoards.Add(wishBoard);
            db.SaveChanges();

            var result = db.WishBoards.Where(x => x.MemberId == Mid).Select(x => new
            {
                x.id,
                MemberId = x.MyMember.id,
                x.MyMember.manpic,
                x.MyMember.name,
                x.Comment1,
                x.Comment2,
                x.CreateOn

            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "留言成功", result });
        }


        [JwtAuthFilter]
        [Route("reply")]
        public HttpResponseMessage PostWishBoardReply(WishBoardReply wishBoardReply)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            wishBoardReply.MemberId = Mid;
            wishBoardReply.CreateOn = DateTime.Now;

            db.WishBoardReplies.Add(wishBoardReply);
            db.SaveChanges();

            var result = db.WishBoardReplies.Where(x => x.MemberId == Mid).Select(x => new
            {
                x.id,
                x.Rid,
                MemberId = x.MyMember.id,
                x.MyMember.manpic,
                x.MyMember.name,
                x.NewComment,
                x.CreateOn

            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "留言成功", result });
        }



        // DELETE: api/WishBoards/5
        [ResponseType(typeof(WishBoard))]
        public IHttpActionResult DeleteWishBoard(int id)
        {
            WishBoard wishBoard = db.WishBoards.Find(id);
            if (wishBoard == null)
            {
                return NotFound();
            }

            db.WishBoards.Remove(wishBoard);
            db.SaveChanges();

            return Ok(wishBoard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WishBoardExists(int id)
        {
            return db.WishBoards.Count(e => e.id == id) > 0;
        }
    }
}