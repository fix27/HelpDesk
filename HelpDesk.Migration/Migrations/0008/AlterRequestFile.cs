using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171113215230, "Изменение таблицы RequestFile: разрешение NULL для поля Thumbnail и изменение размера Type")]
    public class AlterRequestFile : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("RequestFile")
                .AlterColumn("Thumbnail").AsBinary().Nullable()
                .AlterColumn("Type").AsAnsiString(500).NotNullable();



        }
    }
}