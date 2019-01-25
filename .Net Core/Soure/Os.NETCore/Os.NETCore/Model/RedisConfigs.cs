using System;
using System.Collections.Generic;
using System.Text;

namespace Os.NETCore.Model
{
   /// <summary>
   /// Redis配置项
   /// </summary>
   public class RedisConfigs
    {
      public string Redis_instanceName { get; set; }
      public string Redis_connectionString { get; set; }
    }
}
