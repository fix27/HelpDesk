using Microsoft.Practices.Unity;
using HelpDesk.Data.Cache;
using HelpDesk.Data.Repository;


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
