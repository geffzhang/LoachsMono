using System;
using System.Collections.Generic;
using System.Text;

namespace Loachs.Entity
{

    /// <summary>
    /// 分类类别
    /// </summary>
    public enum TermType
    {
        Category = 1,
        Tag = 2,
        //Catalog = 1,
        //Keyword = 4,
        //Tags = 2,
        Unknown = 0

    }

    /// <summary>
    /// 连接类型(后续版本用)
    /// </summary>
    public enum LinkType
    {
        Unknow = 0,

        /// <summary>
        /// 系统
        /// </summary>
        System = 1,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 2,
    }


    /// <summary>
    /// 连接位置
    /// </summary>
    public enum LinkPosition
    {
        Unknow = 0,

        /// <summary>
        /// 导航
        /// </summary>
        Navigation = 1,

        /// <summary>
        /// 普通
        /// </summary>
        General = 2,
    }

 

    /// <summary>
    /// 内容状态
    /// </summary>
    public enum PostStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 0,

        /// <summary>
        /// 发布
        /// </summary>
        Published = 1,


        //Top,
        //Hide,
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ApprovedStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        Wait = 0,

        /// <summary>
        /// 已通过
        /// </summary>
        Success = 1,

    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Administrator = 1,

        /// <summary>
        /// 写作者
        /// </summary>
        Author = 5,
    }

    /// <summary>
    /// 文章URL格式
    /// </summary>
    public enum PostUrlFormat 
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 日期
        /// </summary>
        Date = 1,

        /// <summary>
        /// 别名
        /// </summary>
        Slug = 2,

        ///// <summary>
        ///// 分类
        ///// </summary>
        //Category = 3,
    }
}
