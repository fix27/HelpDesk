using ServiceStack.Redis;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;
        private static CacheLocation _defaultLocation;
        
        public static void Install(string redisConnectionString, CacheLocation defaultLocation, int defaultExpirationSeconds, IUnityContainer container, LifetimeManager lifetimeManager)
        {
            _container = container;
            _defaultLocation = defaultLocation;

            int _defaultExpirationSeconds = defaultExpirationSeconds > 0 ? defaultExpirationSeconds : 1000;

            container.RegisterType<ICache, InMemoryCache>(
                new InjectionConstructor(_defaultExpirationSeconds));
            container.RegisterType<ICache, InMemoryCache>(CacheLocation.InMemory.ToString(),
                new InjectionConstructor(_defaultExpirationSeconds));


            container.RegisterType<ICache, RedisCache>(CacheLocation.Redis.ToString(),
                lifetimeManager,
                new InjectionConstructor(
                    new BasicRedisClientManager(redisConnectionString),
                    _defaultExpirationSeconds));
        }
        public static ICache GetCache(CacheLocation? location = null)
        {
            if (location == null)
                return _container.Resolve<ICache>(_defaultLocation.ToString());

            return _container.Resolve<ICache>(location.ToString());
        }
    }
}
