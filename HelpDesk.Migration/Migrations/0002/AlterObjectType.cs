using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171003184030, "Изменение таблицы ObjectType: добавление поля CountHour")]
    public class AlterObjectType : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("ObjectType").AddColumn("CountHour").AsInt32().NotNullable().WithDefaultValue(8);
        }
    }
}