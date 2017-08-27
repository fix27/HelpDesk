
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[OrganizationObjectTypeWorker_ObjectType_FK]') AND parent_object_id = OBJECT_ID('OrganizationObjectTypeWorker'))
alter table OrganizationObjectTypeWorker  drop constraint OrganizationObjectTypeWorker_ObjectType_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[OrganizationObjectTypeWorker_Organization_FK]') AND parent_object_id = OBJECT_ID('OrganizationObjectTypeWorker'))
alter table OrganizationObjectTypeWorker  drop constraint OrganizationObjectTypeWorker_Organization_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[OrganizationObjectTypeWorker_Worker_FK]') AND parent_object_id = OBJECT_ID('OrganizationObjectTypeWorker'))
alter table OrganizationObjectTypeWorker  drop constraint OrganizationObjectTypeWorker_Worker_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[Request_Status_FK]') AND parent_object_id = OBJECT_ID('Request'))
alter table Request  drop constraint Request_Status_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[Request_Object_FK]') AND parent_object_id = OBJECT_ID('Request'))
alter table Request  drop constraint Request_Object_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[Request_Employee_FK]') AND parent_object_id = OBJECT_ID('Request'))
alter table Request  drop constraint Request_Employee_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[Request_Worker_FK]') AND parent_object_id = OBJECT_ID('Request'))
alter table Request  drop constraint Request_Worker_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestArch_Status_FK]') AND parent_object_id = OBJECT_ID('RequestArch'))
alter table RequestArch  drop constraint RequestArch_Status_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestArch_Object_FK]') AND parent_object_id = OBJECT_ID('RequestArch'))
alter table RequestArch  drop constraint RequestArch_Object_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestArch_Employee_FK]') AND parent_object_id = OBJECT_ID('RequestArch'))
alter table RequestArch  drop constraint RequestArch_Employee_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestArch_Worker_FK]') AND parent_object_id = OBJECT_ID('RequestArch'))
alter table RequestArch  drop constraint RequestArch_Worker_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestEvent_StatusRequest_FK]') AND parent_object_id = OBJECT_ID('RequestEvent'))
alter table RequestEvent  drop constraint RequestEvent_StatusRequest_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestEventArch_StatusRequest_FK]') AND parent_object_id = OBJECT_ID('RequestEventArch'))
alter table RequestEventArch  drop constraint RequestEventArch_StatusRequest_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestObject_ObjectType_FK]') AND parent_object_id = OBJECT_ID('RequestObject'))
alter table RequestObject  drop constraint RequestObject_ObjectType_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestObject_HardType_FK]') AND parent_object_id = OBJECT_ID('RequestObject'))
alter table RequestObject  drop constraint RequestObject_HardType_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[RequestObject_Model_FK]') AND parent_object_id = OBJECT_ID('RequestObject'))
alter table RequestObject  drop constraint RequestObject_Model_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[Model_Manufacturer_FK]') AND parent_object_id = OBJECT_ID('Model'))
alter table Model  drop constraint Model_Manufacturer_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[PersonalProfileObject_PersonalProfile_FK]') AND parent_object_id = OBJECT_ID('PersonalProfileObject'))
alter table PersonalProfileObject  drop constraint PersonalProfileObject_PersonalProfile_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[PersonalProfileObject_RequestObject_FK]') AND parent_object_id = OBJECT_ID('PersonalProfileObject'))
alter table PersonalProfileObject  drop constraint PersonalProfileObject_RequestObject_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[PersonalProfile_Post_FK]') AND parent_object_id = OBJECT_ID('PersonalProfile'))
alter table PersonalProfile  drop constraint PersonalProfile_Post_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[PersonalProfile_Organization_FK]') AND parent_object_id = OBJECT_ID('PersonalProfile'))
alter table PersonalProfile  drop constraint PersonalProfile_Organization_FK


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[PersonalProfile_User_FK]') AND parent_object_id = OBJECT_ID('PersonalProfile'))
alter table PersonalProfile  drop constraint PersonalProfile_User_FK


    if exists (select * from dbo.sysobjects where id = object_id(N'RequestFile') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table RequestFile

    if exists (select * from dbo.sysobjects where id = object_id(N'OrganizationObjectTypeWorker') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table OrganizationObjectTypeWorker

    if exists (select * from dbo.sysobjects where id = object_id(N'BaseRequest') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table BaseRequest

    if exists (select * from dbo.sysobjects where id = object_id(N'Request') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Request

    if exists (select * from dbo.sysobjects where id = object_id(N'RequestArch') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table RequestArch

    if exists (select * from dbo.sysobjects where id = object_id(N'BaseRequestEvent') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table BaseRequestEvent

    if exists (select * from dbo.sysobjects where id = object_id(N'RequestEvent') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table RequestEvent

    if exists (select * from dbo.sysobjects where id = object_id(N'RequestEventArch') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table RequestEventArch

    if exists (select * from dbo.sysobjects where id = object_id(N'StatusRequest') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table StatusRequest

    if exists (select * from dbo.sysobjects where id = object_id(N'RequestObject') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table RequestObject

    if exists (select * from dbo.sysobjects where id = object_id(N'Worker') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Worker

    if exists (select * from dbo.sysobjects where id = object_id(N'HardType') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table HardType

    if exists (select * from dbo.sysobjects where id = object_id(N'Manufacturer') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Manufacturer

    if exists (select * from dbo.sysobjects where id = object_id(N'Model') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Model

    if exists (select * from dbo.sysobjects where id = object_id(N'ObjectType') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ObjectType

    if exists (select * from dbo.sysobjects where id = object_id(N'PersonalProfileObject') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table PersonalProfileObject

    if exists (select * from dbo.sysobjects where id = object_id(N'Organization') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Organization

    if exists (select * from dbo.sysobjects where id = object_id(N'[User]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [User]

    if exists (select * from dbo.sysobjects where id = object_id(N'Settings') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Settings

    if exists (select * from dbo.sysobjects where id = object_id(N'Post') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Post

    if exists (select * from dbo.sysobjects where id = object_id(N'PersonalProfile') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table PersonalProfile

    IF EXISTS (select * from sys.sequences where name = N'SQ_GLOBAL') DROP SEQUENCE SQ_GLOBAL

    IF EXISTS (select * from sys.sequences where name = N'SQ_REQUEST') DROP SEQUENCE SQ_REQUEST

    create table RequestFile (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       Body VARBINARY(MAX) not null,
       Thumbnail VARBINARY(MAX) not null,
       Type NVARCHAR(10) not null,
       Size INT not null,
       TempRequestKey UNIQUEIDENTIFIER not null,
       RequestId BIGINT null,
       primary key (Id)
    )

    create table OrganizationObjectTypeWorker (
        Id BIGINT not null,
       ObjectTypeId BIGINT not null,
       OrganizationId BIGINT not null,
       WorkerId BIGINT not null,
       primary key (Id)
    )

    create table BaseRequest (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME not null,
       DateUpdate DATETIME not null,
       DateEndPlan DATETIME not null,
       DateEndFact DATETIME null,
       DescriptionProblem NVARCHAR(2000) not null,
       UserId BIGINT null,
       CountCorrectionDateEndPlan INT not null,
       primary key (Id)
    )

    create table Request (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME not null,
       DateUpdate DATETIME not null,
       DateEndPlan DATETIME not null,
       DateEndFact DATETIME null,
       DescriptionProblem NVARCHAR(2000) not null,
       UserId BIGINT null,
       CountCorrectionDateEndPlan INT not null,
       StatusId BIGINT not null,
       ObjectId BIGINT not null,
       EmployeeId BIGINT not null,
       WorkerId BIGINT not null,
       primary key (Id)
    )

    create table RequestArch (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME not null,
       DateUpdate DATETIME not null,
       DateEndPlan DATETIME not null,
       DateEndFact DATETIME null,
       DescriptionProblem NVARCHAR(2000) not null,
       UserId BIGINT null,
       CountCorrectionDateEndPlan INT not null,
       StatusId BIGINT not null,
       ObjectId BIGINT not null,
       EmployeeId BIGINT not null,
       WorkerId BIGINT not null,
       primary key (Id)
    )

    create table BaseRequestEvent (
        Id BIGINT not null,
       RequestId BIGINT not null,
       TypeRequestEventId BIGINT null,
       Name NVARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME not null,
       DateInsert DATETIME not null,
       primary key (Id)
    )

    create table RequestEvent (
        Id BIGINT not null,
       RequestId BIGINT not null,
       TypeRequestEventId BIGINT null,
       Name NVARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME not null,
       DateInsert DATETIME not null,
       StatusRequestId BIGINT null,
       primary key (Id)
    )

    create table RequestEventArch (
        Id BIGINT not null,
       RequestId BIGINT not null,
       TypeRequestEventId BIGINT null,
       Name NVARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME not null,
       DateInsert DATETIME not null,
       StatusRequestId BIGINT null,
       primary key (Id)
    )

    create table StatusRequest (
        Id BIGINT not null,
       Name NVARCHAR(200) not null unique,
       BackColor NVARCHAR(255) null,
       primary key (Id)
    )

    create table RequestObject (
        Id BIGINT not null,
       SoftName NVARCHAR(200) null,
       ObjectTypeId BIGINT not null,
       HardTypeId BIGINT null,
       ModelId BIGINT null,
       Archive BIT not null,
       primary key (Id)
    )

    create table Worker (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       primary key (Id)
    )

    create table HardType (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       primary key (Id)
    )

    create table Manufacturer (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       primary key (Id)
    )

    create table Model (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       ManufacturerId BIGINT not null,
       primary key (Id)
    )

    create table ObjectType (
        Id BIGINT not null,
       Name NVARCHAR(200) not null,
       Soft BIT not null,
       Archive BIT not null,
       primary key (Id)
    )

    create table PersonalProfileObject (
        Id BIGINT not null,
       PersonalProfileId BIGINT not null,
       ObjectId BIGINT not null,
       primary key (Id)
    )

    create table Organization (
        Id BIGINT not null,
       Name NVARCHAR(300) not null,
       Address NVARCHAR(300) not null,
       ParentId BIGINT null,
       HasChild BIT not null,
       Archive BIT not null,
       primary key (Id)
    )

    create table [User] (
        Id BIGINT not null,
       Email NVARCHAR(200) not null unique,
       Password NVARCHAR(100) not null,
       primary key (Id)
    )

    create table Settings (
        Id BIGINT not null,
       MinInterval DECIMAL(19,5) not null,
       LimitRequestCount INT not null,
       MaxRequestFileCount INT not null,
       MaxRequestFileSize INT not null,
       MaxFileNameLength INT not null,
       ManualUrl NVARCHAR(200) null,
       ServiceLevelAgreementUrl NVARCHAR(200) null,
       Message NVARCHAR(4000) null,
       TechSupportPhones NVARCHAR(200) null,
       primary key (Id)
    )

    create table Post (
        Id BIGINT not null,
       Name NVARCHAR(200) not null unique,
       primary key (Id)
    )

    create table PersonalProfile (
        Id BIGINT not null,
       FM NVARCHAR(200) not null,
       IM NVARCHAR(200) not null,
       OT NVARCHAR(200) not null,
       Phone NVARCHAR(200) not null,
       Cabinet NVARCHAR(200) not null,
       Subscribe BIT not null,
       PostId BIGINT not null,
       OrganizationId BIGINT not null,
       primary key (Id)
    )

    alter table OrganizationObjectTypeWorker 
        add constraint OrganizationObjectTypeWorker_ObjectType_FK 
        foreign key (ObjectTypeId) 
        references ObjectType

    alter table OrganizationObjectTypeWorker 
        add constraint OrganizationObjectTypeWorker_Organization_FK 
        foreign key (OrganizationId) 
        references Organization

    alter table OrganizationObjectTypeWorker 
        add constraint OrganizationObjectTypeWorker_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table Request 
        add constraint Request_Status_FK 
        foreign key (StatusId) 
        references StatusRequest

    alter table Request 
        add constraint Request_Object_FK 
        foreign key (ObjectId) 
        references RequestObject

    alter table Request 
        add constraint Request_Employee_FK 
        foreign key (EmployeeId) 
        references PersonalProfile

    alter table Request 
        add constraint Request_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table RequestArch 
        add constraint RequestArch_Status_FK 
        foreign key (StatusId) 
        references StatusRequest

    alter table RequestArch 
        add constraint RequestArch_Object_FK 
        foreign key (ObjectId) 
        references RequestObject

    alter table RequestArch 
        add constraint RequestArch_Employee_FK 
        foreign key (EmployeeId) 
        references PersonalProfile

    alter table RequestArch 
        add constraint RequestArch_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table RequestEvent 
        add constraint RequestEvent_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table RequestEventArch 
        add constraint RequestEventArch_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table RequestObject 
        add constraint RequestObject_ObjectType_FK 
        foreign key (ObjectTypeId) 
        references ObjectType

    alter table RequestObject 
        add constraint RequestObject_HardType_FK 
        foreign key (HardTypeId) 
        references HardType

    alter table RequestObject 
        add constraint RequestObject_Model_FK 
        foreign key (ModelId) 
        references Model

    alter table Model 
        add constraint Model_Manufacturer_FK 
        foreign key (ManufacturerId) 
        references Manufacturer

    alter table PersonalProfileObject 
        add constraint PersonalProfileObject_PersonalProfile_FK 
        foreign key (PersonalProfileId) 
        references PersonalProfile

    alter table PersonalProfileObject 
        add constraint PersonalProfileObject_RequestObject_FK 
        foreign key (ObjectId) 
        references RequestObject

    create index User_Email_idx on [User] (Email)

    create index Post_Name_idx on Post (Name)

    alter table PersonalProfile 
        add constraint PersonalProfile_Post_FK 
        foreign key (PostId) 
        references Post

    alter table PersonalProfile 
        add constraint PersonalProfile_Organization_FK 
        foreign key (OrganizationId) 
        references Organization

    alter table PersonalProfile 
        add constraint PersonalProfile_User_FK 
        foreign key (Id) 
        references [User]

    create sequence SQ_GLOBAL as int start with 100000 increment by 1

    create sequence SQ_REQUEST as int start with 100000 increment by 1




insert into Settings(Id, MinInterval, LimitRequestCount,  MaxRequestFileCount,  MaxRequestFileSize,  MaxFileNameLength) values(1, 1, 10, 5, 500, 200);

insert into ObjectType(Id, Name, Soft, Archive) values(1, 'Сопровождение ИС', 1, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(2, 'ТО ВТ', 1, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(3, 'ТО КМТ', 1, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(4, 'Сетевое администрирование', 1, 0);

insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(1, 'ПП "Парус-Бюджет"', 1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(2, 'ПП "1C: Бухгалтерия"', 0, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(3, 'ПП "Кодекс: Документооборот"', 0, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(4, 'ПП "1C: Кадры"', 0, 0);

insert into Manufacturer(Id, Name) values(1,'SONY');
insert into Manufacturer(Id, Name) values(2,'HP');
insert into Manufacturer(Id, Name) values(3,'CANNON');

insert into HardType(Id, Name) values(1,'ПРИНТЕР');
insert into HardType(Id, Name) values(2,'МОНИТОР');
insert into HardType(Id, Name) values(3,'СКАНЕР');
insert into HardType(Id, Name) values(4,'КЛАВИАТУРА');

insert into StatusRequest(Id, Name, BackColor) values(824,'Отказано', '#FFA07A');
insert into StatusRequest(Id, Name, BackColor) values(825,'Перенос', '#FFC0CB');
insert into StatusRequest(Id, Name, BackColor) values(826,'Отказано в готовности', '#FFA500');
insert into StatusRequest(Id, Name, BackColor) values(191,'Рассмотрение', '#6B8E23');
insert into StatusRequest(Id, Name, BackColor) values(192,'Принята', '#FFE4B5');
insert into StatusRequest(Id, Name, BackColor) values(193,'Подтверждение переноса', '#E6E6FA');
insert into StatusRequest(Id, Name, BackColor) values(194,'Подтверждение отказа', '#D2B48C');
insert into StatusRequest(Id, Name, BackColor) values(195,'Подтверждение готовности', '#40E0D0');
insert into StatusRequest(Id, Name, BackColor) values(196,'Выполнена', '#7B68EE');
insert into StatusRequest(Id, Name, BackColor) values(839,'Отказ после принятия', '#ADD8E6');
insert into StatusRequest(Id, Name, BackColor) values(22464,'Пасив', '#F0E68C');
insert into StatusRequest(Id, Name, BackColor) values(383343,'Перенос готовности', '#FF4500');
insert into StatusRequest(Id, Name, BackColor) values(197,'Дата окончания', null);

insert into [User](Id, Email, Password) values(1, 'admin@mail.ru','admin@mail.ru');
insert into [User](Id, Email, Password) values(2, 'user@mail.ru','user@mail.ru');

insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(1, 'Управление строительства', 'ул. Мира 1, стр 2', null, 1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(2, 'Управление муниципального заказа', 'ул. Мира 1, стр 2', null, 1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(3, 'Руководство', 'ул. Мира 1, стр 2', null, 1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(4, 'Отдел ценообразования', 'ул. Мира 1, стр 2', 1, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(5, 'Отдел общего обеспечения', 'ул. Мира 1, стр 2', 1, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(6, 'Плановый отдел', 'ул. Мира 1, стр 2', 1, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(7, 'Отдел закупок', 'ул. Мира 1, стр 2', 2, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(8, 'Отдел контроля цен', 'ул. Мира 1, стр 2', 2, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(9, 'Контрольный отдел', 'ул. Мира 1, стр 2', 2, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(10, 'Бухгалтерия', 'ул. Мира 1, стр 2', 3, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(11, 'Отдел кадров', 'ул. Мира 1, стр 2', 3, 0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(12, 'Отдел охраны труда', 'ул. Мира 1, стр 2', 3, 0, 0);

insert into Worker(Id, Name) values(1, 'ООО "Автоматика"');
insert into Worker(Id, Name) values(2, 'ООО "Старт"');
insert into Worker(Id, Name) values(3, 'ООО "Первый"');
insert into Worker(Id, Name) values(4, 'ООО "Деловые решения"');

insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(1, 1, 1, 1);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(2, 1, 2, 2);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(3, 1, 3, 3);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(4, 1, 4, 4);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(5, 2, 5, 1);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(6, 2, 6, 2);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(7, 2, 7, 3);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(8, 2, 8, 4);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(9, 2, 9, 1);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(10, 3, 10, 2);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(11, 3, 11, 3);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(12, 3, 12, 4);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(13, 3, 1, 1);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(14, 4, 2, 2);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(15, 4, 3, 3);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(16, 4, 4, 4);
insert into OrganizationObjectTypeWorker(Id, ObjectTypeId, OrganizationId, WorkerId) values(17, 4, 5, 1);

insert into Post(Id, Name) values(1, 'Начальник управления');
insert into Post(Id, Name) values(2, 'Начальник отдела');