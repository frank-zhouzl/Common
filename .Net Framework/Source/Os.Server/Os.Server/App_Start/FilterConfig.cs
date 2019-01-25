using Os.Server.Filter;
using System.Web;
using System.Web.Mvc;

namespace Os.Server
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
