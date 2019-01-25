using Newtonsoft.Json;
using Os.Framework.Log;
using Os.Model.ResponseBody;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using WebGrease.Css.Extensions;

namespace Os.Server.Filter
{
    public class OsExceptionFilterAttribute:ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            //出现异常返回友好提示
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, new Response<object>(ResultTypeEnum.ServiceException, "服务方法异常,请稍后再试"));
            var jsonInfo = new StringBuilder();
            jsonInfo.AppendLine("RequestUri:" + actionExecutedContext.Request.RequestUri);
            actionExecutedContext.ActionContext.ActionArguments.ForEach(x => jsonInfo.AppendLine(
                "\r\nRequestArgument:" + JsonConvert.SerializeObject(x.Value)));
            jsonInfo.AppendLine(actionExecutedContext.Exception.ToString());
            //日志记录真实异常信息
            var stream = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK, new Response<object>(ResultTypeEnum.ServiceException, actionExecutedContext.Exception.Message)).Content.ReadAsStreamAsync().Result;
            var encoding = Encoding.UTF8;
            var reader = new StreamReader(stream, encoding);
            stream.Position = 0;
            var info = new StringBuilder();
            info.Append(jsonInfo);
            info.Append("ReponseArgument:" + reader.ReadToEnd());
            info.AppendLine("\r\n****************************************************************************************************");
            LogRequest log = new LogRequest() { ipAddress = HttpContext.Current.Request.UserHostAddress, declaringType = MethodBase.GetCurrentMethod().DeclaringType, logType = LogType.Error, logInfo = info.ToString() };
            LogProvider.Instance.WriteLogAsyn(log);
        }
    }
}