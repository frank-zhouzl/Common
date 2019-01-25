using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Os.Model.RequestModel;
using Os.Model.ResponseBody;
using Os.Server.Filter;

namespace Os.Server.Controllers
{
    [LogFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public Response<List<Person>> WebHttp2()
        {
            List<Person> personlist = new List<Person>();
            personlist.Add(new Person { id = "1101", name = "张三", sex = "男" });
            return new Response<List<Person>>(personlist);
        }
        public Response<Person> WebHttp()
        {
            Person person = new Person() { id = "1101", name = "张三", sex = "男" };
            return new Response<Person>(person);
        }
    }
}
