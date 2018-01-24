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
        public T AddOrGetExisting<T>(string key, Func<T> valueFactory)
        {
            var newValue = new Lazy<T>(valueFactory);
            // the line belows returns existing item or adds the new value if it doesn't exist
            var value = (Lazy<T>)cache.AddOrGetExisting(key, newValue, MemoryCache.InfiniteAbsoluteExpiration);
            return (value ?? newValue).Value; // Lazy<T> handles the locking itself
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

    }
}
