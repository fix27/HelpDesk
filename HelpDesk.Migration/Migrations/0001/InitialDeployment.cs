using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20170804192830, "InitialDeployment")]
    public class InitialDeployment : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("create-database.sql");
        }
    }
}