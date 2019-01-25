using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebGrease.Css.Extensions;
using Os.Framework.Log;

namespace Os.Server.Filter
{
    public class LogFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            StringBuilder jsonInfo;
            //为本次会话增加SessionId
            jsonInfo = new StringBuilder();
            jsonInfo.AppendLine();
            jsonInfo.Append("RequestUri:" + actionContext.Request.RequestUri);
            actionContext.ActionArguments.ForEach(x => jsonInfo.AppendLine(
                "\r\nRequestArgument:" + JsonConvert.SerializeObject(x.Value)
                )
                );
            LogRequest log = new LogRequest() { ipAddress = HttpContext.Current.Request.UserHostAddress, declaringType = MethodBase.GetCurrentMethod().DeclaringType, logType = LogType.Info, logInfo = jsonInfo.ToString() };
            LogProvider.Instance.WriteLogAsyn(log);
        }


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                throw new Exception("发生错误", actionExecutedContext.Exception);
            }
            var stream = actionExecutedContext.Response.Content.ReadAsStreamAsync().Result;
            var encoding = Encoding.UTF8;
            var reader = new StreamReader(stream, encoding);
            var result = reader.ReadToEnd();
            stream.Position = 0;
            var info = new StringBuilder();
            info.Append("RequestUri:" + actionExecutedContext.Request.RequestUri);
            actionExecutedContext.ActionContext.ActionArguments.ForEach(x => info.AppendLine(
                "\r\nRequestArgument:" + JsonConvert.SerializeObject(x.Value)));
            info.Append("ResponseArgument:" + result);
            info.AppendLine("\r\n****************************************************************************************************");
            LogRequest log = new LogRequest() { ipAddress = HttpContext.Current.Request.UserHostAddress, declaringType = MethodBase.GetCurrentMethod().DeclaringType, logType = LogType.Info, logInfo = info.ToString() };
            LogProvider.Instance.WriteLogAsyn(log);
        }

    }
}