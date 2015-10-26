using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KioskyInterfaces
{
    public interface IUser
    {
        int Id { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string Email { get; set; }

        string[] Roles { get; set; }

        DateTime LastModified { get; set; }

        DateTime CreatedAt { get; set; }
    }
}
