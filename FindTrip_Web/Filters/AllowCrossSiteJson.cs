using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace FindTrip_Web.Filters
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "localhost://8080, http://findtrip.rocket-coding.com");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With,Content-Type");
                actionExecutedContext.Response.Headers.Add("Access-Control-Request-Headers","Authorization");

                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}