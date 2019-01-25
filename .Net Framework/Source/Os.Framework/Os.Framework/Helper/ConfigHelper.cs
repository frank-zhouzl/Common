using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Os.Framework.Cache;

namespace Os.Framework.Helper
{
    public class ConfigHelper
    {
        private static volatile ConfigHelper instance = null;
        // Lock对象，线程安全所用
        private static object syncRoot = new Object();
        private ConfigHelper()
        {

        }

        public static ConfigHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ConfigHelper();
                            string serverPath = ConfigurationManager.AppSettings["ConfigPath"].ToString();
                            //判断是否是绝对路径
                            if (!Path.IsPathRooted(serverPath))
                            {
                                serverPath = System.Web.HttpRuntime.AppDomainAppPath + serverPath;
                            }
                            ConfigWatcher.SetupWacher(serverPath);
                        }

                    }
                }

                return instance;
            }
        }

        public SystemConfig getSystemConfig()
        {
            SystemConfig config = CacheManager.Get<SystemConfig>("Os.SystemConfig");
            if (config != null)
            {
                return config;
            }
            string serverPath = ConfigurationManager.AppSettings["SystemConfig"].ToString();
            //判断是否是绝对路径
            if (!Path.IsPathRooted(serverPath))
            {
                serverPath = System.Web.HttpRuntime.AppDomainAppPath + serverPath;
            }
            CacheManager.Insert("Os.SystemConfig", XmlHelper.DeSerialize<SystemConfig>(serverPath));
            return CacheManager.Get<SystemConfig>("Os.SystemConfig");
        }
    }

    #region 自定义系统配置类
    [XmlType(TypeName = "config")]
    public class SystemConfig
    {
        /// <summary>
        /// 日志设置(0：关闭日志，1：文本日志，2：nosql数据库日志，3：sql数据库日志)
        /// </summary>
        [XmlElement("LogSet")]
        public int LogSet { get; set; }

        /// <summary>
        /// 数据库设置(0：两个数据库都访问，优先访问nosql数据库，1：sql数据库，2：nosql数据库)
        /// </summary>
        [XmlElement("DBSet")]
        public int DBSet { get; set; }

        /// <summary>
        /// Redis缓存设置(0：不设Redis缓存，优先访问数据库，1：设置Redis缓存，优先访问redis缓存,不存在则访问数据库) 
        /// </summary>
        [XmlElement("RedisSet")]
        public int RedisSet { get; set; }
    }
    #endregion
}
