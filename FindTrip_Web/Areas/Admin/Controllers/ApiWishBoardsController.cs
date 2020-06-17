using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
            var result = db.WishBoards.OrderBy(w => w.CreateOn).Select(w => new
            {
                w.id,
                w.MyMember.name,
                w.MyMember.manpic,
                w.Comment1,
                w.Comment2,
                CommentTotal = db.WishBoardReplies.Count(y => y.Rid == w.id),
                w.LikeTotal,

            });

            return Request.CreateResponse(HttpStatusCode.OK,
                new { success = true, message = "can view all now", result });

        }//許願版內頁
        [Route("inner/{Wid}")]
        public HttpResponseMessage GetSWishBoard(int Wid)
        {
            var wish = db.WishBoards.Find(Wid);


            var result = new
            {
                wish.id,
                wish.MyMember.name,
                wish.MyMember.manpic,
                wish.Comment1,
                wish.Comment2,
                CommentTotal = db.WishBoardReplies.Count(y => y.Rid == Wid),
                wish.CreateOn,
                
                wishReply = db.WishBoardReplies.Where(y=>y.Rid==Wid).Select(y=>new
                {
                    y.id,
                    y.MyMember.name,
                    y.MyMember.manpic,
                    y.NewComment,
                    y.CreateOn
                })
            };

            //var wishReply = db.WishBoardReplies.Where(y => y.Rid == Wid).FirstOrDefault();

            //var result2 = new
            //{
            //    wishReply.MyMember.name,
            //    wishReply.MyMember.manpic,
            //    wishReply.NewComment,

            //};



            //var result = db.WishBoardReplies.Where(x => x.Rid == Wid).Select(x => new
            //{
            //    WishBoardId = x.MyWishBoard.id,
            //    MainName= x.MyWishBoard.MyMember.name,
            //    MainPic= x.MyWishBoard.MyMember.manpic,
            //    x.MyWishBoard.Comment1,
            //    x.MyWishBoard.Comment2,
            //    MainCreateOn = x.MyWishBoard.CreateOn,
            //    CommentTotal = db.WishBoardReplies.Count(y => y.Rid == Wid),
            //    x.Like,

            //    x.id,
            //    ReplyName= x.MyMember.name,
            //    ReplyPic = x.MyMember.manpic,
            //    x.NewComment,
            //    ReplyCreateOn = x.CreateOn

            //});

            return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message = "單筆許願版內頁", result});

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
                MemberId = Mid,
                x.MyMember.manpic,
                x.MyMember.name,
                x.Comment1,
                x.Comment2,
                x.CreateOn,
                //LikeTotal = db.WishBoardReplies.Where(y=>y.MemberId == Mid)

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
                x.CreateOn,

            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "留言成功", result });
        }


     
        [Route("wimg")]
        public HttpResponseMessage PostBackgroundImage()
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            //Member member = db.Members.Find(Mid);

            try
            {
                var postedFile = HttpContext.Current.Request.Files.Count > 0
                    ? HttpContext.Current.Request.Files[0]
                    : null;
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    //string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                    //int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
                    string fileName = Utility.UploadImage(postedFile);
                    //IList<string> AllowedFileExtensions = new List<string> {".jpg", ".png", ".svg"};

                    //if (!AllowedFileExtensions.Contains(extension))
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    //    {
                    //        success = false,
                    //        message = "請上傳圖片正確格式，可接受格式為 .jpg, .png, .svg"
                    //    });
                    //}

                    UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
                    {
                        Path = $"/Upload/Wimg/{fileName}"
                    };
                    //Userimage myfolder name where i want to save my image
                    Uri imgUploadUrl = uriBuilder.Uri;
                    string Img = imgUploadUrl.ToString();

                    //db.Entry(wishboard).State = EntityState.Modified;
                    //db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "已上傳圖片",
                        Img
                    });

                }

                return Request.CreateResponse(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = "無圖片，請選擇圖片上傳"
                });

            }
            catch
            {
                throw;
            }


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