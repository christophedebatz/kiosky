using Kiosky.App_Start;
using Kiosky.Services.Repositories;
using KioskyInterfaces;
using Kiosky.Services.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Kiosky.Models;
using System.Web.Http;
using System.Net.Http;

namespace Kiosky.Controllers
{
    [Secure(Roles = "user")]
    [HandleException]
    public class UserController : ApiControllerBase<IUser>
    {
        UserRepository userRepository = new UserRepository();

        [ActionName("GetUsers")]
        public HttpResponseMessage GetUsers(HttpRequestMessage request, int offset = 0, int limit = ResultPerPage)
        {
            var users = this.userRepository.GetUsers();

            return CreateHttpResponse(
                request,
                new Paginator<IUser>(users, offset, limit)
            );
        }

        [ActionName("GetUser")]
        public HttpResponseMessage GetUser(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, this.userRepository.GetUser());
        }

        [HttpPut]
        [Secure(Roles = "admin")]
        [ActionName("CreateUser")]
        public HttpResponseMessage CreateUser(HttpRequestMessage request, User user)
        {
            try
            {
                var createdUser = this.userRepository.CreateUser(user);
                return CreateHttpResponse(request, createdUser, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return CreateHttpErrorResponse(HttpStatusCode.BadRequest, request, ex);
            }
        }

        [HttpPost]
        [Secure(Roles = "admin")]
        [ActionName("UpdateUser")]
        public HttpResponseMessage UpdateUser(HttpRequestMessage request, User user)
        {
            try
            {
                this.userRepository.UpdateUser(user);
                return CreateHttpResponse(HttpStatusCode.OK, request);
            }
            catch (Exception ex)
            {
                return CreateHttpErrorResponse(HttpStatusCode.BadRequest, request, ex);
            }
        }
    }
}
