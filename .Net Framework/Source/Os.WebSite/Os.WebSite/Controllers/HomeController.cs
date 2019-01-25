using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Os.Framework.Server;
using Os.Model.RequestModel;
using Os.Model.ResponseBody;

namespace Os.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ContentResult WebRequestTest2()
        {
            var t = ServiceHandler.Instance.Request<Response<List<Person>>>("HomeWebHttpTest2",null);
            return Content(JsonConvert.SerializeObject(t.Data));
        }
        public ContentResult WebRequestTest()
        {
            var t = ServiceHandler.Instance.Request<Response<Person>>("HomeWebHttpTest", null);
            return Content(JsonConvert.SerializeObject(t.Data));
        }
    }
}
