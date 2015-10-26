using Kiosky.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Kiosky
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}