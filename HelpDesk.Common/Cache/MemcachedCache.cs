using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Runtime.Caching;

namespace HelpDesk.Common.Cache
{
    public class MemcachedCache : ICache
    {
        
        public MemcachedCache()
        {
            
        }
        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0)
        {
            using (MemcachedClient client = new MemcachedClient())
            {
                T obj = client.Get<T>(key);
                if (obj != null)
                    return obj;

                obj = valueFactory();

                if (expirationSeconds > 0)
                    client.Store(StoreMode.Set, key, obj, DateTime.Now.AddSeconds(expirationSeconds));
                else
                    client.Store(StoreMode.Set, key, obj);

                return obj;
            }
        }

        public void Remove(string key)
        {
            using (MemcachedClient client = new MemcachedClient())
            {
                client.Remove(key);
            }
        }

    }
}
