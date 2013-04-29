using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    /// 分类接口
    /// </summary>
    public interface ICategory
    {
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int InsertCategory(CategoryInfo category);

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int UpdateCategory(CategoryInfo category);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        int DeleteCategory(int categoryId);

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        CategoryInfo GetCategory(int categoryId);

        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        List<CategoryInfo> GetCategoryList();
 

        /// <summary>
        /// 根据ID获取分类,ID用逗号隔开
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
       // List<CategoryInfo> GetCategoryList(string ids);


        ///// <summary>
        ///// 获取分类的文章数
        ///// </summary>
        ///// <param name="categoryId">分类Id</param>
        ///// <param name="incUncategorized">是否包括未分类文章</param>
        ///// <returns></returns>
        //int GetCount(int categoryId, bool incUncategorized);

    }
}
