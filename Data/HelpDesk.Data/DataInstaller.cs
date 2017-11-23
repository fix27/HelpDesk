using HelpDesk.Data.Cache;
using HelpDesk.Data.Repository;
using Unity;
using Unity.Lifetime;
using Unity.Injection;

namespace HelpDesk.Data
{
    public static class DataInstaller
    {
        public static void Install(IUnityContainer container, LifetimeManager lifetimeManager)
        {
            container.RegisterType<IInMemoryCache, InMemoryCache>();
            container.RegisterType<ISettingsRepository, SettingsManager>(
                new InjectionConstructor(
                    new ResolvedParameter<ISettingsRepository>("SettingsRepository"),
                    new ResolvedParameter<IInMemoryCache>()));
            
        }
            
    }
}
