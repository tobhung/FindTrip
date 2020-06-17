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
using Newtonsoft.Json;
using MessageReply = FindTrip_Web.Models.MessageReply;
using FindTrip_Web.Filters;
using JWT;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    public class ApiMessageRepliesController : ApiController
    {
        private Model1 db = new Model1();

        // GET: api/ApiMessageReplies
        public IQueryable<MessageReply> GetMessageReplies()
        {
            return db.MessageReplies;
        }

        // GET: api/ApiMessageReplies/5
        [ResponseType(typeof(MessageReply))]
        public IHttpActionResult GetMessageReply(int id)
        {
            MessageReply messageReply = db.MessageReplies.Find(id);
            if (messageReply == null)
            {
                return NotFound();
            }

            return Ok(messageReply);
        }

        // PUT: api/ApiMessageReplies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessageReply(int id, MessageReply messageReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messageReply.id)
            {
                return BadRequest();
            }

            db.Entry(messageReply).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageReplyExists(id))
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

        // POST: api/ApiMessageReplies
        [JwtAuthFilter]
        public HttpResponseMessage PostMessageReply(MessageReply messageReply)
        {
            string token = Request.Headers.Authorization.Parameter;
            JwtAuthUtil jwtAuthUtil = new JwtAuthUtil();
            int Mid = Convert.ToInt32(jwtAuthUtil.GetId(token));

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            messageReply.MemberId = Mid;



            db.MessageReplies.Add(messageReply);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE: api/ApiMessageReplies/5
        [ResponseType(typeof(MessageReply))]
        public IHttpActionResult DeleteMessageReply(int id)
        {
            MessageReply messageReply = db.MessageReplies.Find(id);
            if (messageReply == null)
            {
                return NotFound();
            }

            db.MessageReplies.Remove(messageReply);
            db.SaveChanges();

            return Ok(messageReply);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageReplyExists(int id)
        {
            return db.MessageReplies.Count(e => e.id == id) > 0;
        }
    }
}