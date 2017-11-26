using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171126142830, "Изменение таблицы ObjectType: добавление поля DeadlineHour")]
    public class AlterObjectType2 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("ObjectType")
                .AddColumn("DeadlineHour").AsInt32().NotNullable().WithDefaultValue(4);

        }
    }
}