using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Loachs.Business;

namespace Loachs.Common
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    /// <remarks>
    /// date:2012.7.5
    /// </remarks>
    public class NetHelper
    {
        /// <summary>
        /// 获取某地址的状态码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpStatusCode GetHttpStatusCode(string url)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode;
                }
            }

            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }

        }

      
    }
}
