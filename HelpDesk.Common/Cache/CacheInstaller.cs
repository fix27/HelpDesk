using Unity;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;

        public static void Install(IUnityContainer container)
        {
            _container = container;
            
            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ICache, InMemoryCache>(CacheLocation.InMemory.ToString());

            
            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ICache, MemcachedCache>(CacheLocation.Memcached.ToString());
        }

        public static ICache GetCache(CacheLocation? location = null)
        {
            if(location == null)
                _container.Resolve<ICache>();

            return _container.Resolve<ICache>(location.ToString());
        }
    }
}
