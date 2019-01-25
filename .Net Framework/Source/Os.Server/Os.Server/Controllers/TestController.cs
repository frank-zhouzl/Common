using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Os.Framework.Helper;
using Os.Model.RequestModel;
using Os.Model.ResponseBody;
using Os.Server.Filter;

namespace Os.Server.Controllers
{
    [LogFilter]
    public class TestController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public Response<List<Person>> WebHttp2()
        {
            List<Person> personlist = new List<Person>();
            personlist.Add(new Person { id = "1101", name = "张三", sex = "男" });
            return new Response<List<Person>>(personlist);
        }
        [HttpPost]
        public Response<Person> WebHttp()
        {
            Person person = new Person() { id = "1101", name = "张三", sex = "男" };
            var bc = 0;
            var num = 3 / bc;
            return new Response<Person>(person);
        }
        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        /// <summary>
        /// 调用示例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response<string> Import(int id)
        {
            //转换列名称成中文名
            Dictionary<string, string> RenameColumDic = new Dictionary<string, string>();

            string FieldName = "aaa";
            string DisplayName = "bbb";
            RenameColumDic.Add(FieldName, DisplayName);
            //将对象转成datatble
            DataTable dataTable = new DataTable();//ListToDataTableHelper.ToDataTable<ClinicalRegistrationCach>(list, RenameColumDic, queryParams.colCollection.Select(c => c.FieldName).ToArray());
            return new Response<string>(ImportHelper.Instance.GetDwonLoadlPath(dataTable, "CDEExclsName"));
        }
    }
}