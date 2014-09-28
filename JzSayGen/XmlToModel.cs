using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace JzSayGen
{
    /// <summary>
    /// xml转实体
    /// </summary>
    public static class XmlToModel
    {
        /// <summary>
        /// 解析节点列表
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="xpath">查询节点 /root/friends/friend</param>
        /// <returns></returns>
        public static List<T> ParseNodeList<T>(string xmlStr, string xpath) where T : class, new()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            return ParseNodeList<T>(doc, xpath);
        }

        /// <summary>
        /// 解析节点列表
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="doc">xml对象</param>
        /// <param name="xpath">查询节点 /root/friends/friend</param>
        /// <returns></returns>
        public static List<T> ParseNodeList<T>(XmlDocument doc, string xpath) where T : class, new()
        {
            XmlNodeList nodes = doc.SelectNodes(xpath);
            return ParseNodeList<T>(nodes);
        }

        /// <summary>
        /// 解析节点列表
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="nodes">节点数据</param>
        /// <returns></returns>
        public static List<T> ParseNodeList<T>(XmlNodeList nodes) where T : class, new()
        {
            List<T> lt = new List<T>();
            foreach (XmlNode node in nodes)
            {
                lt.Add(ParseNode<T>(node));
            }
            return lt;
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="xpath">查询节点 /root/user</param>
        /// <param name="attributes">属性列表</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseNode(string xmlStr, string xpath, List<string> attributes)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            return ParseNode(doc, xpath, attributes);
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <param name="doc">xml对象</param>
        /// <param name="xpath">查询节点 /root/user</param>
        /// <param name="attributes">属性列表</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseNode(XmlDocument doc, string xpath, List<string> attributes)
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            return ParseNode(node, attributes);
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <param name="node">节点数据</param>
        /// <param name="attributes">属性列表</param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseNode(XmlNode node, List<string> attributes)
        {
            Dictionary<string, string> nodeAtt = GetNodeAttributesIgnoreCaseMap(node);
            Dictionary<string, string> dr = new Dictionary<string, string>();
            foreach (string a in attributes)
            {
                dr.Add(a, nodeAtt.ContainsKey(a.ToLower()) ? node.Attributes[nodeAtt[a.ToLower()]].Value : string.Empty);
            }
            return dr;
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="xpath">查询节点 /root/user</param>
        /// <returns></returns>
        public static T ParseNode<T>(string xmlStr, string xpath) where T : class, new()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlStr);
            return ParseNode<T>(doc, xpath);
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="doc">xml对象</param>
        /// <param name="xpath">查询节点 /root/user</param>
        /// <returns></returns>
        public static T ParseNode<T>(XmlDocument doc, string xpath) where T : class, new()
        {
            XmlNode node = doc.SelectSingleNode(xpath);
            return ParseNode<T>(node);
        }

        /// <summary>
        /// 解析节点属性
        /// </summary>
        /// <typeparam name="T">返回的实体类型</typeparam>
        /// <param name="node">节点数据</param>
        /// <returns></returns>
        public static T ParseNode<T>(XmlNode node) where T : class, new()
        {
            Dictionary<string, string> nodeAtt = GetNodeAttributesIgnoreCaseMap(node);
            T model = new T();
            var ps = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            for (int i = 0, j = ps.Length; i < j; i++)
            {
                if (nodeAtt.ContainsKey(ps[i].Name.ToLower()))
                {
                    var v = node.Attributes[nodeAtt[ps[i].Name.ToLower()]].Value;
                    ps[i].SetValue(model, ObjectExten.Convert(v, ps[i].PropertyType), null);
                }
            }
            return model;
        }

        /// <summary>
        /// 获取节点的属性小写和原属性的映射关系
        /// </summary>
        /// <param name="node">节点数据</param>
        /// <returns></returns>
        static Dictionary<string, string> GetNodeAttributesIgnoreCaseMap(XmlNode node)
        {
            Dictionary<string, string> nodeAtt = new Dictionary<string, string>();
            for (int i = 0, j = node.Attributes.Count; i < j; i++)
            {
                nodeAtt.Add(node.Attributes[i].Name.ToLower(), node.Attributes[i].Name);
            }
            return nodeAtt;
        }

    }
}
