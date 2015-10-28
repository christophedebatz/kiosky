using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kiosky.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTest : TestBase
    {
        private readonly string _uri = "http://localhost:52242/api-0.0.1/users";

        [Test]
        public void GettingStatusCode200WhenIRequestContactsList()
        {
            var result = base.Get(this._uri);

            base.AssertStatusCode(HttpStatusCode.OK, result);
        }

        [Test]
        public void GettingAListWhenIRequestContactsList()
        {
            var result = base.Get(this._uri);

            base.AssertStatusCode(HttpStatusCode.OK, result);

            var jarray = JArray.Parse(result.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(jarray.Count(), 3); // control how many users the controller display
        }
    }
}
