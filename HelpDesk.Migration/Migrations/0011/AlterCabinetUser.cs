using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171127100300, "Изменение таблицы CabinetUser: добавление поля Subscribe")]
    public class AlterCabinetUser : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("CabinetUser")
                .AddColumn("Subscribe").AsBoolean().NotNullable().WithDefaultValue(true);

        }
    }
}