using HelpDesk.Common.Cache.Interface;
using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core;
using System;

namespace HelpDesk.Common.Cache
{
    /// <summary>
    /// Redis-кэш
    /// </summary>
    public class RedisCache : ICache
    {
        private readonly ICacheClient _redisClient;
        public RedisCache(ICacheClient redisClient)
        {
            _redisClient = redisClient;
        }

        public T AddOrGetExisting<T>(string key, Func<T> valueFactory, int expirationSeconds = 0)
        {
            T value = default(T);
            try
            {
                value = _redisClient.Get<T>(key);
            }
            catch
            {
                return valueFactory();
            }

            if (value == null)
            {
                value = valueFactory();

                try
                {
                    if (expirationSeconds > 0)
                        _redisClient.Add(key, value, new TimeSpan(0, 0, expirationSeconds));
                    else
                        _redisClient.Add(key, value);
                }
                catch
                {

                }

                return value;
            }

            return value;
        }
                
        public void Remove(string key)
        {
            _redisClient.Remove(key);
        }

    }
}
