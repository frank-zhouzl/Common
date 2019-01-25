using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Os.Framework.Server
{
    [XmlType(TypeName = "Config")]
    public class Servers
    {
        [XmlArray("Servers")]
        public List<Server> List { get; set; }
    }

    [XmlType(TypeName = "Server")]
    public class Server
    {
        /// <summary>
        /// 服务名称(本地自定义名称)
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        /// <summary>
        /// 服务域名
        /// </summary>
        [XmlAttribute]
        public string Url { get; set; }
    }
}
