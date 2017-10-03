using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171003195130, "Изменение таблицы Settings: добавление полей StartWorkDay, EndWorkDay, StartLunchBreak, EndLunchBreak")]
    public class AlterSettings : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("Settings").AddColumn("StartWorkDay").AsInt32().Nullable().WithDefaultValue(9);
            Alter.Table("Settings").AddColumn("EndWorkDay").AsInt32().Nullable().WithDefaultValue(18);
            
            Alter.Table("Settings").AddColumn("StartLunchBreak").AsInt32().Nullable().WithDefaultValue(13);
            Alter.Table("Settings").AddColumn("EndLunchBreak").AsInt32().Nullable().WithDefaultValue(14);

            Execute.Sql(@"update Settings set StartWorkDay = 9, EndWorkDay = 18, StartLunchBreak = 13, EndLunchBreak = 14");
        }
    }
}