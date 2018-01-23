using Unity;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;

        public static void Install(IUnityContainer container)
        {
            _container = container;
            string key = TypeCacheEnum.InMemoryCache.ToString();
            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ICache, InMemoryCache>(key); 
        }

        public static ICache GetCache(string typeCacheName = null)
        {
            if(string.IsNullOrWhiteSpace(typeCacheName))
                _container.Resolve<ICache>();

            return _container.Resolve<ICache>(typeCacheName);
        }
    }
}
