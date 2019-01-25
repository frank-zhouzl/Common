using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Model.ResponseBody
{
    public class ResponseBase
    {
        public ResultTypeEnum ResultType { get; set; }

        public string Message { get; set; }
    }

    public class Response<T> : ResponseBase
    {

        public T Data { get; set; }

        public Response()
        {
        }

        public Response(T data)
        {
            this.ResultType = ResultTypeEnum.Success;
            this.Data = data;
        }


        public Response(T data, string strMsg)
        {
            this.ResultType = ResultTypeEnum.Success;
            this.Data = data;
            this.Message = strMsg;
        }

        public Response(ResultTypeEnum resultType, string message)
        {
            ResultType = resultType;
            Message = message;
        }

    }
}
