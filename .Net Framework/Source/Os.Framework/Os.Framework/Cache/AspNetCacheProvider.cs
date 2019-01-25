using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Os.Framework.Cache
{
    public class AspNetCacheProvider : ICacheProvider
    {
        #region [  Fileds  ]

        private int defaultExpireSeconds = 60;
        private string keyPreFix = "AspNetCacheProvider_";
        private readonly string name = "Os.Framework.Caching.AspNetCacheProvider";
        private int maximumElementsInCache = 1000;
        private int numberToRemoveWhenScavenging = 500;

        #endregion

        #region [  Constructors  ]

        public AspNetCacheProvider() { }

        public AspNetCacheProvider(string keyPreFix, int defaultExpireSeconds, int maximumElementsInCache
            , int numberToRemoveWhenScavenging)
        {
            this.keyPreFix = keyPreFix;
            this.defaultExpireSeconds = defaultExpireSeconds;
            this.maximumElementsInCache = maximumElementsInCache;
            this.numberToRemoveWhenScavenging = numberToRemoveWhenScavenging;
        }

        #endregion

        #region [  ICacheProvider Members  ]

        public int DefaultExpireSeconds
        {
            get
            {
                return defaultExpireSeconds;
            }
        }

        public string KeyPreFix
        {
            get
            {
                return keyPreFix;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int MaximumElementsInCache
        {
            get
            {
                return maximumElementsInCache;
            }
        }

        public int NumberToRemoveWhenScavenging
        {
            get
            {
                if (numberToRemoveWhenScavenging >= MaximumElementsInCache)
                {
                    numberToRemoveWhenScavenging = MaximumElementsInCache;
                }
                return numberToRemoveWhenScavenging;
            }
        }

        /// <summary>
        /// Inserts an item into the System.Web.Caching.Cache object with a cache key
        /// to reference its location, using default values provided by the System.Web.Caching.CacheItemPriority
        /// enumeration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Insert(string key, object obj)
        {
            Insert(key, obj, true);
        }

        public void Insert(string key, object obj, bool defaultExpire)
        {
            if (defaultExpire)
            {
                Insert(key, obj, defaultExpireSeconds);
            }
            else
            {
                key = MakeUpInnerKey(key);
                CheckPerformance();
                HttpRuntime.Cache.Insert(key, obj);
            }
        }

        public void Insert(string key, object obj, int seconds)
        {
            key = MakeUpInnerKey(key);
            CheckPerformance();
            HttpRuntime.Cache.Insert(key, obj, null, DateTime.Now.AddSeconds(1D * seconds)
                    , TimeSpan.Zero, CacheItemPriority.Normal, null);
        }

        public void Insert(string key, object obj, ECCacheDependency dep)
        {
            key = MakeUpInnerKey(key);
            CheckPerformance();
            CacheDependency aspnetDependency = null;
            if (dep != null)
            {
                if (dep.FileNames.Length == 1)
                {
                    aspnetDependency = new CacheDependency(dep.FileNames[0]);
                }
                else if (dep.FileNames.Length > 1)
                {
                    aspnetDependency = new CacheDependency(dep.FileNames);
                }
            }

            HttpRuntime.Cache.Insert(key, obj, aspnetDependency);
        }

        public object Get(string key)
        {
            key = MakeUpInnerKey(key);
            return HttpRuntime.Cache.Get(key);
        }

        public bool Contains(string key)
        {
            key = MakeUpInnerKey(key);
            IDictionaryEnumerator cacheEnumerator = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnumerator.MoveNext())
            {
                ICacheItem cacheItem = HttpRuntime.Cache[cacheEnumerator.Key.ToString()] as ICacheItem;

                if (cacheItem == null)
                {
                    continue;
                }

                if (MakeUpInnerKey(cacheItem.Key) == key)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ICacheItem> GetCurrentCacheItems()
        {
            IDictionaryEnumerator cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            List<ICacheItem> list = new List<ICacheItem>();

            while (cacheEnumerator.MoveNext())
            {
                ICacheItem cacheItem = HttpRuntime.Cache[cacheEnumerator.Key.ToString()] as ICacheItem;

                if (cacheItem != null)
                {
                    list.Add(cacheItem);
                }
            }

            return list;
        }

        public void Clear()
        {
            IDictionaryEnumerator cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            List<string> keyList = new List<string>();

            while (cacheEnumerator.MoveNext())
            {
                keyList.Add(cacheEnumerator.Key.ToString());
            }

            foreach (string key in keyList)
            {
                HttpRuntime.Cache.Remove(MakeUpInnerKey(key));
            }
        }

        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(MakeUpInnerKey(key));
        }

        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return;
            }

            if (pattern.StartsWith("^"))
            {
                pattern = "^" + string.Concat(keyPreFix, pattern.Substring(1));
            }
            else
            {
                pattern = string.Concat(keyPreFix, pattern);
            }

            IDictionaryEnumerator cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            List<string> keyList = new List<string>();

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            while (cacheEnumerator.MoveNext())
            {
                string cacheKey = cacheEnumerator.Key.ToString();
                if (regex.IsMatch(cacheKey))
                {
                    keyList.Add(cacheKey);
                }
            }

            foreach (string key in keyList)
            {
                HttpRuntime.Cache.Remove(MakeUpInnerKey(key));
            }
        }

        public void CheckPerformance()
        {
            if (HttpRuntime.Cache.Count >= MaximumElementsInCache)
            {
                List<ICacheItem> list = GetCurrentCacheItems();
                list.Sort();

                List<string> keyList = new List<string>();

                for (int index = 0; index < NumberToRemoveWhenScavenging; index++)
                {
                    ICacheItem item = list[index];
                    if (item != null)
                    {
                        keyList.Add(item.Key);
                    }
                }

                foreach (string key in keyList)
                {
                    HttpRuntime.Cache.Remove(MakeUpInnerKey(key));
                }
            }
        }

        #endregion

        #region [  Helpers  ]

        private string MakeUpInnerKey(string key)
        {
            return string.Concat(keyPreFix, key);
        }

        #endregion



    }
}
