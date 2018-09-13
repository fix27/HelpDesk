using HelpDesk.Common.Cache.Interface;
using StackExchange.Redis.Extensions.Core;
using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;
        private static CacheLocation _defaultLocation;
        
        public static void Install(string redisConnectionString, CacheLocation defaultLocation, IUnityContainer container, LifetimeManager lifetimeManager)
        {
            _container = container;
            _defaultLocation = defaultLocation;

            if (!String.IsNullOrWhiteSpace(redisConnectionString))
            {
                _container.RegisterType<ICacheClient>(lifetimeManager,
                    new InjectionFactory(c => RedisClientFactory.Instance.GetCurrent(redisConnectionString)));

                _container.RegisterType<ICache, RedisCache>(_defaultLocation.ToString());
            }
            else
            {
                _container.RegisterType<ICache, MemoryCache>(_defaultLocation.ToString());
            }


            _container.RegisterType<IMemoryCache, MemoryCache>();
        }
        public static ICache GetCache(CacheLocation? location = null)
        {
            if (location == null)
                return _container.Resolve<ICache>(_defaultLocation.ToString());

            return _container.Resolve<ICache>(location.ToString());
        }
    }
}
