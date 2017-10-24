using FluentMigrator;

namespace HelpDesk.Migration.Migrations
{
    [Migration(20171024224930, "Создание таблицы DescriptionProblem")]
    public class CreateDescriptionProblem : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("DescriptionProblem")
                .WithColumn("Id").AsInt64().PrimaryKey()
                .WithColumn("Name").AsAnsiString(2000).NotNullable()
                .WithColumn("ObjectId").AsInt64().Nullable()
                .WithColumn("HardTypeId").AsInt64().Nullable();

            Create.ForeignKey("DescriptionProblem_RequestObject_FK")
                .FromTable("DescriptionProblem")
                .ForeignColumn("ObjectId")
                .ToTable("RequestObject")
                .PrimaryColumn("Id");

            Create.ForeignKey("DescriptionProblem_HardType_FK")
                .FromTable("DescriptionProblem")
                .ForeignColumn("HardTypeId")
                .ToTable("HardType")
                .PrimaryColumn("Id");
        }
    }
}