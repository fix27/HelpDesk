using ServiceStack.Redis;
using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;
        

        public static void Install(string redisConnectionString, IUnityContainer container, LifetimeManager lifetimeManager)
        {
            _container = container;

            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ICache, InMemoryCache>(CacheLocation.InMemory.ToString());


            container.RegisterType<ICache, RedisCache>(CacheLocation.Redis.ToString(),
                lifetimeManager,
                new InjectionConstructor(new BasicRedisClientManager(redisConnectionString)));
        }

        public static ICache GetCache(CacheLocation? location = null)
        {
            if(location == null)
                _container.Resolve<ICache>();

            return _container.Resolve<ICache>(location.ToString());
        }
    }
}
