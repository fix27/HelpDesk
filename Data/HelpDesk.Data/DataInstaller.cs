using HelpDesk.Data.Repository;
using Unity;
using Unity.Lifetime;
using Unity.Injection;
using HelpDesk.Common.Cache;

namespace HelpDesk.Data
{
    public static class DataInstaller
    {
        public static void Install(IUnityContainer container, LifetimeManager lifetimeManager)
        {
            container.RegisterType<ICache, InMemoryCache>();
            container.RegisterType<ISettingsRepository, SettingsManager>(
                new InjectionConstructor(
                    new ResolvedParameter<ISettingsRepository>("SettingsRepository"),
                    new ResolvedParameter<ICache>()));
            
        }
            
    }
}
