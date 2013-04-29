using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Loachs.Common
{

    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Texts to HTML.
        /// </summary>
        /// <param name="txtStr">The TXT STR.</param>
        /// <returns>The formated str.</returns>
        public static string TextToHtml(string content)
        {
            StringBuilder sb = new StringBuilder(content);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\"", "&quot;");
            //     sb.Replace("\'", "&#39;");
            sb.Replace(" ", "&nbsp;");
            sb.Replace("\t", "&nbsp;&nbsp;");
            sb.Replace("\r", "");
            sb.Replace("\n", "<br />");
            return sb.ToString();
            //  return ShitEncode(sb.ToString());

            //return content.Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").
            //    Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        /// <summary>
        /// html to text
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlToText(string content)
        {
            StringBuilder sb = new StringBuilder(content);
            sb.Replace("<br />", "\n");
            sb.Replace("<br/>", "\n");
            //  sb.Replace("\r", "");
            sb.Replace("&nbsp;&nbsp;", "\t");
            sb.Replace("&nbsp;", " ");
            sb.Replace("&#39;", "\'");
            sb.Replace("&quot;", "\"");
            sb.Replace("&gt;", ">");
            sb.Replace("&lt;", "<");
            sb.Replace("&amp;", "&");


            return sb.ToString();
        }

        /// <summary>
        /// HtmlEncode
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlEncode(string content)
        {
            return HttpContext.Current.Server.HtmlEncode(content);
        }

        /// <summary>
        ///  HtmlDecode
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlDecode(string content)
        {
            return HttpContext.Current.Server.HtmlDecode(content);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
 

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetLength(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 编码成 sql 文本可以接受的格式
        /// </summary>
        public static string SqlEncode(string s)
        {
            if (null == s || 0 == s.Length)
            {
                return string.Empty;
            }

            return s.Trim().Replace("'", "''");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// 没有返回true
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
 

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串(过时)
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            return CutString(str, startIndex, length, string.Empty);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int length)
        {
            return CutString(str, 0, length, string.Empty);
        }

        /// <summary>
        /// 截取字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string CutString(string str, int length, string def)
        {
            return CutString(str, 0, length, def);
        }

        public static string CutString(string str, int startIndex, int length, string def)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }


                if (startIndex > str.Length)
                {
                    return "";
                }


            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            if (str.Length - startIndex <= length)
            {
                length = str.Length - startIndex;
                def = string.Empty;
            }

            try
            {
                return str.Substring(startIndex, length) + def;
            }
            catch
            {
                return str + def;
            }
        }


        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string Content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(Content, regexstr, string.Empty, RegexOptions.IgnoreCase).Trim();
        }

        /// <summary>
        ///  判断字符串是否合法的日期格式
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsData(string value)
        {
            try
            {
                System.DateTime.Parse(value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断给定的字符串(strInt)是否是数值型
        /// </summary>
        /// <param name="strInt">要确认的字符串</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsInt(string strInt)
        {
            if (strInt == null || strInt == "")
            {
                return false;
            }
            //return new Regex(@"^([0-9])[0-9]*(\.\w*)?$").IsMatch(strInt);	//整数和小数
            return new Regex(@"^(0|[1-9]\d*)$").IsMatch(strInt);	//正整数
        }

        /// <summary>
        /// 是否为httpUrl地址
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static bool IsHttpUrl(string httpUrl)
        {
            //return Regex.IsMatch(WebUrl, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            //    return Regex.IsMatch(WebUrl, @"http://");

            return httpUrl.IndexOf("http://") != -1;
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// string型转换为int型,转换失败返回缺省值
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="def">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int def)
        {
            if (IsInt(str))
            {
                return int.Parse(str);
            }
            else
            {
                return def;
            }
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");

        }
        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetFileName(string url)
        {
            //	HttpContext.Current.Response.Write( url.IndexOf("?"));

            if (url == null)
            {
                return "";
            }
            //是否有参数
            if (url.IndexOf("?") != -1)
            {
                //去掉参数
                string noquery = url.Substring(0, url.IndexOf("?"));

                //根据/分组
                string[] filenames = noquery.Split(new char[] { '/' });

                //文件名
                string filename = filenames[filenames.Length - 1];

                return filename;
            }
            else
            {
                return System.IO.Path.GetFileName(url);
            }

            //以前的
            //			if (url == null)
            //			{
            //				return "";
            //			}
            //			string[] strs1 = url.Split(new char[]{'/'});
            //			return strs1[strs1.Length - 1].Split(new char[]{'?'})[0];
        }


        ///// <summary>
        ///// 生成随机码
        ///// </summary>
        ///// <param name="length">长度</param>
        ///// <param name="isLower">是否转化成小写</param>
        ///// <returns></returns>
        //public static string RandCode(int length, bool isLower)
        //{
        //    char[] arrChar = new char[]{
        //   'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
        //   '0','1','2','3','4','5','6','7','8','9',
        //   'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
        //  };

        //    StringBuilder num = new StringBuilder();

        //    Random rnd = new Random(DateTime.Now.Millisecond);
        //    for (int i = 0; i < length; i++)
        //    {
        //        num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

        //    }
        //    if (isLower)
        //    {
        //        return num.ToString().ToLower();
        //    }
        //    return num.ToString();
        //}

        /// <summary>
        /// 将时间换成中文
        /// </summary>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public static string DateToChineseString(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now - datetime;
            //    System.Web.HttpContext.Current.Response.Write(ts.TotalDays);
            if ((int)ts.TotalDays >= 365)
            {
                return (int)ts.TotalDays / 365 + "年前";
            }
            if ((int)ts.TotalDays >= 30 && ts.TotalDays <= 365)
            {
                return (int)ts.TotalDays / 30 + "月前";
            }
            if ((int)ts.TotalDays == 1)
            {
                return "昨天";
            }
            if ((int)ts.TotalDays == 2)
            {
                return "前天";
            }
            if ((int)ts.TotalDays >= 3 && ts.TotalDays <= 30)
            {
                return (int)ts.TotalDays + "天前";
            }
            if ((int)ts.TotalDays == 0)
            {
                if ((int)ts.TotalHours != 0)
                {
                    return (int)ts.TotalHours + "小时前";
                }
                else
                {
                    if ((int)ts.TotalMinutes == 0)
                    {
                        return "1分钟前";
                    }
                    else
                    {
                        return (int)ts.TotalMinutes + "分钟前";
                    }
                }
            }
            return datetime.ToString("yyyy年MM月dd日 HH:mm");
        }

 

        public static int ObjectToInt(object expression)
        {
            return ObjectToInt(expression, 0);
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static int ObjectToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }
    }
}
