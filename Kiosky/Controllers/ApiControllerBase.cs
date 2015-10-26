using Kiosky.Services.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace Kiosky.Controllers
{
    /// <summary>
    /// Base class for ApiController
    /// </summary>
    public class ApiControllerBase<T> : ApiController
    {

        protected const int ResultPerPage = 10;

        internal ApiControllerBase()
        {
        }

        /// <summary>
        /// Log and return Http error (with custom error message).
        /// </summary>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="request">The input request</param>
        /// <param name="message">An additional message for log and response</param>
        /// <returns>The http error message object</returns>
        protected HttpResponseMessage CreateHttpErrorResponse(HttpStatusCode statusCode, HttpRequestMessage request, string message)
        {
            LoggerRegistrar.Factory.ExceptionLogger.Error(message);
            return request.CreateErrorResponse(statusCode, new Exception(message));
        }

        /// <summary>
        /// Log and return Http error.
        /// </summary>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="request">The input request</param>
        /// <param name="exception">The error exception</param>
        /// <returns>The http error message object</returns>
        protected HttpResponseMessage CreateHttpErrorResponse(HttpStatusCode statusCode, HttpRequestMessage request, Exception exception)
        {
            LoggerRegistrar.Factory.ExceptionLogger.Error(exception.Message, exception);
            return request.CreateErrorResponse(statusCode, exception);
        }

        /// <summary>
        /// Log and return Http error (with custom error message).
        /// </summary>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="request">The input request</param>
        /// <param name="exception">The error exception</param>
        /// <param name="message">An additional message for log and response</param>
        /// <returns>The http error message object</returns>
        protected HttpResponseMessage CreateHttpErrorResponse(HttpStatusCode statusCode, HttpRequestMessage request, Exception exception, string message)
        {
            LoggerRegistrar.Factory.ExceptionLogger.Error(message, exception);
            return request.CreateErrorResponse(statusCode, message, exception);
        }

        /// <summary>
        /// Return a simple response without body data.
        /// </summary>
        /// <param name="statusCode">The status code of the response</param>
        /// <param name="request">The input request</param>
        /// <returns>The http response</returns>
        protected HttpResponseMessage CreateHttpResponse(HttpStatusCode statusCode, HttpRequestMessage request)
        {
            return request.CreateResponse(statusCode);
        }

        /// <summary>
        /// Create paginated http response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paginator"></param>
        /// <returns></returns>
        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Paginator<T> paginator, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (paginator.Items.Count() > ResultPerPage)
            {
                var links = this.CreateNavigationLinks(this.ControllerContext, paginator);

                return request.CreateResponse(
                    statusCode,
                    new { results = paginator.Items, links = links }
                );
            }

            return request.CreateResponse(
                statusCode,
                new { results = paginator.Items }
            );
        }

        /// <summary>
        /// Create paginated http response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, IEnumerable<T> items, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return request.CreateResponse(
                statusCode,
                new { results = items }
            );
        }

        /// <summary>
        /// Create paginated http response.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, T item, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return request.CreateResponse(
                statusCode,
                item
            );
        }

        /// <summary>
        /// Construct previous and next links for collections pagination.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="paginator"></param>
        /// <returns></returns>
        private object CreateNavigationLinks(HttpControllerContext controllerContext, Paginator<T> paginator)
        {
            var action = controllerContext.RouteData.Values["action"].ToString();
            var controller = controllerContext.RouteData.Values["controller"].ToString();

            return new
            {
                First = this.Url.Link(
                    action,
                    new
                    {
                        Controller = controller,
                        Action = action,
                        offset = paginator.First.Offset,
                        limit = paginator.First.Limit
                    }
                ),

                Prev = this.Url.Link(
                    action,
                    new
                    {
                        Controller = controller,
                        Action = action,
                        offset = paginator.Previous.Offset,
                        limit = paginator.Previous.Limit
                    }
                ),

                Next = this.Url.Link(
                    action,
                    new
                    {
                        Controller = controller,
                        Action = action,
                        offset = paginator.Next.Offset,
                        limit = paginator.Next.Limit
                    }
                ),

                Last = this.Url.Link(
                    action,
                    new
                    {
                        Controller = controller,
                        Action = action,
                        offset = paginator.Last.Offset,
                        limit = paginator.Last.Limit
                    }
                )
            };
        }
    }
}
