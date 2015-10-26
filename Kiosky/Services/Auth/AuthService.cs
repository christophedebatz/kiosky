using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kiosky.Services.Repositories;
using Kiosky.Models;
using System.Security.Principal;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using KioskyInterfaces;
using Kiosky.Models.Dto;
using System.Net.Http;
using Kiosky.Services.Internal;

namespace Kiosky.Services.Auth
{
    public class AuthService
    {
        protected UserRepository UserDao;

        private const int Ttl = 60;

        public AuthService()
        {
            this.UserDao = new UserRepository();
        }

        public IDictionary<string, object> Login(Credentials credentials)
        {
            var user = this.UserDao.GetByUsernameAndPassword(
                credentials.Username, 
                credentials.Password
            );

            if (user != null)
            {
                // not very usefull because of restfull context
                HttpContext.Current.User = new GenericPrincipal(
                    new GenericIdentity(user.Username),
                    user.Roles
                );
                
                var token = GenerateToken(user);
                RefreshUserCache(user, token);

                return new Dictionary<string, object>
                {
                    {"token", token},
                    {"expireAt", DateTime.Now.AddMinutes(Ttl).ToString(new DateTimeFormatInfo())},
                    {"user",
                        new Dictionary<string, object>
                        {
                            {"name", user.Username},
                            {"roles", user.Roles}
                        }
                    }
                };
            }

            return null;
        }

        public static string RetrieveToken(HttpRequestMessage request, string headerTokenName)
        {
            if (headerTokenName == null)
            {
                throw new ArgumentNullException("headerTokenName");
            }

            try
            {
                // trying to extract token from headers
                return request.Headers.GetValues(headerTokenName).First();
            }
            catch (InvalidOperationException ex)
            {
                LoggerRegistrar.Factory.MonitoringLogger.Debug("No token was found in http request headers", ex);
                return null;
            }
        }

        public static void RemoveUserCache(string token)
        {
            HttpRuntime.Cache.Remove(token);
        }

        private static string GenerateToken(IUser user)
        {
            var timestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var unhashed = string.Format("{0}{1}{2}", user.Id, user.Username, timestamp);
            var token = EncryptWithSHA1(unhashed).Replace("-", "");

            return token.ToLower(CultureInfo.CurrentCulture);
        }

        private static void RefreshUserCache(IUser user, string token)
        {
            HttpRuntime.Cache.Add(token, user, null, DateTime.Now.AddMinutes(Ttl), TimeSpan.Zero,
                CacheItemPriority.High, null);
        }

        public static string EncryptWithSHA1(string str)
        {
            var sha1Factory = new SHA1CryptoServiceProvider();

            return BitConverter.ToString(
                sha1Factory.ComputeHash(
                    Encoding.Default.GetBytes(str)
                )
            );
        }
    }
}