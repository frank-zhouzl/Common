using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Os.Framework.Helper;

namespace Os.Framework.Log
{
    public class LogProvider
    {
        //存放日志的接口实例
        private Dictionary<Type, ILogHelper> Log = new Dictionary<Type, ILogHelper>();
        //静态自动hold实例
        private static volatile LogProvider instance = null;
        // Lock对象，线程安全所用
        private static object syncRoot = new Object();

        private LogProvider()
        {
            //为服务做配置化接口注册
            Log.Add(typeof(LogHelper), new LogHelper());
        }

        public static LogProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LogProvider();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 获取LogHelper实例
        /// </summary>
        /// <returns>Log实例，如果不存在则返回null</returns>
        public ILogHelper GetLogHelper()
        {
            switch ((LogTypeEnum)Convert.ToInt32(ConfigHelper.Instance.getSystemConfig().LogSet))
            {
                case LogTypeEnum.Log4Net:
                    return Log[typeof(LogHelper)];
            }
            return null;
        }

        /// <summary>
        /// 异步调用日志记录
        /// </summary>
        /// <param name="logInfo"></param>
        public void WriteLogAsyn(LogRequest logInfo)
        {
            //委托异步打印日志
            Thread t = new Thread(new ParameterizedThreadStart(WriteLog));
            t.Start(logInfo);
        }

        /// <summary>
        /// 写日志方法
        /// </summary>
        /// <param name="o"></param>
        private void WriteLog(object o)
        {
            LogRequest log = o as LogRequest;
            this.GetLogHelper().WriteLog(log.declaringType, log.logInfo, log.logType, log.ipAddress);
        }
    }

    /// <summary>
    ///日志实体对象 
    /// </summary>
    public class LogRequest
    {
        /// <summary>
        /// 声明操作类型
        /// </summary>
        public Type declaringType { get; set; }
        /// <summary>
        /// 日志信息字符串
        /// </summary>
        public string logInfo { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType logType { get; set; }
        /// <summary>
        /// 请求源ip地址
        /// </summary>
        public string ipAddress { get; set; }
    }
}
