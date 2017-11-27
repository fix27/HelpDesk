using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171127100330, "Изменение таблицы Employee: удаление поля Subscribe")]
    public class AlterEmployee : ForwardOnlyMigration
    {
        public override void Up()
        {
            Delete.Column("Subscribe").FromTable("Employee");
            
        }
    }
}