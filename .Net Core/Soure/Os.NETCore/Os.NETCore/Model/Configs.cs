using System;
using System.Collections.Generic;
using System.Text;

namespace Os.NETCore.Model
{
    public class Configs
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        public Services Services { get; set; }
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigPath { get; set; }
        /// <summary>
        /// 系统配置
        /// </summary>
        public string SystemConfig { get; set; }
    }
    public class Services
    {
        public string ServerConfig { get; set; }
        public string ServiceConfig { get; set; }
    }
}
