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


namespace FindTrip_Web.Areas.Admin.Controllers
{
    [RoutePrefix("api/plan")]
    public class ApiTravelPlansController : ApiController
    {
        private Model1 db = new Model1();
        TravelPlan travlPlan = new TravelPlan();


        // GET: api/ApiTravelPlans
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


        [Route("index")]
        public HttpResponseMessage GetMemberPlans(TravelPlan travelPlan)
        {
            Countries country = new Countries();

            var countries = db.Countries.Select(c => new
            {
                c.id,
                country = c.country,
                city = c.Districts.Where(d => d.Cid == c.id).Select(d => d.city)
            });

            var allPlans = db.TravelPlans.Select(x => new
            {
                id = x.id,
                MemberId = x.MemberId,
                points = x.TPPrice,
                Cpicture = x.Cpicture,
                manpic = x.MyMember.manpic,
                name = x.MyMember.name,
                //x.CountryId,
                //country = x.MyCountry.country,
                //city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),
                x.CreateOn,
                //star = x.MyRating.star,
                //rating = x.MyRating.rating,
                //tags = db.TravelPlans.Where(y => y.id == travelPlan.id).Select(y => new
                //{
                //    y.Religion,
                //    y.Secret,
                //    y.Act,
                //    y.Food,
                //    y.Culture,
                //    y.Shopping,
                //})

                tags = new
                {
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Secret,
                    x.Shopping,
                    x.Religion
                }


            });

            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, allPlans, countries });

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
                country = c.country,
                city = c.Districts.Where(d => d.Cid == c.id).Select(d => d.city)
            });

            var allPlans = db.TravelPlans.Select(x => new
            {
                id = x.id,
                MemberId = x.MyMember.id,
                points = x.TPPrice,
                name = x.MyMember.name,
                manpic = x.MyMember.manpic,
                Cpicture = x.Cpicture,
                //star = x.MyRating.star,
                //rating = x.MyRating.rating,
                //country = x.MyCountry.country,
                //city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),
                x.CreateOn,

                //x.TPBGImg,
                //x.TravelPlanIntro,
                //tags = db.TravelPlans.Where(z => z.id == x.id).Select(z => new
                //{
                //    z.Act,
                //    z.Culture,
                //    z.Food,
                //    z.Religion,
                //    z.Secret,
                //    z.Shopping
                //})

                tags = new
                {
                    x.Act,
                    x.Culture,
                    x.Food,
                    x.Religion,
                    x.Secret,
                    x.Shopping
                }

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

            var result = db.TravelPlans.Select(x => new
            {
                id = x.id,
                MemberId = x.MyMember.id,
                points = x.TPPrice,
                //x.TravelPlanIntro,
                //x.TPExperience,
                //x.CountryId,
                //country = x.MyCountry.country,
                //city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),
                Cpicture = x.Cpicture,
                x.TPBGImg,
                x.TravelPlanIntro,
                x.TPExperience,
                x.CreateOn,
                //manpic = x.MyMember.UserImg,
                //x.MyMember.MemberIntro,
                //x.MyMember.PlannerSocial3,
                //x.MyMember.PlannerSocial4,

                //tags = db.TravelPlans.Where(z => z.id == x.id).Select(z => new
                //{
                //    z.Religion,
                //    z.Secret,
                //    z.Act,
                //    z.Food,
                //    z.Culture,
                //    z.Shopping,
                //})

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


        //// PUT: api/ApiTravelPlans/5
        ////[ResponseType(typeof(void))]
        [JwtAuthFilter]
        [Route("update/{id}")]
        public HttpResponseMessage PatchTravelPlan(int id, TravelPlan travelPlan)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            var planChanges = db.TravelPlans.Find(id);
            //TravelPlan travelPlan = db.TravelPlans.Find(id);
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


            //if (id != travelPlan.id)
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound);
            //}

            //travelPlan.MemberId = Mid;


            //planChanges.id = travelPlan.id;
            planChanges.TravelPlanIntro = travelPlan.TravelPlanIntro;
            planChanges.TPExperience = travelPlan.TPExperience;
            //planChanges.MyCountry.id = travelPlan.MyCountry.id;
            //planChanges.MyCountry.country = travelPlan.MyCountry.country;
            planChanges.TPPrice = travelPlan.TPPrice;
            planChanges.Act = travelPlan.Act;
            planChanges.Culture = travelPlan.Culture;
            planChanges.Food = travelPlan.Food;
            planChanges.Secret = travelPlan.Secret;
            planChanges.Shopping = travelPlan.Shopping;

            //db.TravelPlans.Attach(travelPlan);
            //db.Entry(travelPlan).State = EntityState.Modified;
            db.SaveChanges();

            var result = db.TravelPlans.Where(x => x.id == planChanges.id).Select(x => new
            {
                id = travelPlan.id,
                MemberId = travelPlan.MemberId,
                point = travelPlan.TPPrice,
                //Cpicture = travelPlan.TPMainImg,
                //manpic = travelPlan.MyMember.UserImg,
                name = travelPlan.MyMember.name,
                //country = travelPlan.MyCountry.country,
                //city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),

                travelPlan.TravelPlanIntro,
                travelPlan.TPExperience,

                tags = db.TravelPlans.Where(z => z.id == travelPlan.id).Select(z => new
                {
                    z.Act,
                    z.Secret,
                    z.Culture,
                    z.Food,
                    z.Shopping,
                    z.Religion
                })
            });



            return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行計畫修改成功" });
        }





        // POST: api/ApiTravelPlans
        //[ResponseType(typeof(TravelPlan))]


        //[JwtAuthFilter]
        //[Route("create")]
        //public HttpResponseMessage PostTravelPlan(TravelPlan travelPlan)
        //{
        //    string token = Request.Headers.Authorization.Parameter;
        //    JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
        //    int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

        //    travelPlan.MemberId = Mid;
        //    travelPlan.CreateOn = DateTime.Now; //only year/mth/day


        //    db.TravelPlans.Add(travelPlan);
        //    db.SaveChanges();

        //    var result = db.TravelPlans.Where(x => x.id == travelPlan.id).Select(x => new
        //    {
        //        id = x.id,
        //        MemberId = x.MyMember.id,
        //        points = x.TPPrice,
        //        //x.TravelPlanIntro,
        //        //x.TPExperience,
        //        //x.CountryId,
        //        country = x.MyCountry.country,
        //        city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),
        //        Cpicture = x.Cpicture,
        //        x.TPBGImg,
        //        //manpic = x.MyMember.UserImg,
        //        x.TPExperience,
        //        x.TravelPlanIntro,
        //        x.CreateOn,


        //        //tags = db.TravelPlans.Where(z => z.id == travelPlan.id).Select(z => new
        //        //{
        //        //    z.Religion,
        //        //    z.Secret,
        //        //    z.Act,
        //        //    z.Food,
        //        //    z.Culture,
        //        //    z.Shopping,
        //        //})

        //        tags = new
        //        {
        //            x.Religion,
        //            x.Secret,
        //            x.Act,
        //            x.Food,
        //            x.Culture,
        //            x.Shopping
        //        }

        //    });
        //    //traveltype = x.TravelType.ToString(),
        //    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "旅行計畫建立成功", result });
        //}


        [JwtAuthFilter]
        [Route("create")]
        public HttpResponseMessage PostTravelPlan(TravelPlan travelPlan)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));
            //TravelPlan travelPlan = new TravelPlan();
            travelPlan.MemberId = Mid;
            travelPlan.CreateOn = DateTime.Now; //only year/mth/day


            db.TravelPlans.Add(travelPlan);
            db.SaveChanges();

            var result = db.TravelPlans.Where(x => x.id == travelPlan.id).Select(x => new
            {
                id = x.id,
                MemberId = x.MyMember.id,
                points = x.TPPrice,
                //x.TravelPlanIntro,
                //x.TPExperience,
                //x.CountryId,
                //country = x.MyCountry.country,
                //city = x.MyCountry.Districts.Where(y => y.Cid == x.CountryId).Select(y => y.city),
                Cpicture = x.Cpicture,
                x.TPBGImg,
                //manpic = x.MyMember.UserImg,
                x.TPExperience,
                x.TravelPlanIntro,
                x.CreateOn,
                x.country,
                x.city,


                    //tags = db.TravelPlans.Where(z => z.id == travelPlan.id).Select(z => new
                //{
                //    z.Religion,
                //    z.Secret,
                //    z.Act,
                //    z.Food,
                //    z.Culture,
                //    z.Shopping,
                //})

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
                int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
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
                int MaxContentLength = 1024 * 1024 * 1; //Size = 1MB
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