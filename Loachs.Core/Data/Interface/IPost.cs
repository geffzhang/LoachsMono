using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Entity;

namespace Loachs.Data
{
    /// <summary>
    /// 内容接口
    /// </summary>
    public interface IPost
    {
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="_postinfo"></param>
        /// <returns></returns>
        int InsertPost(PostInfo _postinfo);

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="_postinfo"></param>
        /// <returns></returns>
        int UpdatePost(PostInfo _postinfo);

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        int DeletePost(int postid);

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        PostInfo GetPost(int postid);

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        PostInfo GetPost(string slug);

        ///// <summary>
        ///// 更新文章评论数
        ///// </summary>
        ///// <param name="postID"></param>
        ///// <param name="commentNum"></param>
        ///// <returns></returns>
        //bool UpdatePostComment(int postID, int commentNum);


        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        List<PostInfo> GetPostList();

        
       
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="topstatus"></param>
        /// <param name="begindate"></param>
        /// <param name="enddate"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount,  int categoryId,int tagId, int userId,int recommend, int status, int topstatus,int hidestatus,string begindate,string enddate, string keyword);

        /// <summary>
        /// 获取相关文章
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        List<PostInfo> GetPostListByRelated(int postId, int rowCount);

        /// <summary>
        /// 获取归档
        /// </summary>
        /// <returns></returns>
        List<ArchiveInfo> GetArchive();

     
        /// <summary>
        /// 更新文章浏览量
        /// </summary>
        /// <param name="postId">文章Id</param>
        /// <param name="addCount">增量</param>
        /// <returns></returns>
        int UpdatePostViewCount(int postId, int addCount);

 

        ///// <summary>
        ///// 根据标签ID获取文章列表
        ///// </summary>
        ///// <param name="tagID"></param>
        ///// <returns></returns>
        //List<PostInfo> GetPostListByTagID(int tagID);

        ///// <summary>
        ///// 根据文章ID获取相关文章列表
        ///// </summary>
        ///// <param name="postID"></param>
        ///// <param name="num"></param>
        ///// <returns></returns>
        //List<PostInfo> GetPostListByPostID(int postID, int num);

        ///// <summary>
        ///// 获取置顶文章
        ///// </summary>
        ///// <returns></returns>
        //List<PostInfo> GetPostListByTop();

        ///// <summary>
        ///// 获取分类文章列表
        ///// </summary>
        ///// <param name="categoryID">分类ID,-1为全部</param>
        ///// <param name="num">条数</param>
        ///// <returns></returns>
        //List<PostInfo> GetPostListByCategoryID(int categoryID, int num);

        ///// <summary>
        ///// 是否存在别名
        ///// </summary>
        ///// <param name="customUrl"></param>
        ///// <returns></returns>
        //bool ExistsPost(string customUrl);

        ///// <summary>
        ///// 是否存在别名,修改时
        ///// </summary>
        ///// <param name="postID"></param>
        ///// <param name="customUrl"></param>
        ///// <returns></returns>
        //bool ExistsPost(int postID, string customUrl);



        ///// <summary>
        ///// 获取某个标签的使用次数
        ///// </summary>
        ///// <param name="tagID"></param>
        ///// <returns></returns>
        //int GetTagTotalCount(int tagId);


        ///// <summary>
        ///// 统计文章数
        ///// </summary>
        ///// <param name="categoryID"></param>
        ///// <returns></returns>
        //int TotalPost(int categoryID);


    }
}
