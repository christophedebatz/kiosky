using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiosky.Models.Dto
{
    public class Response<T>
    {
        public T Content { get; set; }

        public string Name { get; protected set; }

        public Response(string name, T content)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}