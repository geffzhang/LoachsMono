using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    /// 标签接口
    /// </summary>
    public interface ITag
    {
        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        int InsertTag(TagInfo tag);

        /// <summary>
        /// 修改标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        int UpdateTag(TagInfo tag);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        int DeleteTag(int tagId);

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        TagInfo GetTag(int tagId);

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        List<TagInfo> GetTagList();

        /// <summary>
        /// 分页获取标签
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
    //    List<TagInfo> GetTagList(int pageSize, int pageIndex, out int recordCount);

        /// <summary>
        /// 根据ID获取分类,ID用逗号隔开
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
       // List<TagInfo> GetTagList(string ids);


        /// <summary>
        /// 获取标签的文章数
        /// </summary>
        /// <param name="tagId">标签Id</param>
        /// <param name="incUncategorized">是否包括未分类文章</param>
        /// <returns></returns>
     //   int GetCount(int tagId, bool incUncategorized);

    }
}
