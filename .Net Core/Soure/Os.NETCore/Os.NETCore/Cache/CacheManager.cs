using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Cache
{
    public sealed class CacheManager
    {
        #region [  Fileds  ]

        /// <summary>
        /// Total seconds of one day.
        /// </summary>
        public static readonly int DaySenonds = 86400;
        /// <summary>
        /// Total seconds of one hour.
        /// </summary>
        public static readonly int HourSeconds = 3600;
        /// <summary>
        /// Total seconds of one minute.
        /// </summary>
        public static readonly int MinuteSeconds = 60;

        #endregion

        #region [  Constructors  ]

        private CacheManager() { }

        #endregion

        #region [  LoadCacheProvider  ] 
        #endregion

        #region [  Insert  ]

        public static void Insert<T>(string key, T obj)
        {
            if (obj != null)
            {
                T cacheItem = obj;
                CacheServiceProvider.Instance.Add(key, cacheItem);
            }
        }


        public static void Insert<T>(string key, T obj, int seconds)
        {
            if (obj != null)
            {
                T cacheItem =obj; 
                CacheServiceProvider.Instance.Add(key, cacheItem,new TimeSpan(1,0,0,0),false);
            }
        }


        #endregion

        #region [  Remove  ]
        

        /// <summary>
        /// Removes the specified Key from the cache
        /// </summary>
        /// <param name="Key"></param>
        public static void Remove(string key)
        {
            CacheServiceProvider.Instance.Remove(key);
        }

        #endregion

        #region  [  Retrieves  ]

        public static bool Contains(string key)
        { 
            return CacheServiceProvider.Instance.Exists(key);
        }


        /// <summary>
        /// Retrieves the specified item from the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)where T : class
        {
            T cacheItem = CacheServiceProvider.Instance.Get<T>(key);
            if (cacheItem != null)
            {
                return cacheItem;
            }

            return default(T);
        }
         

        #endregion

        #region [  Helpers  ]

        public static int CalculateSecoundsByMinutes(int minutes)
        {
            return minutes * MinuteSeconds;
        }

        public static int CalculateSecoundsByHours(int hours)
        {
            return hours * HourSeconds;
        }

        public static int CalculateSecoundsByDays(int days)
        {
            return days * DaySenonds;
        }

        #endregion
    }
}
