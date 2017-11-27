using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171127100400, "Изменение таблицы WorkerUser: добавление поля Subscribe")]
    public class AlterWorkerUser : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("WorkerUser")
                .AddColumn("Subscribe").AsBoolean().NotNullable().WithDefaultValue(true);

        }
    }
}