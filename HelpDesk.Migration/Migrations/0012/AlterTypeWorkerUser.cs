using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171129093700, "Изменение таблицы TypeWorkerUser: добавление поля TypeCode")]
    public class AlterTypeWorkerUser : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("TypeWorkerUser")
                .AddColumn("TypeCode").AsInt32().NotNullable().WithDefaultValue(0);
            Execute.Sql("update TypeWorkerUser set TypeCode = Id - 1");

        }
        
    }
}