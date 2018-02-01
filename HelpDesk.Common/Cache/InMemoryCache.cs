using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Кэш в памяти
    /// </summary>
    public class InMemoryCache: BaseCache, ICache
    {
        private readonly ObjectCache cache;

        public InMemoryCache(int defaultExpirationSeconds) : base(defaultExpirationSeconds)
        {
            cache = MemoryCache.Default;
        }
        
        public object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (expirationSeconds == 0)
                policy.AbsoluteExpiration = MemoryCache.InfiniteAbsoluteExpiration;
            else
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationSeconds > 0 ? expirationSeconds : defaultExpirationSeconds);

            var newValue = new Lazy<object>(valueFactory);
            var value = (Lazy<object>)cache.AddOrGetExisting(key, newValue, policy);
            return (value ?? newValue).Value;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

    }
}
