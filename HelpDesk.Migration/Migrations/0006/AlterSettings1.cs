using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171007133130, "Изменение таблицы Settings: добавление полей MinCountTransferDay и MaxCountTransferDay")]
    public class AlterSettings1 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("Settings").AddColumn("MinCountTransferDay").AsInt32().Nullable().WithDefaultValue(3);
            Alter.Table("Settings").AddColumn("MaxCountTransferDay").AsInt32().Nullable().WithDefaultValue(30);

            Execute.Sql(@"update Settings set MinCountTransferDay = 3, MaxCountTransferDay = 30");
        }
    }
}