using System.Web.Mvc;
using IExceptionFilter = System.Web.Mvc.IExceptionFilter;

namespace Kiosky.App_Start
{
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            filterContext.Result = new JsonResult
            {
                Data = new
                {
                    Success = false,
                    Source = filterContext.Exception.Source,
                    Message = filterContext.Exception.Message
                }
            };
        }
    }
}