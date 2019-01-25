using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using Os.NETCore.Helper;
using Os.NETCore.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Os.NETCore.Cache
{
    /// <summary>
    /// RedisCache缓存实现类
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        protected IDatabase _cache;

        private ConnectionMultiplexer _connection;

        private readonly string _instance;
        public RedisCacheService()
        {
            RedisCacheOptions options = new RedisCacheOptions
            {
                Configuration = ConfigurationHelper.GetAppSettings<RedisConfigs>("RedisConfigs").Redis_connectionString,
                InstanceName = ConfigurationHelper.GetAppSettings<RedisConfigs>("RedisConfigs").Redis_instanceName
            };
            _connection = ConnectionMultiplexer.Connect(options.Configuration);
            _cache = _connection.GetDatabase(0);
            _instance = options.InstanceName;
        }
        public string GetKeyForRedis(string key)
        {
            return _instance + key;
        }
        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyExists(GetKeyForRedis(key));
        }
        /// <summary>
        /// 验证缓存项是否存在(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run<bool>(() => _cache.KeyExists(GetKeyForRedis(key)));
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }
        /// <summary>
        /// 添加缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run<bool>(() => _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))));
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte);
        }
        /// <summary>
        /// 添加缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() => _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiressAbsoulte));
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn);
        }
        /// <summary>
        /// 添加缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间,Redis中无效）</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() => _cache.StringSet(GetKeyForRedis(key), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), expiresIn));
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.KeyDelete(GetKeyForRedis(key));
        }
        /// <summary>
        /// 删除缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run<bool>(() => _cache.KeyDelete(GetKeyForRedis(key)));
        }
        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            keys.ToList().ForEach(item => Remove(item));
        }
        /// <summary>
        /// 批量删除缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key集合</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            await Task.Run(() => keys.ToList().ForEach(item => Remove(item)));
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = _cache.StringGet(GetKeyForRedis(key));

            if (!value.HasValue)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var value = _cache.StringGet(GetKeyForRedis(key));

            if (!value.HasValue)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value);
        }
        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns></returns>
        public IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }
            var dict = new Dictionary<string, object>();

            keys.ToList().ForEach(item => dict.Add(item, Get(GetKeyForRedis(item))));

            return dict;
        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public bool Replace(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value);

        }
        /// <summary>
        /// 修改缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run<bool>(() =>
            {
                if (Exists(key))
                    if (!Remove(key))
                        return false;
                return Add(key, value);
            });
        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key))
                    return false;

            return Add(key, value, expiresSliding, expiressAbsoulte);
        }
        /// <summary>
        /// 修改缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run(() =>
            {
                if (Exists(key))
                    if (!Remove(key))
                        return false;
                return Add(key, value, expiresSliding, expiressAbsoulte);
            });

        }
        /// <summary>
        /// 修改缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public bool Replace(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Exists(key))
                if (!Remove(key)) return false;

            return Add(key, value, expiresIn, isSliding);
        }
        /// <summary>
        /// 修改缓存(异步)
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">新的缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public async Task<bool> ReplaceAsync(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return await Task.Run<bool>(() =>
            {
                if (Exists(key))
                    if (!Remove(key)) return false;
                return Add(key, value, expiresIn, isSliding);
            });
        }
        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
