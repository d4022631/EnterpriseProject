using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using BookingBlock.EntityFramework;
using BookingBlock.WebApplication.Models;
using Microsoft.AspNet.Identity;

namespace BookingBlock.WebApplication.ApiControllers
{
    public abstract class BaseApiController : ApiController
    {
        protected ApplicationDbContext db = new ApplicationDbContext();

        private Lazy<PostcodesService> _postcodesService = new Lazy<PostcodesService>(ValueFactory);

        private static PostcodesService ValueFactory()
        {
            return new PostcodesService();
        }

        public PostcodesService PostcodesService
        {
            get { return _postcodesService.Value; }
        }

        public bool IsUserAuthenticated
        {
            get
            {
                var t = Request.GetQueryString("api_key");
                if (!string.IsNullOrWhiteSpace(t))
                {
                    return true;
                }

                var cl = this.ClaimsPrincipal;

                if (cl != null)
                {
                    return cl.Identity.IsAuthenticated;
                }

                return false;
            }
        }

        public string UserId
        {
            get
            {

                var t = Request.GetQueryString("api_key");
                if (!string.IsNullOrWhiteSpace(t))
                {
                    return t;
                }

                var ownerId = User.Identity.GetUserId();

      

                return ownerId;
            }
        }

        protected IHttpActionResult InvalidModel()
        {

            string validationErrors = string.Join(",",
                ModelState.Values.Where(E => E.Errors.Count > 0)
                .SelectMany(E => E.Errors)
                .Select(E => E.ErrorMessage + E.Exception?.Message)
                .ToArray());

            return BadRequest(validationErrors);
        }

        public ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                var user = User as ClaimsPrincipal;

                

                


                return user;
            }
        }

        internal IHttpActionResult Response<T>(HttpStatusCode statusCode, T content) where T : ApiResponse
        {
           
            return Content(statusCode, content);
        }
    }


    /// <summary>
    /// Extends the HttpRequestMessage collection
    /// </summary>
    public static class HttpRequestMessageExtensions
    {

        /// <summary>
        /// Returns a dictionary of QueryStrings that's easier to work with 
        /// than GetQueryNameValuePairs KevValuePairs collection.
        /// 
        /// If you need to pull a few single values use GetQueryString instead.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStrings(this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns an individual querystring value
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            // IEnumerable<KeyValuePair<string,string>> - right!
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
                return null;

            return match.Value;
        }

        /// <summary>
        /// Returns an individual HTTP Header value
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetHeader(this HttpRequestMessage request, string key)
        {
            IEnumerable<string> keys = null;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }

        /// <summary>
        /// Retrieves an individual cookie from the cookies collection
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpRequestMessage request, string cookieName)
        {
            CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie != null)
                return cookie[cookieName].Value;

            return null;
        }

    }
}