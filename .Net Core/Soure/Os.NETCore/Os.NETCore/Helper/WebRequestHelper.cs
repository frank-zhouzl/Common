using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Helper
{
    public class WebRequestHelper
    {

        /// <summary>
        /// 以POST 形式请求数据
        /// </summary>
        /// <param name="RequestPara"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string PostData(string RequestPara, string Url)
        {
            return PostData(RequestPara, Url, "application/json");
        }

        /// <summary>
        /// 以POST 形式请求数据
        /// </summary>
        /// <param name="RequestPara"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string PostData(string RequestPara, string Url, string ContentType)
        {
            WebRequest hr = HttpWebRequest.Create(Url);
            string ReturnVal = null;
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(RequestPara);
            hr.ContentType = ContentType;
            hr.ContentLength = buf.Length;
            hr.Method = "POST";
            using (Stream RequestStream = hr.GetRequestStream())
            {
                RequestStream.Write(buf, 0, buf.Length);
                RequestStream.Close();
            }
            using (WebResponse response = hr.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    ReturnVal = reader.ReadToEnd();
                }
            }

            return ReturnVal;
        }

        /// <summary>
        /// 以GET 形式获取数据
        /// </summary>
        /// <param name="RequestPara"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string GetData(string RequestPara, string Url)
        {
            string ReturnVal = null;
            RequestPara = RequestPara.IndexOf('?') > -1 ? (RequestPara) : ("?" + RequestPara);
            WebRequest hr = HttpWebRequest.Create(Url + RequestPara);
            byte[] buf = Encoding.GetEncoding("utf-8").GetBytes(RequestPara);
            hr.Method = "GET";
            using (WebResponse response = hr.GetResponse())
            {
                using(StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                {
                    ReturnVal = reader.ReadToEnd();
                }
            }
            return ReturnVal;
        }
    }
}
