using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171125140030, "Создание таблиц CabinetUserEventSubscribe и WorkerUserEventSubscribe")]
    public class CreateUserEventSubscribe : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("CabinetUserEventSubscribe")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("StatusRequestId").AsInt64().NotNullable();

            Create.ForeignKey("CabinetUserEventSubscribe_CabinetUser_FK")
                .FromTable("CabinetUserEventSubscribe")
                .ForeignColumn("UserId")
                .ToTable("CabinetUser")
                .PrimaryColumn("Id");

            Create.ForeignKey("CabinetUserEventSubscribe_StatusRequest_FK")
                .FromTable("CabinetUserEventSubscribe")
                .ForeignColumn("StatusRequestId")
                .ToTable("StatusRequest")
                .PrimaryColumn("Id");




            Create.Table("WorkerUserEventSubscribe")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("UserId").AsInt64().NotNullable()
                .WithColumn("StatusRequestId").AsInt64().NotNullable();

            Create.ForeignKey("WorkerUserEventSubscribe_WorkerUser_FK")
                .FromTable("WorkerUserEventSubscribe")
                .ForeignColumn("UserId")
                .ToTable("WorkerUser")
                .PrimaryColumn("Id");

            Create.ForeignKey("WorkerUserEventSubscribe_StatusRequest_FK")
                .FromTable("WorkerUserEventSubscribe")
                .ForeignColumn("StatusRequestId")
                .ToTable("StatusRequest")
                .PrimaryColumn("Id");

        }
    }
}