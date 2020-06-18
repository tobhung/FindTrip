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


namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/plan")]
    public class ApiTravelPlansController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ApiTravelPlans

            
        //search function for index page
        [Route("search")]
        public HttpResponseMessage GetSearchPlans(string search)
        {

            var result = db.TravelPlans.Where(x => x.country.Contains(search) || x.city.Contains(search)).Select(
                    x => new
                    {
                        x.id,
                        x.MemberId,
                        x.points,
                        x.TPBGImg,
                        x.MyMember.manpic,
                        x.MyMember.name,
                        x.country,
                        x.city,
                        x.CreateOn,

                        tags = new
                        {
                            x.Act,
                            x.Culture,
                            x.Food,
                            x.Secret,
                            x.Shopping,
                            x.Religion
                        },

                        rating = db.Ratings.Count(y => y.TravelId == x.id),
                        star = db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average() == null
                            ? 0
                            : db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average(),

                    });

                return Request.CreateResponse(HttpStatusCode.OK, new {success = true, message = "搜尋成功", result});


        }
        public IQueryable<TravelPlan> GetTravelPlans()
        {
            return db.TravelPlans;
        }

        // GET: api/ApiTravelPlans/5
        [ResponseType(typeof(TravelPlan))]
        public IHttpActionResult GetTravelPlan(int id)
        {
            TravelPlan travelPlan = db.TravelPlans.Find(id);
            if (travelPlan == null)
            {
                return NotFound();
            }

            return Ok(travelPlan);
        }

        //首頁所有旅行計畫  不帶token
        [Route("index")]
        public HttpResponseMessage GetMemberPlans(TravelPlan travelPlan)
        {
            Countries country = new Countries();

            var countries = db.TravelPlans.Select(c => c.country);
            var allPlans = db.TravelPlans.OrderByDescending(x=>x.CreateOn).Select(x => new
            {
                x.id,
                x.MemberId,
                x.points,
                x.TPBGImg,
                x.MyMember.manpic,
                name = x.MyMember.PlannerName,
                x.country,
                x.city,
                x.CreateOn,

                tags = new
                {
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Secret,
                    x.Shopping,
                    x.Religion
                },

                rating = db.Ratings.Count(y => y.TravelId == x.id),
                star = db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average() == null
                    ? 0
                    : db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average(),

            });


            var wishboard = db.WishBoards.OrderBy(w => w.CreateOn).Select(w => new
            {
                w.id,
                w.MyMember.name,
                w.MyMember.manpic,
                w.Comment1,
                w.Comment2,
                w.LikeTotal,

            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, allPlans, countries, wishboard });

        }


        [JwtAuthFilter]
        [Route("member/index")]
        public HttpResponseMessage GetAuthMemberPlans()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            Member member = db.Members.Find(Mid);

            var countries = db.Countries.Select(c => new
            {
                c.id,
                c.country,
                city = c.Districts.Where(d => d.Cid == c.id).Select(d => d.city)
            });

            var allPlans = db.TravelPlans.Select(x => new
            {
                x.id,
                MemberId = x.MyMember.id,
                x.points,
                x.MyMember.name,
                x.MyMember.manpic,
                x.Cpicture,
                x.country,
                x.city,
                x.CreateOn,

                tags = new
                {
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Religion,
                    x.Secret,
                    x.Shopping
                },

                rating = db.Ratings.Where(y => y.TravelId == x.id).Select(y => new
                {
                    y.star,
                    y.rating
                })

            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, allPlans, countries });
        }



        //規劃師檢視自己所有訂單
        [JwtAuthFilter]
        [Route("loadsingle")]
        public HttpResponseMessage GetMemberPlans()
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            Member member = db.Members.Find(Mid);

            var result = db.TravelPlans.Where(x=>x.MemberId == Mid).OrderByDescending(x=>x.CreateOn).Select(x=>new
            {
                id = x.id,
                MemberId = x.MyMember.id,
                x.points,
                x.Cpicture,
                x.TravelPlanIntro,
                x.TPExperience,
                x.CreateOn,
                x.country,
                x.city,

                tags = new
                {
                    x.Religion,
                    x.Secret,
                    x.Act,
                    x.Food,
                    x.Culture,
                    x.Shopping
                }

            });
            //string newResult = JsonConvert.SerializeObject(result);
            // HttpContext.Current.Response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, result });
        }


        [Route("inner/{id}")]
        public HttpResponseMessage GetPlanInner(int id)
        {
            TravelPlan travelPlan = db.TravelPlans.Find(id);

            var result = new
            {
                travelPlan.id,
                travelPlan.TravelPlanIntro,
                travelPlan.TPExperience,
                travelPlan.TPBGImg,
                travelPlan.Cpicture,
                travelPlan.points,
                travelPlan.country,
                travelPlan.city,
                travelPlan.Act,
                travelPlan.Culture,
                travelPlan.Food,
                travelPlan.Shopping,
                travelPlan.Secret,
                travelPlan.Religion,
                travelPlan.MyMember.PlannerIntro,
                travelPlan.MyMember.PlannerName,
                travelPlan.MyMember.PlannerSocial3,
                travelPlan.MyMember.PlannerSocial4,
                travelPlan.MyMember.manpic,
                PlannerId = travelPlan.MyMember.id,

                rating = db.Ratings.Where(y => y.TravelId == travelPlan.id).Select(y => new
                {
                    y.Ratingtotal,
                    y.star,
                    buyerName = db.Orders.Where(z => z.TravelPlan_id == travelPlan.id).Select(z => z.MyMember.name)
                        .FirstOrDefault(),
                    buyerPic = db.Orders.Where(a => a.TravelPlan_id == travelPlan.id).Select(a => a.MyMember.manpic)
                        .FirstOrDefault(),
                    y.RatingContent,
                    y.CreateOn


                }),

                ratings = db.Ratings.Count(y => y.TravelId == travelPlan.id),
                stars = db.Ratings.Where(z => z.TravelId == travelPlan.id).Select(z => z.star).Sum() == null
                    ? 0
                    : db.Ratings.Where(z => z.TravelId == travelPlan.id).Select(z => z.star).Average(),


            };

                //var result = db.TravelPlans.Where(x => x.id == id).Select(x => new
                //{
                //    x.id,
                //    x.TravelPlanIntro,
                //    x.TPExperience,
                //    x.TPBGImg,
                //    x.Cpicture,
                //    x.points,
                //    x.country,
                //    x.city,
                //    x.Act,
                //    x.Culture,
                //    x.Food,
                //    x.Shopping,
                //    x.Secret,
                //    x.Religion, 
                //    x.MyMember.PlannerIntro,
                //    x.MyMember.PlannerName,
                //    x.MyMember.PlannerSocial3,
                //    x.MyMember.PlannerSocial4,
                //    x.MyMember.manpic,

      //    //rating = db.Ratings.Count(y => y.TravelId == x.id),
      //    //star = db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average(),
      //    //username = db.Orders.Where(a=>a.TravelPlan_id == x.id).Select(a=>a.MyMember.name),
      //    //userpic = db.Orders.Where(b=>b.TravelPlan_id == x.id).Select(b=>b.MyMember.manpic),

      //    rating = db.Ratings.Where(y => y.TravelId == x.id).Select(y => new
      //    {
      //        y.Ratingtotal,
      //        y.star,
      //        buyerName = db.Orders.Where(z => z.TravelPlan_id == x.id).Select(z => z.MyMember.name).FirstOrDefault(),
      //        buyerPic = db.Orders.Where(a => a.TravelPlan_id == x.id).Select(a => a.MyMember.manpic).FirstOrDefault(),
      //        y.RatingContent,
      //        y.CreateOn


      //    }),

      //    ratings = db.Ratings.Count(y => y.TravelId == x.id),
      //    stars = db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Sum() == null
      //        ? 0
      //        : db.Ratings.Where(z => z.TravelId == x.id).Select(z => z.star).Average(),

      //});

            return Request.CreateResponse(HttpStatusCode.OK, new {success = true, result});
            }


        [JwtAuthFilter]
        [Route("update/{id}")]
        public HttpResponseMessage PatchTravelPlan(int id, TravelPlan travelPlan)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var planChanges = db.TravelPlans.Find(id);
     
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


            planChanges.TravelPlanIntro = travelPlan.TravelPlanIntro;
            planChanges.TPExperience = travelPlan.TPExperience;
            planChanges.points = travelPlan.points;
            planChanges.Act = travelPlan.Act;
            planChanges.Culture = travelPlan.Culture;
            planChanges.Food = travelPlan.Food;
            planChanges.Secret = travelPlan.Secret;
            planChanges.Shopping = travelPlan.Shopping;
            planChanges.country = travelPlan.country;
            planChanges.city = travelPlan.city;


            //db.TravelPlans.Attach(travelPlan);
            //db.Entry(travelPlan).State = EntityState.Modified;
            db.SaveChanges();

            var result = db.TravelPlans.Where(x => x.id == id).Select(x => new
            {
                x.id,
                x.MemberId,
                x.points,
                x.MyMember.name,
                x.TravelPlanIntro,
                x.TPExperience,
                x.country,
                x.city,
                x.Act,
                x.Culture,
                x.Food,
                x.Secret,
                x.Shopping,
                x.Religion

            });



            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行計畫修改成功" });
        }

        [JwtAuthFilter]
        [Route("update/test/{id}")]
        public HttpResponseMessage PatchTravelPlans(int id, TravelPlan travelPlan)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var planChanges = db.TravelPlans.Find(id);

            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (planChanges.MemberId != Mid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { success = false, message = "THE PLAN DOESNT BELONG TO PLANNER LA" });
            }

            planChanges.TravelPlanIntro = travelPlan.TravelPlanIntro;
            planChanges.TPExperience = travelPlan.TPExperience;
            planChanges.points = travelPlan.points;
            planChanges.Act = travelPlan.Act;
            planChanges.Culture = travelPlan.Culture;
            planChanges.Food = travelPlan.Food;
            planChanges.Secret = travelPlan.Secret;
            planChanges.Shopping = travelPlan.Shopping;
            planChanges.country = travelPlan.country;
            planChanges.city = travelPlan.city;



            //db.TravelPlans.Attach(travelPlan);
            //db.Entry(travelPlan).State = EntityState.Modified;
            db.SaveChanges();

            var result = db.TravelPlans.Where(x => x.id == id).Select(x => new
            {
                x.id,
                x.MemberId,
                x.points,
                x.MyMember.name,
                x.TravelPlanIntro,
                x.TPExperience,
                x.country,
                x.city,
                x.Act,
                x.Culture,
                x.Food,
                x.Secret,
                x.Shopping,
                x.Religion

            });


            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行計畫修改成功" });
        }


        [JwtAuthFilter]
        [Route("create")]
        public HttpResponseMessage PostTravelPlan(TravelPlan travelPlan)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            //var seller = db.Members.Find(Mid);

            //if (seller.Permission != "02")
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "非規劃師，無權限" });
            //}

            
            travelPlan.CreateOn = DateTime.Now; //only year/mth/day
            travelPlan.MemberId = Mid;

            db.TravelPlans.Add(travelPlan);
            db.SaveChanges();

            var result = db.TravelPlans.Where(x => x.id == travelPlan.id).Select(x => new
            {
                 x.id,
                x.MemberId,
                x.points,
                x.Cpicture,
                x.TPBGImg,
                x.TPExperience,
                x.TravelPlanIntro,
                x.CreateOn,
                x.country,
                x.city,
                x.Act,
                x.Culture,
                x.Food,
                x.Secret,
                x.Shopping,
                x.Religion

            });

            //var result1 = JsonConvert.SerializeObject(result); 
            //traveltype = x.TravelType.ToString(),
            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行計畫建立成功", result });
        }

        //    return Request.CreateResponse(HttpStatusCode.BadRequest);
        //}


        [JwtAuthFilter]
        [Route("bgimg")]
        public HttpResponseMessage PostPlanBackgroundImg()
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            //TravelPlan travelPlan = db.TravelPlans.Find();
            ////travelPlan.MemberId = Mid;
            //TravelPlan travelPlan = db.TravelPlans.Single(x => x.id == id);

            var postedFile = HttpContext.Current.Request.Files.Count > 0
                ? HttpContext.Current.Request.Files[0]
                : null;

            if (postedFile != null && postedFile.ContentLength > 0)
            {
                //string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                //int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
                string fileName = Utility.UploadPlanBGImage(postedFile);
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
                    Path = $"/Upload/TPBGimg/{fileName}"
                };
                //Userimage myfolder name where i want to save my image
                Uri imageUrl = uriBuilder.Uri;
                string TPBGImg = imageUrl.ToString();
                //string imgUrl = imageUrl.ToString();

                //db.Entry(travelPlan).State = EntityState.Modified;
                //db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    message = "已上傳旅行計畫背景圖片",
                    TPBGImg
                });
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, new
            {
                success = false,
                message = "無圖片，請選擇圖片上傳"
            });

        }




        [JwtAuthFilter]
        [Route("cyimg")]
        //frontend send travelplan.id back to upload
        public HttpResponseMessage PostPlanCountryImg()
        {
            //string token = Request.Headers.Authorization.Parameter;
            //JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            //int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            //TravelPlan travelPlan = db.TravelPlans.Find(Mid);
            //travelPlan.MemberId = Mid;
            //TravelPlan travelPlan = db.TravelPlans.Single(x => x.id == id);

            var postedFile = HttpContext.Current.Request.Files.Count > 0
                ? HttpContext.Current.Request.Files[0]
                : null;

            if (postedFile != null && postedFile.ContentLength > 0)
            {
                //string extension = postedFile.FileName.Split('.')[postedFile.FileName.Split('.').Length - 1];
                //int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
                string fileName = Utility.UploadPlanCYImage(postedFile);
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
                    Path = $"/Upload/TPCimg/{fileName}"
                };
                //Userimage myfolder name where i want to save my image
                Uri imageUrl = uriBuilder.Uri;
                string Cpicture = imageUrl.ToString();

                //db.Entry(travelPlan).State = EntityState.Modified;
                //db.SaveChanges();
                //string countryImg = countryImgUrl.ToString();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    message = "已上傳旅行計畫國家圖片",
                    Cpicture
                });
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, new
            {
                success = false,
                message = "無圖片，請選擇圖片上傳"
            });


        }


        // DELETE: api/ApiTravelPlans/5
        //[ResponseType(typeof(TravelPlan))]
        [JwtAuthFilter]
        [Route("delete/{id}")]
        public IHttpActionResult DeleteTravelPlan(int id)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            TravelPlan travelPlan = db.TravelPlans.Find(id);

            if (travelPlan == null)
            {
                return NotFound();
            }

            db.TravelPlans.Remove(travelPlan);
            db.SaveChanges();

            return Ok(new { success = true, message = "成功刪除旅行計畫" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TravelPlanExists(int id)
        {
            return db.TravelPlans.Count(e => e.id == id) > 0;
        }
    }
}