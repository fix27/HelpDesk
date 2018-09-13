using HelpDesk.Common.Cache.Interface;
using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Кэш в памяти
    /// </summary>
    public class MemoryCache: IMemoryCache
    {
        private readonly ObjectCache _cache;

        public MemoryCache()
        {
            _cache = System.Runtime.Caching.MemoryCache.Default;
        }

        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (expirationSeconds == 0)
                policy.AbsoluteExpiration = System.Runtime.Caching.MemoryCache.InfiniteAbsoluteExpiration;
            else
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationSeconds);


            var value = (T)_cache.Get(key);

            if (value == null)
            {
                value = valueFactory();
                _cache.Add(key, value, policy);
                return value;
            }

            return value;

        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

    }
}
