using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

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
            throw new NotSupportedException();            
        }
               
        public object AddOrGetExisting(Type typeValue, string key, Func<object> valueFactory, int expirationSeconds = 0)
        {
            TimeSpan expiresAt = new TimeSpan(0, 0, 0, expirationSeconds > 0 ? expirationSeconds : 1000);
            using (IRedisClient redisClient = clientsManager.GetClient())
            {
                object obj = redisClient.GetValue(key);
                if (obj != null)
                {
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes((string)obj)))
                    {
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeValue);
                        return deserializer.ReadObject(ms);
                    }
                }
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
