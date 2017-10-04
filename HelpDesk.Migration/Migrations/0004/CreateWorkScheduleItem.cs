using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171004184330, "Создание таблицы WorkScheduleItem")]
    public class CreateWorkScheduleItem : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("WorkScheduleItem")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("DayOfWeek").AsInt32().NotNullable().WithDefaultValue(0).Unique()
                .WithColumn("StartWorkDay").AsInt32().NotNullable().WithDefaultValue(9)
                .WithColumn("EndWorkDay").AsInt32().NotNullable().WithDefaultValue(18)
                .WithColumn("StartLunchBreak").AsInt32().Nullable().WithDefaultValue(13)
                .WithColumn("EndLunchBreak").AsInt32().Nullable().WithDefaultValue(14);
        }
    }
}