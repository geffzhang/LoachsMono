using System;
using System.Collections.Generic;
using System.Text;

namespace Loachs.Entity
{
    /// <summary>
    /// 统计实体
    /// </summary>
    public class StatisticsInfo
    {
      //  private int _siteid;
        private int _postcount;
        private int _commentcount;
        private int _visitcount;
        private int _tagcount;
    //    private int _trackbackcount;


        ///// <summary>
        ///// 
        ///// </summary>
        //public int SiteId
        //{
        //    set { _siteid = value; }
        //    get { return _siteid; }
        //}

        /// <summary>
        /// 文章数
        /// </summary>
        public int PostCount
        {
            set { _postcount = value; }
            get { return _postcount; }
        }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            set { _commentcount = value; }
            get { return _commentcount; }
        }
        /// <summary>
        /// 访问数
        /// </summary>
        public int VisitCount
        {
            set { _visitcount = value; }
            get { return _visitcount; }
        }
        /// <summary>
        /// 标签数
        /// </summary>
        public int TagCount
        {
            set { _tagcount = value; }
            get { return _tagcount; }
        }

        //public string ToXmlString()
        //{
        //    return string.Empty;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        //public int TrackbackCount
        //{
        //    set { _trackbackcount = value; }
        //    get { return _trackbackcount; }
        //}


    }
}
