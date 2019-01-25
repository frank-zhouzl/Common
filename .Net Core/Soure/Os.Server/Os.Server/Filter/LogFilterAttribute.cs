using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using CoreExtension;
using Microsoft.AspNetCore.Http;
using Os.NETCore.Log;

namespace Os.Server.Filter
{
    public class LogFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            StringBuilder jsonInfo;
            //为本次会话增加SessionId
            jsonInfo = new StringBuilder();
            jsonInfo.AppendLine();
            jsonInfo.Append("RequestUri:" + actionContext.HttpContext.Request.GetAbsoluteUri());
            actionContext.ActionArguments.ForEach(x => jsonInfo.AppendLine(
                "\r\nRequestArgument:" + JsonConvert.SerializeObject(x.Value)
                )
                );
            LogRequest log = new LogRequest() { ipAddress = actionContext.HttpContext.GetUserIp(), declaringType = MethodBase.GetCurrentMethod().DeclaringType, logType = LogType.Info, logInfo = jsonInfo.ToString() };
            LogProvider.Instance.WriteLogAsyn(log);
        }


        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                throw new Exception("发生错误", actionExecutedContext.Exception);
            }
            //var stream = actionExecutedContext.HttpContext.Response.ToString();
            //var encoding = Encoding.UTF8;
            //var reader = new StreamReader(stream, encoding);
            var result = "aaaaaaa";
            var info = new StringBuilder();
            info.Append("RequestUri:" + actionExecutedContext.HttpContext.Request.GetAbsoluteUri());
            //actionExecutedContext.HttpContext.Items.ForEach(x => info.AppendLine(
            //    "\r\nRequestArgument:" + JsonConvert.SerializeObject(x.Value)));
            info.Append("ResponseArgument:" + result);
            info.AppendLine("\r\n****************************************************************************************************");
            LogRequest log = new LogRequest() { ipAddress = actionExecutedContext.HttpContext.GetUserIp(), declaringType = MethodBase.GetCurrentMethod().DeclaringType, logType = LogType.Info, logInfo = info.ToString() };
            LogProvider.Instance.WriteLogAsyn(log);
        }

    }
}