using Unity;
using Unity.Lifetime;

namespace HelpDesk.Common.Cache
{
    public static class CacheInstaller
    {
        public static void Install(IUnityContainer container)
        {
            container.RegisterType<ICache, InMemoryCache>();                        
        }
            
    }
}
