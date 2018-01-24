using Unity;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        private static IUnityContainer _container;

        public static void Install(IUnityContainer container)
        {
            _container = container;
            string key = CacheLocation.InMemoryCache.ToString();
            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ICache, InMemoryCache>(key); 
        }

        public static ICache GetCache(string location = null)
        {
            if(string.IsNullOrWhiteSpace(location))
                _container.Resolve<ICache>();

            return _container.Resolve<ICache>(location);
        }
    }
}
