using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;
using Kiosky.Controllers.Formatter;
using Kiosky.Services;
using Kiosky.Services.Internal;
using System;
using MongoDB.Bson.Serialization;
using Kiosky.Models;

namespace Kiosky
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            DeclareApiFormatters(config); // adding formatters
            DeclareApiRoutes(config);     // adding routes
        }

        /// <summary>
        /// Decalre API formatters into global services configuration.
        /// </summary>
        /// <param name="config">The input config</param>
        private static void DeclareApiFormatters(HttpConfiguration config)
        {
            // adding valid formatter to display services data
            config.Formatters.Clear();

            config.Formatters.Add(MediaFormatterFactory.GetFormatter("xml"));
            config.Formatters.Add(MediaFormatterFactory.GetFormatter("json"));
        }

        /// <summary>
        /// Declare API routes descriptions into global services configuration.
        /// </summary>
        /// <param name="config">The input config</param>
        private static void DeclareApiRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "CreateUser",
                routeTemplate: ConstructRouteUri("/users"),
                defaults: new { controller = "User", action = "CreateUser" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) }
            );

            config.Routes.MapHttpRoute(
                name: "UpdateUser",
                routeTemplate: ConstructRouteUri("/users"),
                defaults: new { controller = "User", action = "UpdateUser" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );

            config.Routes.MapHttpRoute(
                name: "GetUser",
                routeTemplate: ConstructRouteUri("/user"),
                defaults: new { controller = "User", action = "GetUser" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "GetUsers",
                routeTemplate: ConstructRouteUri("/users"),
                defaults: new { controller = "User", action = "GetUsers" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "GetToken",
                routeTemplate: ConstructRouteUri("/token"),
                defaults: new { controller = "Auth", action = "GetToken" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) }
            );
        }

        /// <summary>
        /// Return a route with the good api version prefix.
        /// </summary>
        /// <param name="uri">route http</param>
        /// <returns>The entier route</returns>
        private static string ConstructRouteUri(string uri)
        {
            if (uri == null) {
                throw new ArgumentException("Null uri argument in method ConstructRouteUri");
            }

            if (!uri[0].Equals('/')) {
                uri = "/" + uri;                
            }

            string apiVersion = ConfiguratorRegistrar.Config.GetAsString("ApiVersion");

            return string.Format("api-{0}{1}", apiVersion, uri);
        }
    }
}
