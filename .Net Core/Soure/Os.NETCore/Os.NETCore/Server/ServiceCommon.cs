
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Os.NETCore.Helper;
using Os.NETCore.Model;

namespace Os.NETCore.Server
{
    public class ServiceCommon
    { 
        public static Dictionary<string, Service> InitServiceList()
        {
            Dictionary<string, Server> servers = new Dictionary<string, Server>();
            Dictionary<string, Service> services = new Dictionary<string, Service>();
            Configs configs = ConfigurationHelper.GetAppSettings<Configs>("Configs");
            string serverPath = configs.Services.ServerConfig;
            string servicePath = configs.Services.ServiceConfig;
            //判断是否是绝对路径
            if (!Path.IsPathRooted(serverPath))
            {
                serverPath = Directory.GetCurrentDirectory() + serverPath;
                
            }
            //获取服务器列表
            Servers serverList = XmlHelper.DeSerialize<Servers>(serverPath);
            foreach (var server in serverList.List)
            {
                servers.Add(server.Name, server);
            }

            //判断是否是绝对路径
            if (!Path.IsPathRooted(servicePath))
            {
                servicePath = Directory.GetCurrentDirectory() + servicePath;
            }
            //获取服务列表
            Services serviceList =
                XmlHelper.DeSerialize<Services>(servicePath);
            foreach (var service in serviceList.List)
            {
                if (service.IsRelative && servers.Keys.Contains(service.Server))
                {
                    service.Url = servers[service.Server].Url + service.Url;
                }
                services.Add(service.Name, service);
            }
            return services;
        }

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Server> InitServerList()
        {
            Dictionary<string, Server> servers = new Dictionary<string, Server>();
            Servers serverList =
               XmlHelper.DeSerialize<Servers>(ConfigurationHelper.GetAppSettings<Configs>("Configs").Services.ServerConfig);
            foreach (var server in serverList.List)
            {
                servers.Add(server.Name, server);
            }
            return servers;
        }



    }
}
