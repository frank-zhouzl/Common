using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Model.ResponseBody
{ 
       /// <summary>
       /// 接口调用结果
       /// </summary>
        public enum ResultTypeEnum
        {
            [Description("成功")]
            Success = 1,

            [Description("服务方法异常,错误号:{0}")]
            ServiceException = 1000
        }

}
