using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;

namespace Kiosky.Controllers.Formatter
{
    public class XmlFormatter : XmlMediaTypeFormatter
    {
        public XmlFormatter()
        {
            this.Indent = true;
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/xml");
        }
    }
}