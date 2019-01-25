using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Os.Framework.Server
{
    [XmlType(TypeName = "Config")]
    public class Services
    {
        [XmlArray("Services")]
        public List<Service> List { get; set; }
    }

    [XmlType(TypeName = "Service")]
    public class Service
    {
        /// <summary>
        /// 本地重命名接口名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        [XmlAttribute]
        public string Url { get; set; }
        /// <summary>
        /// 服务名称(本地自定义名称)
        /// </summary>
        [XmlAttribute]
        public string Server { get; set; }
        /// <summary>
        /// 接口请求类型
        /// </summary>
        [XmlAttribute]
        public string Method { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [XmlAttribute]
        public bool IsRelative { get; set; }
    }
}
