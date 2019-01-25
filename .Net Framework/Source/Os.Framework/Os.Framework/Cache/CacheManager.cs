using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Framework.Cache
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

        private readonly static object _lock = new object();
        private static ICacheProvider provider;

        private static void LoadProvider()
        {
            if (provider == null)
            {
                lock (_lock)
                {
                    if (provider == null)
                    {
                        provider = new AspNetCacheProvider();
                    }
                }
            }
        }

        #endregion

        #region [  Insert  ]

        public static void Insert<T>(string key, T obj)
        {
            if (obj != null)
            {
                CacheItem<T> cacheItem = new CacheItem<T>(key, obj);
                LoadProvider();
                provider.Insert(key, cacheItem);
            }
        }


        public static void Insert<T>(string key, T obj, int seconds)
        {
            if (obj != null)
            {
                CacheItem<T> cacheItem = new CacheItem<T>(key, obj);
                LoadProvider();
                provider.Insert(key, cacheItem, seconds);
            }
        }

        public static void Insert<T>(string key, T obj, ECCacheDependency dep)
        {
            if (obj != null)
            {
                CacheItem<T> cacheItem = new CacheItem<T>(key, obj);
                LoadProvider();
                provider.Insert(key, cacheItem, dep);
            }
        }


        #endregion

        #region [  Remove  ]

        /// <summary>
        /// Remove all items from cache
        /// </summary>
        public static void Clear()
        {
            LoadProvider();
            provider.Clear();
        }

        /// <summary>
        /// Removes the specified Key parttern from the 
        /// </summary>
        /// <param name="pattern"></param>
        public static void RemoveByPattern(string pattern)
        {
            LoadProvider();
            provider.RemoveByPattern(pattern);
        }

        /// <summary>
        /// Removes the specified Key from the cache
        /// </summary>
        /// <param name="Key"></param>
        public static void Remove(string key)
        {
            LoadProvider();
            provider.Remove(key);
        }

        #endregion

        #region  [  Retrieves  ]

        public static bool Contains(string key)
        {
            LoadProvider();
            return provider.Contains(key);
        }


        /// <summary>
        /// Retrieves the specified item from the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            LoadProvider();
            CacheItem<T> cacheItem = provider.Get(key) as CacheItem<T>;
            if (cacheItem != null)
            {
                return cacheItem.Object;
            }

            return default(T);
        }

        /// <summary>
        /// Get all current cache items
        /// </summary>
        /// <returns></returns>
        public static List<ICacheItem> GetCurrentCacheItems()
        {
            LoadProvider();
            return provider.GetCurrentCacheItems();
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
