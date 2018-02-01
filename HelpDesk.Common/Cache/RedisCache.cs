using Newtonsoft.Json;
using ServiceStack.Redis;
using System;

namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Redis-кэш
    /// </summary>
    public class RedisCache : BaseCache, ICache
    {
        private readonly IRedisClientsManager clientsManager;
        public RedisCache(IRedisClientsManager clientsManager, int defaultExpirationSeconds): base(defaultExpirationSeconds)
        {
            this.clientsManager = clientsManager;
        }
           
        public object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0)
        {
            TimeSpan expiresAt = new TimeSpan(0, 0, 0, expirationSeconds > 0 ? expirationSeconds : defaultExpirationSeconds);
            using (IRedisClient redisClient = clientsManager.GetClient())
            {
                object obj = redisClient.GetValue(key);
                if (obj != null)
                    return JsonConvert.DeserializeObject((string)obj, typeValue);
                else
                {
                    obj = valueFactory();
                    if (obj != null)
                        redisClient.Set(key, obj, expiresAt);
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
