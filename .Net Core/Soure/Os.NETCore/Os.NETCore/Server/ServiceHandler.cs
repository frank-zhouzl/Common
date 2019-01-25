using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Os.NETCore.Helper;
using Os.NETCore.Model;

namespace Os.NETCore.Server
{
    /// <summary>
    /// 服务请求处理程序
    /// </summary>
    public class ServiceHandler
    {
        // 依然是静态自动hold实例
        private static volatile ServiceHandler instance = null;

        private static volatile Dictionary<string, Service> serviceList = null;
        // Lock对象，线程安全所用
        private static object syncRoot = new Object();
        private ServiceHandler()
        {
            serviceList = ServiceCommon.InitServiceList();
        }

        public static ServiceHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ServiceHandler();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 返回字符串对象
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="param">请求的参数，GET请求传字符串，Post请求直接传对象</param>
        /// <returns>返回字符串</returns>
        public string RequestStr(string serviceName, object param)
        {
            Service service = null;
            string resultStr = "";
            //判断是否存在该服务的配置
            if (!serviceList.Keys.Contains(serviceName))
            {
                throw new Exception("调用服务未找到！");
            }
            service = serviceList[serviceName];
            switch (service.Method.ToUpper())
            {
                case "GET":
                    resultStr = Get(service.Url, param as string);
                    break;
                case "POST":
                    resultStr = Post(service.Url, param);
                    break;
                default:
                    break;
            }
            return resultStr;
        }


        /// <summary>
        /// 向服务器发起请求
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="serviceName">服务名称</param>
        /// <param name="param">请求的参数，GET请求传字符串，Post请求直接传对象</param>
        /// <returns>返回一个反序列化过后的对象</returns>
        public T Request<T>(string serviceName, object param) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(RequestStr(serviceName, param));
        }
        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Service> getServiceList()
        {
            return ServiceCommon.InitServiceList();
        }
        /// <summary>
        /// Get方式
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private string Get(string url, string param)
        {
            string result = WebRequestHelper.GetData(param, url);
            return result;
        }
        /// <summary>
        /// Post方式
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        private string Post(string url, object param)
        {
            string paramString;
            if (param is null)
            {
                paramString = "";
            }
            else
            {
                paramString = JsonConvert.SerializeObject(param);
            }
            string result = WebRequestHelper.PostData(paramString, url);
            return result;
        }
    }
}
