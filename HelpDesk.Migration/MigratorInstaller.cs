using Microsoft.Practices.Unity;


namespace HelpDesk.Migration
{
    public class MigratorInstaller
    {
        public static void Install(IUnityContainer container, string connectionString)
        {

            container.RegisterType<IHelpDeskMigrationRunner, HelpDeskMigrationRunner>(new InjectionConstructor(connectionString));
        }
    }
}
