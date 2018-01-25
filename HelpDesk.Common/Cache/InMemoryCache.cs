using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    public class InMemoryCache: ICache
    {
        private readonly ObjectCache cache;

        public InMemoryCache()
        {
            cache = MemoryCache.Default;
        }
        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (expirationSeconds == 0)
                policy.AbsoluteExpiration = MemoryCache.InfiniteAbsoluteExpiration;
            else
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationSeconds);
                                    
            var newValue = new Lazy<T>(valueFactory);
            // the line belows returns existing item or adds the new value if it doesn't exist
            var value = (Lazy<T>)cache.AddOrGetExisting(key, newValue, policy);
            return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        }

        public object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0)
        {
            throw new NotSupportedException();
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

    }
}
