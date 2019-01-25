using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Os.Framework.Helper
{
    public class XmlHelper
    {
        public XmlHelper() { }

        public static T DeSerialize<T>(string XmlPath)
        {
            try
            {
                if (!File.Exists(XmlPath))
                    throw new ArgumentNullException(XmlPath + " not Exists");

                using (StreamReader reader = new System.IO.StreamReader(XmlPath))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    T ret = (T)xs.Deserialize(reader);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        /// <summary>
        /// 创建一个带有根节点的Xml文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="RootName">根节点名称</param>
        /// <param name="Encode">编码方式:gb2312，UTF-8等常见的</param>
        /// <returns></returns>
        public static bool CreateXmlDocument(string FilePath, string RootName, string Encode)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = doc.CreateXmlDeclaration("1.0", Encode, null);
                doc.AppendChild(xmldecl);
                XmlElement xmlelem = doc.CreateElement("", RootName, "");
                doc.AppendChild(xmlelem);
                doc.Save(FilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 得到节点集合
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodeList(string path, string node)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList xmlNodeList = doc.SelectNodes(node);
            return xmlNodeList;
        }

        /// <summary>
        /// 得到节点
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <returns></returns>
        public static XmlNode GetXmlNode(string path, string node)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode xmlNode = doc.SelectSingleNode(node);
            return xmlNode;
        }

    }

}
