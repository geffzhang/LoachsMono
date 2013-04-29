/*
 * 缓存方式扩展，缓存Tag(命名空间)的实现
 * 给TagKey分组,不能分多层组
 * http://huacnlee.com/blog/group-caches-with-tag-or-namespace
 * 
 * 
 * 再论缓存删除 - 用正则匹配 CacheKey 批量删除缓存
 * http://huacnlee.com/blog/howto-remove-cache-by-regular-expressions\.
 * 
 * 
 * memcached批量删除方案探讨
 * http://it.dianping.com/memcached_item_batch_del.htm
 * 
 * 
 * 效率：tag>cache>regex>xml
 * 不需要清除的缓存越多,cache 相对效率越低
 * cache和regex 原理一样，效率接近,Key越大，cache 越快
 * cache,tag:  key<10时:tag 与cache 差距极大， key=10时:tag 快20倍以上，key=20时:tag 快10倍以上，key=50时:tag 快5倍以上，key=100时:tag 快4倍左右， key=200时:tag 快3倍左右，  key=500时:tag快一倍左右， key=1000时: tag 略快， key=5000:当有其它缓存时，tag 明显要快， key=10000 接近 ，key>20000 cache 略快
 * 单台用cache,多台用tag
 */
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;
 
namespace Loachs.Common
{
    /// <summary>
    ///缓存管理
    /// </summary>
    public class CacheByXml
    {
        /// <summary>
        /// XML 元素
        /// </summary>
        public static XmlElement objectXmlMap;

        /// <summary>
        /// 缓存键XML文档
        /// </summary>
        public static XmlDocument rootXml = new XmlDocument();

        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        private static object lockHelper = new object();

        static CacheByXml()
        {
            objectXmlMap = rootXml.CreateElement("Cache");
            //build the internal xml document.
            rootXml.AppendChild(objectXmlMap);
        }


        /// <summary>
        /// Add the object to the underlying storage and Xml mapping document
        /// </summary>
        /// <param name="xpath">the hierarchical location of the object in Xml document </param>
        /// <param name="o">the object to be cached</param>
        public static void Add(string xpath, object o)
        {
            //clear up the xpath expression
            string newXpath = PrepareXpath(xpath);
            int separator = newXpath.LastIndexOf("/");
            //find the group name
            string group = newXpath.Substring(0, separator);
            //find the item name
            string element = newXpath.Substring(separator + 1);

            XmlNode groupNode = objectXmlMap.SelectSingleNode(group);

            string objectId = "";

            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            if (node != null)
            {
                objectId = node.Attributes["objectId"].Value;
            }

            if (objectId == "")
            {
                groupNode = CreateNode(group);
                objectId = Guid.NewGuid().ToString();
                XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                objectAttribute.Value = objectId;
                objectElement.Attributes.Append(objectAttribute);
                groupNode.AppendChild(objectElement);
            }
            else
            {
                XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                objectAttribute.Value = objectId;
                objectElement.Attributes.Append(objectAttribute);
                groupNode.ReplaceChild(objectElement, node);
            }

            webCache.Insert(objectId, o);
        }

        /// <summary>
        /// Retrieve the cached object using its hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location of the object in xml document</param>
        /// <returns>cached object </returns>
        public static object Get(string xpath)
        {
            object o = null;
            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            //if the hierarchical location existed in the xml, retrieve the object
            //otherwise, return the object as null
            if (node != null)
            {
                string objectId = node.Attributes["objectId"].Value;
                //retrieve the object through cache strategy
                //  o = cs.RetrieveObject(objectId);
                o = webCache.Get(objectId);
            }
            return o;

        }



        /// <summary>
        /// Retrive a list of object under a hierarchical location
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns>an array of objects</returns>
        public static object[] GetList(string xpath)
        {
            XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
            XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
            ArrayList objects = new ArrayList();
            string objectId = null;
            //loop through each node and link the object in object[]
            //to objects stored via cache strategy
            foreach (XmlNode result in results)
            {
                objectId = result.Attributes["objectId"].Value;
                //   objects.Add(cs.RetrieveObject(objectId));
                objects.Add(webCache.Get(objectId));
            }
            //convert the ArrayList to object[]
            return (object[])objects.ToArray(typeof(System.Object));
        }

        /// <summary>
        /// Remove the object from the storage and clear the Xml assocated with
        /// the object
        /// </summary>
        /// <param name="xpath">hierarchical locatioin of the object</param>
        public static void Remove(string xpath)
        {
            lock (lockHelper)
            {
                XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
                //check if the xpath refers to a group(container) or
                //actual element for cached object
                if (result.HasChildNodes)
                {
                    //remove all the cached object in the hastable
                    //and remove all the child nodes 
                    XmlNodeList objects = result.SelectNodes("*[@objectId]");
                    string objectId = "";
                    foreach (XmlNode node in objects)
                    {
                        objectId = node.Attributes["objectId"].Value;
                        node.ParentNode.RemoveChild(node);
                        //use cache strategy to remove the objects from the 
                        //underlying storage
                        //     cs.RemoveObject(objectId);
                        webCache.Remove(objectId);
                    }
                }
                else
                {
                    //just remove the element node and the object associate with it
                    string objectId = result.Attributes["objectId"].Value;
                    result.ParentNode.RemoveChild(result);
                    //    cs.RemoveObject(objectId);

                    webCache.Remove(objectId);
                }
            }
        }

        /// <summary>
        /// CreateNode is responsible for creating the xml tree that is
        /// specificed in the hierarchical location of the object.
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private static XmlNode CreateNode(string xpath)
        {
            lock (lockHelper)
            {
                string[] xpathArray = xpath.Split('/');
                string root = "";
                XmlNode parentNode = (XmlNode)objectXmlMap;
                //loop through the array of levels and create the corresponding node for each level
                //skip the root node.
                for (int i = 1; i < xpathArray.Length; i++)
                {
                    XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
                    // if the current location doesn't exist, build one
                    //otherwise set the current locaiton to the it child location
                    if (node == null)
                    {
                        XmlElement newElement = objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
                        parentNode.AppendChild(newElement);
                    }
                    //set the new location to one level lower
                    root = root + "/" + xpathArray[i];
                    parentNode = objectXmlMap.SelectSingleNode(root);
                }
                return parentNode;
            }
        }

        /// <summary>
        /// clean up the xpath so that extra '/' is removed
        /// </summary>
        /// <param name="xpath">hierarchical location</param>
        /// <returns></returns>
        private static string PrepareXpath(string xpath)
        {

            string[] xpathArray = xpath.Split('/');
            xpath = "/Cache";
            foreach (string s in xpathArray)
            {
                if (s != "")
                {
                    xpath = xpath + "/" + s;
                }
            }
            return xpath;

        }
    }



    public class CacheByTag
    {
        private static readonly Cache _cache = HttpContext.Current.Cache;
        private static string GenerateKey(string key)
        {
            return "project_name/" + key;
        }

        /// 
        /// 取缓存
        /// 
        /// 
        /// 
        public static object Get(string key)
        {
            key = GenerateKey(key);

            object obj = _cache.Get(key);

            if (obj != null)
            {

              //  Logs.Debug("Cache hit " + key);
            }

            return obj;
        }

        /// 
        /// 存缓存
        /// 
        /// 
        /// 
        public static void Set(string key, object value)
        {
            key = GenerateKey(key);

            _cache.Insert(key, value);

          //  Logs.Debug("Cache set " + key);
        }

        /// 
        /// 存缓存，带Tag，用于做类似命名空间的管理
        /// 
        /// 
        /// 
        /// 
        public static void Set(string key, object value, string[] tags)
        {
            if (tags.Length == 0)
            {
                Set(key, value);
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = "tags/" + tags[i];
            }

            List<string> tagList = new List<string>();
            foreach (string tag in tags)
            {
                object tagObj = Get(tag);
                if (tagObj != null)
                {
                    tagList = (List<string>)tagObj;
                }

                tagList.Add(key);
                Set(tag, tagList);
            }

            Set(key, value);
        }


        /// 
        /// 删缓存
        /// 
        /// 
        public static void Remove(string key)
        {
            key = GenerateKey(key);

            _cache.Remove(key);

         //   Logs.Debug("Cache remove " + key);
        }

        /// 
        /// 根据 Tag 删除缓存
        /// 
        /// 
        public static void RemoveByTags(string[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = "tags/" + tags[i];
            }

            List<string> tagList = new List<string>();
            foreach (string tag in tags)
            {
                object tagsObj = Get(tag);
                if (tagsObj != null)
                {
                    tagList = (List<string>)tagsObj;
                    foreach (string key in tagList)
                    {
                        Remove(key);
                    }
                }

                Remove(tag);
            }
        }
    }


    public class CacheByRegex
    {
        private static readonly Cache _cache = HttpContext.Current.Cache;
        /// 
        /// 用于存放所有cache keys，以便于用正则的方式删除缓存
        /// 如： place/home/page/1 ，以后用 place/home/page/* 来删除 place/home/page/1,place/home/page/2 ... place/home/page/n
        /// 
        private static List<string> cacheKeys = new List<string>();
        /// 
        /// 将 Key 存入 cacheKeyStore
        /// 
        /// 
        private static void saveKeyToCacheKeys(string key)
        {
            if (!cacheKeys.Exists(c => c == key))
            {
                cacheKeys.Add(key);
            }
        }

        public static void Set(string key, object value)
        {

            saveKeyToCacheKeys(key);

            _cache.Insert(key, value);

            //  Logs.Debug("Cache set " + key);
        }

        /// 
        /// 删缓存
        /// 
        /// 
        /// 是否用正则匹配
        public static void Remove(string key, bool match)
        {
            if (!match)
            {
                // Memcached 虚构的自已实现一个
                _cache.Remove(key);
                // 写 Cached 操作日志
              //  Debug.Log("Cache remove " + key);
            }
            else
            {
                Regex reg = new Regex(key, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
                List<string> matchedKeys = cacheKeys.FindAll(c => reg.IsMatch(c));
                foreach (var k in matchedKeys)
                {
                    _cache.Remove(k);
                 //   Debug.Log("Cache remove " + key);
                }

            }
        }
    }

}