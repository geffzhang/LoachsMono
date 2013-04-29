using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    /// 统计接口
    /// </summary>
    public interface ISetting
    {
        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="statistics"></param>
        /// <returns></returns>
        bool UpdateSetting(SettingInfo setting);

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        SettingInfo GetSetting();
    }
}
