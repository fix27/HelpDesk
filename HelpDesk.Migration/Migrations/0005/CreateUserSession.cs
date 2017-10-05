using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171005203930, "Создание таблицы UserSession и последовательности SQ_USERSESSION")]
    public class CreateUserSession : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("UserSession")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("DateInsert").AsDateTime().NotNullable()
                .WithColumn("ApplicationType").AsInt32().NotNullable()
                .WithColumn("IP").AsAnsiString(100);

            Execute.Sql("create sequence SQ_USERSESSION as int start with 100000 increment by 1");
        }
    }
}