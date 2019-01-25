using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;

namespace Os.Server.Filter
{
    public class OsExceptionFilterAttribute:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            //出现异常返回友好提示
            //actionExecutedContext.Exception.Data;// = actionExecutedContext.Exception.CreateResponse(HttpStatusCode.OK, new Response<object>(ResultTypeEnum.ServiceException, "服务方法异常,请稍后再试"));
            var jsonInfo = new StringBuilder();
            jsonInfo.AppendLine("RequestUri:" + actionExecutedContext.HttpContext.Request.Host);
        }
    }
}