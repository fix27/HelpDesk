using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;
using System;
using System.Web;

namespace HelpDesk.Common.Cache
{
    public class RedisClientFactory
	{
		private const string REDIS_CONTEXT_KEY = "HelpDesk.Redis";
		
		
		private static Lazy<RedisClientFactory> instance =
		  new Lazy<RedisClientFactory>(() =>
		  {
			  var instance = new RedisClientFactory();
			  return instance;
		  });

		public static RedisClientFactory Instance => instance.Value;
		

		public ICacheClient GetCurrent(string connectionString)
		{
			if (!HttpContext.Current.Items.Contains(REDIS_CONTEXT_KEY))
			{
                int port = 6379;
                var parts = connectionString.Split(':');
                if (parts.Length == 2)
                    Int32.TryParse(parts[1], out port);

                ISerializer serializer = new NewtonsoftSerializer();
                RedisConfiguration configuration = new RedisConfiguration()
                {
                    Hosts = new RedisHost[]
                    {
                        new RedisHost(){Host = parts[0], Port = port}
                    }
                };

                var redisClient = new StackExchangeRedisCacheClient(serializer, configuration);
				HttpContext.Current.Items.Add(REDIS_CONTEXT_KEY, redisClient);
			}

			return HttpContext.Current.Items[REDIS_CONTEXT_KEY] as ICacheClient;
		}
		
	}
}