using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Os.NETCore.Helper
{
    /// <summary>
    /// 自定义封装的一个读取配置文件节点类
    /// </summary>
    public class ConfigurationHelper
    {
        /// <summary>
        /// 读取的节点名称
        /// </summary>
        /// <typeparam name="T">映射的对象类型</typeparam>
        /// <param name="key">节点名称</param>
        /// <returns></returns>
        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            //获取应用程序的当前目录
            string BaseDirectory = GetBaseDirectory();
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(BaseDirectory)
                .Add(new JsonConfigurationSource { Path = "appsettings.json", Optional = false, ReloadOnChange = true })
                .Build();
            var appconfig = new ServiceCollection().Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return appconfig;
        }
        /// <summary>
        /// 获取网站根目录
        /// </summary>
        /// <returns></returns>
        public static string GetBaseDirectory()
        {
            string rootdir = AppContext.BaseDirectory;
            DirectoryInfo Dir = Directory.GetParent(rootdir);
            //路径这里获取方式需修改
            string BaseDirectory = Dir.Parent.Parent.Parent.FullName;
            return BaseDirectory;
        }
    }
}
