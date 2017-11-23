using NHibernate;
using HelpDesk.Data.Query;
using HelpDesk.Data.Command;
using Unity;
using Unity.Lifetime;
using Unity.Injection;

namespace HelpDesk.Data.NHibernate
{
    public static class NHibernateDataInstaller
    {
        public static void Install(IUnityContainer container, LifetimeManager lifetimeManager)
        {
            //регистрация ISession
            container.RegisterType<ISession>(new InjectionFactory(c => NHibernateSessionManager.Instance.GetSession()));
            container.RegisterType<ISession>(lifetimeManager);
            
            container.RegisterType<IQueryRunner, QueryRunner>();
            container.RegisterType<ICommandRunner, CommandRunner>();
            
        }
    }
}
