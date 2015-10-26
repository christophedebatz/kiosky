using Kiosky.App_Start;
using Kiosky.Models.Dto;
using Kiosky.Services.Auth;
using Kiosky.Services.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Kiosky.Controllers
{
    public class AuthController : ApiControllerBase<IDictionary<string, object>>
    {
        protected AuthService AuthService;

        public AuthController()
        {
            this.AuthService = new AuthService();
        }

        [HttpPost]
        [ActionName("GetToken")]
        public HttpResponseMessage GetToken(HttpRequestMessage request)
        {
            var requestContext = new HttpContextWrapper(HttpContext.Current).Request;

            try
            {
                var loginDictionnary = this.GetLoginData(request, requestContext);

                if (loginDictionnary != null)
                {
                    return CreateHttpResponse(request, loginDictionnary);
                }
            }
            catch (ArgumentNullException)
            {
                return CreateHttpErrorResponse(
                    HttpStatusCode.BadRequest,
                    request,
                    "You must provide credentials"
                );
            }

            return CreateHttpErrorResponse(
                HttpStatusCode.Forbidden, 
                request, 
                new UnauthorizedAccessException("Bad credentials")
            );
        }

        [HttpDelete]
        [Secure(Roles = "user")]
        [ActionName("RemoveToken")]
        public HttpResponseMessage RemoveToken(HttpRequestMessage request)
        {
            string headerTokenName = ConfiguratorRegistrar.Config.GetAsString("KioskyAuthHeaderName");

            string userToken = AuthService.RetrieveToken(request, headerTokenName);

            AuthService.RemoveUserCache(userToken);

            return CreateHttpResponse(HttpStatusCode.NoContent, request);
        }

        /// <summary>
        /// Retrieve login data.
        /// </summary>
        /// <param name="requestContext">The current request object</param>
        /// <returns>The collection of data</returns>
        private IDictionary<string, object> GetLoginData(HttpRequestMessage request, HttpRequestBase requestContext)
        {
            IDictionary<string, object> loginDictionnary = null;

            if (requestContext.Form["username"] == null || requestContext.Form["password"] == null)
            {
                throw new ArgumentNullException("credentials");
            }

            var credentials = new Credentials(
                requestContext["username"],
                requestContext["password"]
            );

            try
            {
                loginDictionnary = this.AuthService.Login(credentials);
            }
            catch (AggregateException)
            {
                string exceptionMessage = "Login failed. Given data " + credentials;
                System.Diagnostics.Debug.WriteLine(exceptionMessage);
            }

            return loginDictionnary;
        }
    }
}
