using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using Member = FindTrip_Web.Models.Member;
using FindTrip_Web.Filters;
using JWT;



namespace FindTrip_Web.Areas.Admin.Controllers
{
    //[EnableCors("*", headers: "*", methods: "*")] //DO NOT OPEN THIS
    //[CorsHandle]//DO NOT OPEN THIS
    //[AllowCrossSiteJson]// DO NOT OPEN THIS

    [RoutePrefix(prefix: "api/Login")]
    public class ApiMembersController : ApiController
    {

        private Model1 db = new Model1();

        // GET: api/ApiMembers

        [JwtAuthFilter]
        [Route("all")]
        public IHttpActionResult GetMembers(Member member)
        {

            var result = db.Members.Select(x => new
            {
                x.id,
                x.Email,
                x.name,

            }).ToList();
            string text = JsonConvert.SerializeObject(result);
            return Ok(new { success = true, result });
        }

        [JwtAuthFilter]
        [Route("load")]
        public HttpResponseMessage GetMembersInfo(Member member)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var check = db.Members.Find(Mid);

            if (check.Permission == "01")
            {


                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.name,
                    x.points,
                    x.manpic,
                    x.MemberIntro,
                    x.Tel,
                    x.PlannerSocial1,
                    x.PlannerSocial2,
                    x.Email
                });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, result });

            }

            if (check.Permission == "02")
            {
                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.name,
                    x.manpic,
                    x.MemberIntro,
                    x.Tel,
                    x.PlannerSocial1,
                    x.PlannerSocial2,
                    x.Email
                });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, result });

            }

            //string newResult = JsonConvert.SerializeObject(result);
            // HttpContext.Current.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //return Request.CreateResponse(HttpStatusCode.NoContent);
            return Request.CreateResponse(HttpStatusCode.NoContent, new { messaage = "沒東西啦" });
        }




        // GET: api/ApiMembers/5
        //[ResponseType(typeof(Member))]
        public IHttpActionResult GetMember(int? id)
        {
            return Ok();
        }

        [Route("update/test")]
        public HttpResponseMessage PatchMember(PatchMember patchMember)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));


            Member member = db.Members.Find(Mid);


            if (member.Permission == "01")
            {

                patchMember.Password = Utility.GenerateHashWithSalt(patchMember.Password, member.PasswordSalt);
                patchMember.Patch(member);

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.name,
                    x.Tel,
                    x.MemberIntro,
                    x.PlannerSocial1,
                    x.PlannerSocial2,

                });

                //string result = JsonConvert.DeserializeObject<PatchMember>(result1).ToString();

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "成功修改", result });

            }

            if (member.Permission == "02")
            {

                member.name = patchMember.name;
                patchMember.Password = Utility.GenerateHashWithSalt(patchMember.Password, member.PasswordSalt);
                patchMember.Patch(member);
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.name,
                    x.Tel,
                    x.MemberIntro,
                    x.PlannerSocial1,
                    x.PlannerSocial2,
                    x.PlannerName,
                    x.PlannerIntro,
                    x.PlannerSocial3,
                    x.PlannerSocial4,

                });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "規劃師成功修改", result });

            }

            return Request.CreateResponse(HttpStatusCode.NoContent, new { message = "沒東西R" });

        }


        [JwtAuthFilter]
        [Route("account")]
        public HttpResponseMessage PutMember(Member member)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var change = db.Members.Find(Mid);

            if (change.Permission == "01")
            {
                change.name = member.name;
                change.PasswordSalt = Utility.CreateSalt();
                change.Password = Utility.GenerateHashWithSalt(member.Password, change.PasswordSalt);
                change.Tel = member.Tel;
                change.PlannerSocial1 = member.PlannerSocial1;
                change.PlannerSocial2 = member.PlannerSocial2;
                change.MemberIntro = member.MemberIntro;
                //change.manpic = member.manpic;

                //db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.Email,
                    x.name,
                    x.Tel,
                    x.MemberIntro,
                    x.PlannerSocial1,
                    x.PlannerSocial2,
                    x.manpic

                });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行家修改成功", result });
            }

            if (change.Permission == "02")
            {
                change.name = member.name;
                change.PasswordSalt = Utility.CreateSalt();
                change.Password = Utility.GenerateHashWithSalt(member.Password, change.PasswordSalt);
                change.Tel = member.Tel;
                change.PlannerIntro = member.PlannerIntro;
                change.PlannerName = member.PlannerName;
                change.PlannerTel = member.PlannerTel;
                change.PlannerSocial1 = member.PlannerSocial1;
                change.PlannerSocial2 = member.PlannerSocial2;
                change.PlannerSocial3 = member.PlannerSocial3;
                change.PlannerSocial4 = member.PlannerSocial4;
                change.MemberIntro = member.MemberIntro;
                //change.manpic = member.manpic;

                //db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.Members.Where(x => x.id == Mid).Select(x => new
                {
                    x.id,
                    x.Email,
                    x.name,
                    x.Tel,
                    x.MemberIntro,
                    x.PlannerSocial1,
                    x.PlannerSocial2,
                    x.PlannerIntro,
                    x.PlannerName,
                    x.PlannerTel,
                    x.PlannerSocial3,
                    x.PlannerSocial4,
                    x.manpic
                });

                var result1 = new
                {
                    member.id,
                    member.Email,
                    member.name

                };
         

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = " 規劃師修改成功", result });

            }

            return Request.CreateResponse(HttpStatusCode.NoContent);

        }


        // POST: api/ApiMembers

        [JwtAuthFilter]
        [Route("userimg")]
        public HttpResponseMessage PostUserImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Member member = db.Members.Find(Mid);

            try
            {
                var postedFile = HttpContext.Current.Request.Files.Count > 0
                    ? HttpContext.Current.Request.Files[0]
                    : null;

                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    //string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                    //int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
                    string fileName = Utility.SaveUpImage(postedFile);
                    //IList<string> AllowedFileExtensions = new List<string> {".jpg", ".png", ".svg"};

                    //if (!AllowedFileExtensions.Contains(extension))
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    //    {
                    //        success = false,
                    //        message = "請上傳圖片正確格式，可接受格式為 .jpg, .png, .svg"
                    //    });
                    //}

                    //產生圖片連結
                    UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
                    {
                        Path = $"/Upload/Userimg/{fileName}"
                    };
                    //Userimage myfolder name where i want to save my image
                    Uri imageUrl = uriBuilder.Uri;
                    member.manpic = imageUrl.ToString();

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "已上傳個人圖片",
                        imageUrl
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


        [JwtAuthFilter]
        [Route("bgimg")]
        public HttpResponseMessage PostBackgroundImage()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Member member = db.Members.Find(Mid);

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
                        Path = $"/Upload/BGimg/{fileName}"
                    };
                    //Userimage myfolder name where i want to save my image
                    Uri imgUploadUrl = uriBuilder.Uri;
                    member.BGImg = imgUploadUrl.ToString();

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        message = "已上傳個人圖片",
                        imgUploadUrl
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

            return Request.CreateResponse();

        }


        [Route("MemberLogin")]
        public HttpResponseMessage MemberLogin(ViewLogin viewLogin)
        {
            Member member = ValidateUser(viewLogin.Email, viewLogin.Password);
            if (member != null)
            {
                JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
                string jwtToken = jwtAuthUtil.GenerateToken(member.id, member.Email);


                return Request.CreateResponse(HttpStatusCode.OK,
                    new { success = true, message = "登入成功", token = jwtToken, member.points, member.Permission, member.id, member.Email });
                //return Request.CreateResponse(HttpStatusCode.OK,
                //    new { success = true, message = "登入成功" });
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, new
            {
                success = false,
                message = "登入失敗"

            });
        }

        private Member ValidateUser(string Email, string Password)
        {
            Member member = db.Members.SingleOrDefault(x => x.Email == Email);
            if (member == null)
            {
                return null;
            }
            string saltPassword = Utility.GenerateHashWithSalt(Password, member.PasswordSalt);
            return saltPassword == member.Password ? member : null;
        }

        //[AllowCrossSiteJson]
        [Route("Register")]
        public IHttpActionResult PostMember(Member member)
        {
            var checkmember = db.Members.Where(x => x.Email == member.Email).FirstOrDefault();
            //bool duplicate = db.Members.Any(x => x.Email == member.Email);

            if (checkmember != null)
            {
                return Content(HttpStatusCode.BadRequest, new { success = false, message = "此帳號已經註冊，請重新輸入" });
            }

            UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url)
            {
                Path = "/Upload/Userimg/20200601054121.jpg"
            };
            Uri defaultImgUri = uriBuilder.Uri;
            member.manpic = defaultImgUri.ToString();
            string img = "http://findtrip.rocket-coding.com:80/Upload/Userimg/20200601054121.jpg";
            UriBuilder ub = new UriBuilder();
            member.PasswordSalt = Utility.CreateSalt();
            member.Password = Utility.GenerateHashWithSalt(member.Password, member.PasswordSalt);
            member.points = Convert.ToInt32("1000"); //add 1k points
            string pathSocial1 = "https://www.facebook.com/";
            string remind = "輸入您的臉書帳號";
            member.PlannerSocial1 = pathSocial1 + remind;
            string pathSocial2 = "https://twitter.com/";
            string remind2 = "輸入您的推特帳號";
            member.PlannerSocial2 = pathSocial2 + remind2;
            member.Permission = "01";
            member.CreateOn = DateTime.Now;

            db.Members.Add(member);
            //SendAuthCodeToMember(member);
            db.SaveChanges();

            return Ok(new { success = true, message = "註冊成功", member.id, member.Email, member.points, member.manpic });
        }

        //private void SendAuthCodeToMember(Member member1)
        //{
        //    string mailBody = System.IO.File.ReadAllText(path:C:\Users\yuhchinhung\source\repos\FindTrip_Web\FindTrip_Web\App_Data\MemberEmailVerification.html);
        //}
        //return CreatedAtRoute("DefaultApi", new { id = member.id }, member),;


        [JwtAuthFilter]
        [Route("topup")]
        public HttpResponseMessage RechargePoints(PointsHistory pointsHistory)
        {

            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            Member member = db.Members.Find(Mid);

            if (pointsHistory.Product != null)
            {
                pointsHistory.MemberId = Mid;
                var total = member.points + Convert.ToInt32(pointsHistory.Product);
                member.points = total;
                pointsHistory.CreateOn = DateTime.Now;


                db.Entry(member).State = EntityState.Modified;
                db.PointsHistories.Add(pointsHistory);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,
                    new { success = true, member.points, message = "儲值成功", pointsHistory.Product });
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "儲值失敗" });
        }

        // DELETE: api/ApiMembers/5
        [JwtAuthFilter]
        //[ResponseType(typeof(Member))]
        public IHttpActionResult DeleteMember(int id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            db.SaveChanges();

            return Ok(member);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.id == id) > 0;
        }
    }
}