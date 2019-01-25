using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.Framework.Cache
{
    interface ICacheProvider
    {
        int DefaultExpireSeconds { get; }

        string KeyPreFix { get; }

        string Name { get; }

        int MaximumElementsInCache { get; }

        int NumberToRemoveWhenScavenging { get; }

        void Insert(string key, object obj);

        void Insert(string key, object obj, bool defaultExpire);

        void Insert(string key, object obj, int seconds);

        void Insert(string key, object obj, ECCacheDependency dep);

        object Get(string key);

        bool Contains(string key);

        List<ICacheItem> GetCurrentCacheItems();

        void Clear();

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void CheckPerformance();
    }
}
