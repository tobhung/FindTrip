using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FindTrip_Web.Areas.Admin.Controllers
{
    public class CorsController : ApiController
    {
        // GET: api/Cors
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Cors/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cors
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Cors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Cors/5
        public void Delete(int id)
        {
        }
    }
}
