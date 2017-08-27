using System;
using System.Runtime.Caching;

namespace HelpDesk.Data.Cache
{
    public class InMemoryCache: IInMemoryCache
    {
        private readonly ObjectCache cache;

        public InMemoryCache()
        {
            cache = MemoryCache.Default;
        }
        public T AddOrGetExisting<T>(object key, Func<T> get)
        {
            T val = get();
            T existingVal = (T)cache.AddOrGetExisting(key.ToString(), val, new CacheItemPolicy());
            return existingVal !=null ? existingVal: val;
        }
    }
}
