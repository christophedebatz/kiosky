using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Kiosky.Services;
using Kiosky.Services.Internal;
using Kiosky.Services.Auth;
using KioskyInterfaces;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Web;

namespace Kiosky.App_Start
{
    public sealed class SecureAttribute : AuthorizationFilterAttribute
    {
        public IUser CurrentUser { get; private set; }
        
        public string Roles { get; set; }

        private const string CacheKey = "UserTokenStore";

        private Cache Cache;        


        public SecureAttribute()
        {
            this.Cache = HttpRuntime.Cache;
        }

        /// <summary>
        /// Called as an authorization filter. Always before controller call.
        /// </summary>
        /// <param name="actionContext">The current request action context.</param>
        /// <exception cref="UnauthorizedAccessException">Raised if user access is denied.</exception>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (!this.Authorize(actionContext.Request))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Forbidden,
                    new UnauthorizedAccessException("Access denied")
                );
            }
        }

        /// <summary>
        /// In charge of knowing if a request can access to controller or not.
        /// </summary>
        /// <param name="request">The current request that ask for authorization.</param>
        /// <returns>true if request has access, false else.</returns>
        private bool Authorize(HttpRequestMessage request)
        {
            string headerTokenName = ConfiguratorRegistrar.Config.GetAsString("KioskyAuthHeaderName");
            string givenToken = AuthService.RetrieveToken(request, headerTokenName);

            if (givenToken != null && this.Cache.Get(givenToken) != null)
            {
                this.CurrentUser = (IUser)this.Cache.Get(givenToken);
                var rolesArray = Regex.Replace(this.Roles, @"\s+", "").Split(',');

                if (this.CurrentUser.Roles.Intersect(rolesArray).Count() == rolesArray.Length)
                {
                    this.Cache.Remove(givenToken);
                    this.Cache.Add(givenToken, this.CurrentUser, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero,
                        CacheItemPriority.High, null);

                    return true;
                }
            }

            return false;
        }
    }
}