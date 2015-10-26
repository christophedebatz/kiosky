using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiosky.Models.Dto
{
    public class Credentials
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public Credentials(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override string ToString()
        {
            return "[ username = " + this.Username + ", password = " + this.Password + " ]";
        }
    }
}