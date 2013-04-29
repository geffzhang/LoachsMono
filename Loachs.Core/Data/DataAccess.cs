using System.Reflection;
using System.Configuration;

namespace Loachs.Data
{
    public sealed class DataAccess
    {
        private static readonly string path = "Loachs.Data.Access";

        private static object lockHelper = new object();

       
        public static ITag _itag = null;
        public static ICategory _icategory = null;
        public static IPost _ipost = null;
        public static IUser _iuser = null;
        public static ILink _ilink = null;
        public static IComment _icomment = null;
        public static IStatistics _istatistics = null;
        public static ISetting _isetting = null;

        private DataAccess() { }



        public static ITag CreateTag()
        {
            string className = path + ".Tag";
         //   return (ITag)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<ITag>(_itag, className);
        }

        public static ICategory CreateCategory()
        {
            string className = path + ".Category";
         //   return (ICategory)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<ICategory>(_icategory, className);
        }

       

        public static IPost CreatePost()
        {
            string className = path + ".Post";
            //     return (IPost)Assembly.Load(path).CreateInstance(className);

            return CreateInstance<IPost>(_ipost, className);
        }



        public static IUser CreateUser()
        {
            string className = path + ".User";
          //  return (IUser)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<IUser>(_iuser, className);
        }

        public static ILink CreateLink()
        {
            string className = path + ".Link";
            //return (ILink)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<ILink>(_ilink, className);
        }

        public static IComment CreateComment()
        {
            string className = path + ".Comment";
       //     return (IComment)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<IComment>(_icomment, className);
        }

        public static IStatistics CreateStatistics()
        {
            string className = path + ".Statistics";
         //   return (IStatistics)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<IStatistics>(_istatistics, className);
        }

        public static ISetting CreateSetting()
        {
            string className = path + ".Setting";
         //   return (ISetting)Assembly.Load(path).CreateInstance(className);
            return CreateInstance<ISetting>(_isetting, className);
        }

        

        /// <summary>
        /// 实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_instance"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(T _instance, string className)
        {

            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = (T)Assembly.Load(path).CreateInstance(className);
                    }
                }
            }
            return _instance;

        }
    }

    //public class DatabaseProvider<T> //where T : new()
    //{
    //    private DatabaseProvider()
    //    { }

    //    private static T _instance = default(T);
    //    private static object lockHelper = new object();

    //    static DatabaseProvider()
    //    {

    //    }



    //    public static T Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //            {
    //                lock (lockHelper)
    //                {
    //                    if (_instance == null)
    //                    {
    //                        //  _instance = new T();
    //                        //  _instance =  (T)Assembly.Load(path).CreateInstance(className);
    //                    }
    //                }
    //            }
    //            return _instance;
    //        }
    //    }

    //}
}
