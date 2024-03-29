﻿using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;
using Os.NETCore.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Log
{
    public class LogHelper : ILogHelper
    { 
        public LogHelper()
        {
            string baseDir = ConfigurationHelper.GetBaseDirectory();
            var logCfg = new FileInfo(baseDir+@"\Configs\Log4net.config"); 
            XmlConfigurator.ConfigureAndWatch(LogManager.CreateRepository("NETCoreRepository"), logCfg);
        }

        /// <summary>
        /// 配置文件路径
        /// </summary>
        /// <param name="path"></param>
        private void ConfigFilePath(string path)
        {
            var repository = LogManager.GetRepository("NETCoreRepository");
            var appenders = repository.GetAppenders();
            foreach (RollingFileAppender roolingfileappender in appenders)
            {
                roolingfileappender.File = path;
                roolingfileappender.ActivateOptions();
            }
        }



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public void WriteLog(Type t, string info, LogType type, string fromIP)
        {
            var mLog = LogManager.GetLogger("NETCoreRepository",t);
            info = string.Format("FromIP:{0}\r\n{1}", fromIP, info);
            switch (type)
            {
                case LogType.Info:
                    mLog.Info(info);
                    break;
                case LogType.Error:
                    mLog.Error(info);
                    break;
                case LogType.Debug:
                    mLog.Debug(info);
                    break;
                default:
                    mLog.Info(info);
                    break;
            }

        }
    }
}
