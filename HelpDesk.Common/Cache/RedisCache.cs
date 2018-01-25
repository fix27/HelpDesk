using ServiceStack.Redis;
using System;

namespace HelpDesk.Common.Cache
{
    public class RedisCache : ICache
    {
        private readonly IRedisClientsManager clientsManager;
        public RedisCache(IRedisClientsManager clientsManager)
        {
            this.clientsManager = clientsManager;
        }
        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0)
        {
            TimeSpan expiresAt = TimeSpan.FromTicks(DateTime.Now.AddSeconds(0).Ticks);
            using (IRedisClient redisClient = clientsManager.GetClient())
            {
                var obj = redisClient.Get<T>(key);
                if (obj != null)
                    return obj;
                else
                {
                    obj = valueFactory();
                    if (obj != null)
                        redisClient.Set<T>(key, obj, expiresAt);
                    return obj;
                }
            }
        }

        public void Remove(string key)
        {
            using (IRedisClient redisClient = clientsManager.GetClient())
            {
                redisClient.Remove(key);
            }
        }

    }
}
