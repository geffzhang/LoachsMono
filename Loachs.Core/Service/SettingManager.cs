using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using System.IO;
using System.Xml.Serialization;


using Loachs.Data;
using Loachs.Entity;
using Loachs.Common;

namespace Loachs.Business
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public class SettingManager
    {
        static ISetting dao = DataAccess.CreateSetting();

        /// <summary>
        /// 静态变量
        /// </summary>
        private static SettingInfo _setting;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static SettingManager()
        {
            LoadSetting();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadSetting()
        {
            if (_setting == null)
            {
                lock (lockHelper)
                {
                    if (_setting == null)
                    {
                        _setting = dao.GetSetting();
                    }
                }
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static SettingInfo GetSetting()
        {
            return _setting;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static bool UpdateSetting()
        {
            bool result = dao.UpdateSetting(_setting);
            return result;
        }
    }
}
