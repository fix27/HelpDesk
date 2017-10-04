using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171004124830, "Создание таблицы WorkCalendarItem")]
    public class CreateWorkCalendarItem : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("WorkCalendarItem")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("TypeItem").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("Date").AsDateTime().NotNullable();
        }
    }
}