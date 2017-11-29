    create table CabinetUserEventSubscribe (
        Id BIGINT not null,
       UserId BIGINT not null,
       StatusRequestId BIGINT not null,
       primary key (Id)
    )

    create table WorkerUserEventSubscribe (
        Id BIGINT not null,
       UserId BIGINT not null,
       StatusRequestId BIGINT not null,
       primary key (Id)
    )

    create table DescriptionProblem (
        Id BIGINT not null,
       Name VARCHAR(2000) not null,
       ObjectId BIGINT null,
       HardTypeId BIGINT null,
       primary key (Id)
    )

    create table UserSession (
        Id BIGINT not null,
       UserId BIGINT not null,
       DateInsert DATETIME2 not null,
       ApplicationType INT not null,
       IP VARCHAR(255) null,
       primary key (Id)
    )

    create table WorkScheduleItem (
        Id BIGINT not null,
       DayOfWeek INT not null,
       StartWorkDay INT not null,
       EndWorkDay INT not null,
       StartLunchBreak INT null,
       EndLunchBreak INT null,
       primary key (Id)
    )

    create table WorkCalendarItem (
        Id BIGINT not null,
       TypeItem INT not null,
       Date DATETIME2 not null,
       primary key (Id)
    )

    create table TypeWorkerUser (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       AllowableStates VARCHAR(200) null,
       TypeCode INT not null,
       primary key (Id)
    )

    create table AccessWorkerUser (
        Id BIGINT not null,
       Type INT not null,
       UserId BIGINT not null,
       ObjectTypeId BIGINT null,
       OrganizationId BIGINT null,
       ObjectId BIGINT null,
       WorkerId BIGINT null,
       OrganizationAddress VARCHAR(1000) null,
       primary key (Id)
    )

    create table WorkerUser (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       Email VARCHAR(200) not null unique,
       Password VARCHAR(100) not null,
       Subscribe BIT not null,
       WorkerId BIGINT null,
       TypeWorkerUserId BIGINT not null,
       primary key (Id)
    )

    create table RequestFile (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       Body VARBINARY(MAX) not null,
       Thumbnail VARBINARY(MAX) null,
       Type VARCHAR(10) not null,
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
       DateInsert DATETIME2 not null,
       DateUpdate DATETIME2 not null,
       DateEndPlan DATETIME2 not null,
       DateEndFact DATETIME2 null,
       DescriptionProblem VARCHAR(2000) not null,
       CountCorrectionDateEndPlan INT not null,
       primary key (Id)
    )

    create table Request (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME2 not null,
       DateUpdate DATETIME2 not null,
       DateEndPlan DATETIME2 not null,
       DateEndFact DATETIME2 null,
       DescriptionProblem VARCHAR(2000) not null,
       CountCorrectionDateEndPlan INT not null,
       StatusId BIGINT not null,
       ObjectId BIGINT not null,
       EmployeeId BIGINT not null,
       WorkerId BIGINT not null,
       UserId BIGINT null,
       primary key (Id)
    )

    create table RequestArch (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME2 not null,
       DateUpdate DATETIME2 not null,
       DateEndPlan DATETIME2 not null,
       DateEndFact DATETIME2 null,
       DescriptionProblem VARCHAR(2000) not null,
       CountCorrectionDateEndPlan INT not null,
       StatusId BIGINT not null,
       ObjectId BIGINT not null,
       EmployeeId BIGINT not null,
       WorkerId BIGINT not null,
       UserId BIGINT null,
       primary key (Id)
    )

    create table BaseRequestEvent (
        Id BIGINT not null,
       RequestId BIGINT not null,
       Note VARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME2 not null,
       DateInsert DATETIME2 not null,
       primary key (Id)
    )

    create table RequestEvent (
        Id BIGINT not null,
       RequestId BIGINT not null,
       Note VARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME2 not null,
       DateInsert DATETIME2 not null,
       StatusRequestId BIGINT null,
       UserId BIGINT null,
       primary key (Id)
    )

    create table RequestEventArch (
        Id BIGINT not null,
       RequestId BIGINT not null,
       Note VARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME2 not null,
       DateInsert DATETIME2 not null,
       StatusRequestId BIGINT null,
       UserId BIGINT null,
       primary key (Id)
    )

    create table StatusRequest (
        Id BIGINT not null,
       Name VARCHAR(200) not null unique,
       BackColor VARCHAR(40) null,
       AllowableStates VARCHAR(200) null,
       ActionName VARCHAR(200) null,
       primary key (Id)
    )

    create table RequestObject (
        Id BIGINT not null,
       SoftName VARCHAR(200) null,
       ObjectTypeId BIGINT not null,
       HardTypeId BIGINT null,
       ModelId BIGINT null,
       Archive BIT not null,
       primary key (Id)
    )

    create table Worker (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       primary key (Id)
    )

    create table HardType (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       primary key (Id)
    )

    create table Manufacturer (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       primary key (Id)
    )

    create table Model (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       ManufacturerId BIGINT not null,
       primary key (Id)
    )

    create table ObjectType (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       Soft BIT not null,
       Archive BIT not null,
       CountHour INT not null,
       DeadlineHour INT not null,
       primary key (Id)
    )

    create table EmployeeObject (
        Id BIGINT not null,
       EmployeeId BIGINT not null,
       ObjectId BIGINT not null,
       primary key (Id)
    )

    create table Organization (
        Id BIGINT not null,
       Name VARCHAR(300) not null,
       Address VARCHAR(300) not null,
       ParentId BIGINT null,
       HasChild BIT not null,
       Archive BIT not null,
       primary key (Id)
    )

    create table CabinetUser (
        Id BIGINT not null,
       Email VARCHAR(200) not null unique,
       Password VARCHAR(100) not null,
       Subscribe BIT not null,
       primary key (Id)
    )

    create table Settings (
        Id BIGINT not null,
       MinInterval DECIMAL(19,5) not null,
       LimitRequestCount INT not null,
       MaxRequestFileCount INT not null,
       MaxRequestFileSize INT not null,
       MaxFileNameLength INT not null,
       ManualUrl VARCHAR(200) null,
       ServiceLevelAgreementUrl VARCHAR(200) null,
       Message VARCHAR(4000) null,
       TechSupportPhones VARCHAR(200) null,
       StartWorkDay INT null,
       EndWorkDay INT null,
       StartLunchBreak INT null,
       EndLunchBreak INT null,
       MinCountTransferDay INT null,
       MaxCountTransferDay INT null,
       primary key (Id)
    )

    create table Post (
        Id BIGINT not null,
       Name VARCHAR(200) not null unique,
       primary key (Id)
    )

    create table Employee (
        Id BIGINT not null,
       FM VARCHAR(200) not null,
       IM VARCHAR(200) not null,
       OT VARCHAR(200) not null,
       Phone VARCHAR(200) not null,
       Cabinet VARCHAR(200) not null,
       PostId BIGINT not null,
       OrganizationId BIGINT not null,
       primary key (Id)
    )

    alter table CabinetUserEventSubscribe 
        add constraint CabinetUserEventSubscribe_CabinetUser_FK 
        foreign key (UserId) 
        references CabinetUser

    alter table CabinetUserEventSubscribe 
        add constraint CabinetUserEventSubscribe_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table WorkerUserEventSubscribe 
        add constraint WorkerUserEventSubscribe_WorkerUser_FK 
        foreign key (UserId) 
        references WorkerUser

    alter table WorkerUserEventSubscribe 
        add constraint WorkerUserEventSubscribe_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table DescriptionProblem 
        add constraint DescriptionProblem_RequestObject_FK 
        foreign key (ObjectId) 
        references RequestObject

    alter table DescriptionProblem 
        add constraint DescriptionProblem_HardType_FK 
        foreign key (HardTypeId) 
        references HardType

    alter table AccessWorkerUser 
        add constraint AccessWorkerUser_WorkerUser_FK 
        foreign key (UserId) 
        references WorkerUser

    alter table AccessWorkerUser 
        add constraint AccessWorkerUser_ObjectType_FK 
        foreign key (ObjectTypeId) 
        references ObjectType

    alter table AccessWorkerUser 
        add constraint AccessWorkerUser_Organization_FK 
        foreign key (OrganizationId) 
        references Organization

    alter table AccessWorkerUser 
        add constraint AccessWorkerUser_Object_FK 
        foreign key (ObjectId) 
        references RequestObject

    alter table AccessWorkerUser 
        add constraint AccessWorkerUser_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    create index User_Email_idx on WorkerUser (Email)

    alter table WorkerUser 
        add constraint WorkerUser_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table WorkerUser 
        add constraint WorkerUser_TypeWorkerUser_FK 
        foreign key (TypeWorkerUserId) 
        references TypeWorkerUser

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
        references Employee

    alter table Request 
        add constraint Request_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table Request 
        add constraint Request_User_FK 
        foreign key (UserId) 
        references WorkerUser

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
        references Employee

    alter table RequestArch 
        add constraint RequestArch_Worker_FK 
        foreign key (WorkerId) 
        references Worker

    alter table RequestArch 
        add constraint RequestArch_User_FK 
        foreign key (UserId) 
        references WorkerUser

    alter table RequestEvent 
        add constraint RequestEvent_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table RequestEvent 
        add constraint RequestEvent_User_FK 
        foreign key (UserId) 
        references WorkerUser

    alter table RequestEventArch 
        add constraint RequestEventArch_StatusRequest_FK 
        foreign key (StatusRequestId) 
        references StatusRequest

    alter table RequestEventArch 
        add constraint RequestEventArch_User_FK 
        foreign key (UserId) 
        references WorkerUser

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

    alter table EmployeeObject 
        add constraint EmployeeObject_Employee_FK 
        foreign key (EmployeeId) 
        references Employee

    alter table EmployeeObject 
        add constraint EmployeeObject_RequestObject_FK 
        foreign key (ObjectId) 
        references RequestObject

    create index User_Email_idx on CabinetUser (Email)

    create index Post_Name_idx on Post (Name)

    alter table Employee 
        add constraint Employee_Post_FK 
        foreign key (PostId) 
        references Post

    alter table Employee 
        add constraint Employee_Organization_FK 
        foreign key (OrganizationId) 
        references Organization

    create sequence SQ_GLOBAL as int start with 100000 increment by 1

    create sequence SQ_USERSESSION as int start with 100000 increment by 1

    create sequence SQ_REQUEST as int start with 100000 increment by 1


insert into Settings(Id, MinInterval, LimitRequestCount,  MaxRequestFileCount,  MaxRequestFileSize,  MaxFileNameLength,
   StartWorkDay, EndWorkDay, StartLunchBreak, EndLunchBreak,
   MinCountTransferDay, MaxCountTransferDay) 
values(1, 1, 10, 5, 500, 200, 9, 18, 13, 14, 3, 30);

insert into ObjectType(Id, Name, Soft, Archive, CountHour, DeadlineHour) values(1, 'Сопровождение ПО',				1, 0, 16, 4);
insert into ObjectType(Id, Name, Soft, Archive, CountHour, DeadlineHour) values(2, 'ТО ВТ',							0, 0, 8, 2);
insert into ObjectType(Id, Name, Soft, Archive, CountHour, DeadlineHour) values(3, 'ТО КМТ',						0, 0, 8, 2);
insert into ObjectType(Id, Name, Soft, Archive, CountHour, DeadlineHour) values(4, 'Сетевое администрирование',		0, 0, 8, 2);

insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(1, 'ПП "Галактика"',			1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(2, 'ПП "1C: Кадры"',			1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(3, 'ПП "1C: Бухгалтерия"',	1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(4, 'ПП "Компас"',			1, 0);

insert into Manufacturer(Id, Name) values(1,'BROTHER');
insert into Manufacturer(Id, Name) values(2,'BZB');
insert into Manufacturer(Id, Name) values(3,'CANON');
insert into Manufacturer(Id, Name) values(4,'EPSON');
insert into Manufacturer(Id, Name) values(5,'HEWLETT-PACKARD');
insert into Manufacturer(Id, Name) values(6,'KONICA MINOLTA');
insert into Manufacturer(Id, Name) values(7,'KYOCERA');
insert into Manufacturer(Id, Name) values(8,'LEXMARK');
insert into Manufacturer(Id, Name) values(9,'MB');
insert into Manufacturer(Id, Name) values(10,'OKI');
insert into Manufacturer(Id, Name) values(11,'PANASONIC');
insert into Manufacturer(Id, Name) values(12,'RICOH');
insert into Manufacturer(Id, Name) values(13,'SAMSUNG');
insert into Manufacturer(Id, Name) values(14,'SHARP');
insert into Manufacturer(Id, Name) values(15,'XEROX');
insert into Manufacturer(Id, Name) values(16,'SONY');

insert into HardType(Id, Name) values(1,	'ПРИНТЕР');
insert into HardType(Id, Name) values(2,	'МФУ');
insert into HardType(Id, Name) values(3,	'КОПИР');
insert into HardType(Id, Name) values(4,	'ПЛОТТЕР');
insert into HardType(Id, Name) values(5,	'ФАКС');
insert into HardType(Id, Name) values(6,	'МОНИТОР');
insert into HardType(Id, Name) values(7,	'СКАНЕР');
insert into HardType(Id, Name) values(8,	'КЛАВИАТУРА');
insert into HardType(Id, Name) values(9,	'МЫШЬ');
insert into HardType(Id, Name) values(10,	'СИСТЕМНЫЙ БЛОК');
insert into HardType(Id, Name) values(11,	'НОУТБУК');

insert into Model(Id, ManufacturerId, Name) values(1,	1,	'Принтер HL-2030R');
insert into Model(Id, ManufacturerId, Name) values(2,	1,	'Принтер HL-2040R');
insert into Model(Id, ManufacturerId, Name) values(3,	1,	'Принтер HL-2070NR');
insert into Model(Id, ManufacturerId, Name) values(4,	1,	'Принтер HL-5130');
insert into Model(Id, ManufacturerId, Name) values(5,	1,	'Принтер HL-5140');
insert into Model(Id, ManufacturerId, Name) values(6,	1,	'Принтер HL-5150D');
insert into Model(Id, ManufacturerId, Name) values(7,	1,	'Принтер HL-5170DN');
insert into Model(Id, ManufacturerId, Name) values(8,	1,	'Принтер HL-5240');
insert into Model(Id, ManufacturerId, Name) values(9,	1,	'Принтер HL-5250DN');
insert into Model(Id, ManufacturerId, Name) values(10,	1,	'Принтер HL-5270DN');
insert into Model(Id, ManufacturerId, Name) values(11,	1,	'МФУ DCP-8040');
insert into Model(Id, ManufacturerId, Name) values(12,	1,	'МФУ MFC-8440');
insert into Model(Id, ManufacturerId, Name) values(13,	1,	'МФУ MFC-8840D');
insert into Model(Id, ManufacturerId, Name) values(14,	1,	'МФУ MFC-8840DN');
insert into Model(Id, ManufacturerId, Name) values(15,	1,	'МФУ MFC-9180');

insert into Model(Id, ManufacturerId, Name) values(16,	2,	'Копир 1360');
insert into Model(Id, ManufacturerId, Name) values(17,	2,	'Копир 1560');

insert into Model(Id, ManufacturerId, Name) values(18,	3,	'Принтер BJC-2000');
insert into Model(Id, ManufacturerId, Name) values(19,	3,	'Принтер BJC-210');
insert into Model(Id, ManufacturerId, Name) values(20,	3,	'Принтер BJC-2100');
insert into Model(Id, ManufacturerId, Name) values(21,	3,	'Принтер BJC-230');
insert into Model(Id, ManufacturerId, Name) values(22,	3,	'Принтер BJC-240');
insert into Model(Id, ManufacturerId, Name) values(23,	3,	'Принтер BJC-250');
insert into Model(Id, ManufacturerId, Name) values(24,	3,	'Принтер BJC-3000');
insert into Model(Id, ManufacturerId, Name) values(25,	3,	'Принтер BJC-4000');
insert into Model(Id, ManufacturerId, Name) values(26,	3,	'Принтер BJC-4100');
insert into Model(Id, ManufacturerId, Name) values(27,	3,	'Принтер BJC-4200');
insert into Model(Id, ManufacturerId, Name) values(28,	3,	'Принтер BJC-4300');
insert into Model(Id, ManufacturerId, Name) values(29,	3,	'Принтер BJC-4400');
insert into Model(Id, ManufacturerId, Name) values(30,	3,	'Принтер BJC-4550');
insert into Model(Id, ManufacturerId, Name) values(31,	3,	'Принтер BJC-4650');
insert into Model(Id, ManufacturerId, Name) values(32,	3,	'Принтер BJC-5100');
insert into Model(Id, ManufacturerId, Name) values(33,	3,	'Принтер BJC-5500');
insert into Model(Id, ManufacturerId, Name) values(34,	3,	'Принтер BJC-6000');
insert into Model(Id, ManufacturerId, Name) values(35,	3,	'Принтер BJC-6100');
insert into Model(Id, ManufacturerId, Name) values(36,	3,	'Принтер BJC-6200');
insert into Model(Id, ManufacturerId, Name) values(37,	3,	'Принтер BJC-6500');
insert into Model(Id, ManufacturerId, Name) values(38,	3,	'Принтер i250');
insert into Model(Id, ManufacturerId, Name) values(39,	3,	'Принтер i320');
insert into Model(Id, ManufacturerId, Name) values(40,	3,	'Принтер i350');
insert into Model(Id, ManufacturerId, Name) values(41,	3,	'Принтер i550');
insert into Model(Id, ManufacturerId, Name) values(42,	3,	'Принтер i560');
insert into Model(Id, ManufacturerId, Name) values(43,	3,	'Принтер i6500');
insert into Model(Id, ManufacturerId, Name) values(44,	3,	'Принтер i850');
insert into Model(Id, ManufacturerId, Name) values(45,	3,	'Принтер i950');
insert into Model(Id, ManufacturerId, Name) values(46,	3,	'Принтер i965');
insert into Model(Id, ManufacturerId, Name) values(47,	3,	'Принтер LBP 1120');
insert into Model(Id, ManufacturerId, Name) values(48,	3,	'Принтер LBP 1210');
insert into Model(Id, ManufacturerId, Name) values(49,	3,	'Принтер LBP 2900');
insert into Model(Id, ManufacturerId, Name) values(50,	3,	'Принтер LBP 3000');
insert into Model(Id, ManufacturerId, Name) values(51,	3,	'Принтер LBP 3200');
insert into Model(Id, ManufacturerId, Name) values(52,	3,	'Принтер LBP 460');
insert into Model(Id, ManufacturerId, Name) values(53,	3,	'Принтер LBP 465');
insert into Model(Id, ManufacturerId, Name) values(54,	3,	'Принтер LBP 660');
insert into Model(Id, ManufacturerId, Name) values(55,	3,	'Принтер LBP 800');
insert into Model(Id, ManufacturerId, Name) values(56,	3,	'Принтер LBP 810');
insert into Model(Id, ManufacturerId, Name) values(57,	3,	'Принтер Pixma i865');
insert into Model(Id, ManufacturerId, Name) values(58,	3,	'Принтер Pixma iP1000');
insert into Model(Id, ManufacturerId, Name) values(59,	3,	'Принтер Pixma iP1200');
insert into Model(Id, ManufacturerId, Name) values(60,	3,	'Принтер Pixma iP1500');
insert into Model(Id, ManufacturerId, Name) values(61,	3,	'Принтер Pixma iP1600');
insert into Model(Id, ManufacturerId, Name) values(62,	3,	'Принтер Pixma iP2000');
insert into Model(Id, ManufacturerId, Name) values(63,	3,	'Принтер Pixma iP2200');
insert into Model(Id, ManufacturerId, Name) values(64,	3,	'Принтер Pixma iP3000');
insert into Model(Id, ManufacturerId, Name) values(65,	3,	'Принтер Pixma iP4000');
insert into Model(Id, ManufacturerId, Name) values(66,	3,	'Принтер Pixma iP5000');
insert into Model(Id, ManufacturerId, Name) values(67,	3,	'Принтер Pixma iP6210D');
insert into Model(Id, ManufacturerId, Name) values(68,	3,	'Принтер Pixma iP6220D');
insert into Model(Id, ManufacturerId, Name) values(69,	3,	'Принтер S100');
insert into Model(Id, ManufacturerId, Name) values(70,	3,	'Принтер S200');
insert into Model(Id, ManufacturerId, Name) values(71,	3,	'Принтер S200x');
insert into Model(Id, ManufacturerId, Name) values(72,	3,	'Принтер S300');
insert into Model(Id, ManufacturerId, Name) values(73,	3,	'Принтер S330 Photo');
insert into Model(Id, ManufacturerId, Name) values(74,	3,	'Принтер S400');
insert into Model(Id, ManufacturerId, Name) values(75,	3,	'Принтер S450');
insert into Model(Id, ManufacturerId, Name) values(76,	3,	'Принтер S4500');
insert into Model(Id, ManufacturerId, Name) values(77,	3,	'Принтер S500');
insert into Model(Id, ManufacturerId, Name) values(78,	3,	'Принтер S520');
insert into Model(Id, ManufacturerId, Name) values(79,	3,	'Принтер S530D');
insert into Model(Id, ManufacturerId, Name) values(80,	3,	'Принтер S600');
insert into Model(Id, ManufacturerId, Name) values(81,	3,	'Принтер S630');
insert into Model(Id, ManufacturerId, Name) values(82,	3,	'Принтер S6300');
insert into Model(Id, ManufacturerId, Name) values(83,	3,	'Принтер S750');
insert into Model(Id, ManufacturerId, Name) values(84,	3,	'Принтер S800');
insert into Model(Id, ManufacturerId, Name) values(85,	3,	'Принтер S820');
insert into Model(Id, ManufacturerId, Name) values(86,	3,	'Принтер S820D');
insert into Model(Id, ManufacturerId, Name) values(87,	3,	'Принтер S830D');
insert into Model(Id, ManufacturerId, Name) values(88,	3,	'Факс L-200');
insert into Model(Id, ManufacturerId, Name) values(89,	3,	'Факс L-220');
insert into Model(Id, ManufacturerId, Name) values(90,	3,	'Факс L-240');
insert into Model(Id, ManufacturerId, Name) values(91,	3,	'Факс L-250');
insert into Model(Id, ManufacturerId, Name) values(92,	3,	'Факс L-290');
insert into Model(Id, ManufacturerId, Name) values(93,	3,	'Факс L-300');
insert into Model(Id, ManufacturerId, Name) values(94,	3,	'Копир FC-100');
insert into Model(Id, ManufacturerId, Name) values(95,	3,	'Копир FC-108');
insert into Model(Id, ManufacturerId, Name) values(96,	3,	'Копир FC-120');
insert into Model(Id, ManufacturerId, Name) values(97,	3,	'Копир FC-128');
insert into Model(Id, ManufacturerId, Name) values(98,	3,	'Копир FC-200');
insert into Model(Id, ManufacturerId, Name) values(99,	3,	'Копир FC-206');
insert into Model(Id, ManufacturerId, Name) values(100,	3,	'Копир FC-210');
insert into Model(Id, ManufacturerId, Name) values(101,	3,	'Копир FC-220');
insert into Model(Id, ManufacturerId, Name) values(102,	3,	'Копир FC-226');
insert into Model(Id, ManufacturerId, Name) values(103,	3,	'Копир FC-228');
insert into Model(Id, ManufacturerId, Name) values(104,	3,	'Копир FC-230');
insert into Model(Id, ManufacturerId, Name) values(105,	3,	'Копир FC-336');
insert into Model(Id, ManufacturerId, Name) values(106,	3,	'Копир FC-500');
insert into Model(Id, ManufacturerId, Name) values(107,	3,	'Копир IR-1210');
insert into Model(Id, ManufacturerId, Name) values(108,	3,	'Копир IR-1510');
insert into Model(Id, ManufacturerId, Name) values(109,	3,	'Копир IR-1530');
insert into Model(Id, ManufacturerId, Name) values(110,	3,	'Копир iR2016J');
insert into Model(Id, ManufacturerId, Name) values(111,	3,	'Копир NP 1215');
insert into Model(Id, ManufacturerId, Name) values(112,	3,	'Копир NP 1550');
insert into Model(Id, ManufacturerId, Name) values(113,	3,	'Копир NP 6012');
insert into Model(Id, ManufacturerId, Name) values(114,	3,	'Копир NP 6112');
insert into Model(Id, ManufacturerId, Name) values(115,	3,	'Копир NP 6212');
insert into Model(Id, ManufacturerId, Name) values(116,	3,	'Копир NP 6216');
insert into Model(Id, ManufacturerId, Name) values(117,	3,	'Копир NP 6220');
insert into Model(Id, ManufacturerId, Name) values(118,	3,	'Копир NP 6312');
insert into Model(Id, ManufacturerId, Name) values(119,	3,	'Копир NP 6317');
insert into Model(Id, ManufacturerId, Name) values(120,	3,	'Копир NP 6416');
insert into Model(Id, ManufacturerId, Name) values(121,	3,	'Копир NP 6512');
insert into Model(Id, ManufacturerId, Name) values(122,	3,	'Копир NP 6612');
insert into Model(Id, ManufacturerId, Name) values(123,	3,	'Копир NP 7161');
insert into Model(Id, ManufacturerId, Name) values(124,	3,	'Копир PC-220');
insert into Model(Id, ManufacturerId, Name) values(125,	3,	'Копир PC-700');
insert into Model(Id, ManufacturerId, Name) values(126,	3,	'Копир PC-860');
insert into Model(Id, ManufacturerId, Name) values(127,	3,	'Копир PC-900');
insert into Model(Id, ManufacturerId, Name) values(128,	3,	'МФУ FAX-L380');
insert into Model(Id, ManufacturerId, Name) values(129,	3,	'МФУ FAX-L380S');
insert into Model(Id, ManufacturerId, Name) values(130,	3,	'МФУ FAX-L390');
insert into Model(Id, ManufacturerId, Name) values(131,	3,	'МФУ FAX-L400');
insert into Model(Id, ManufacturerId, Name) values(132,	3,	'МФУ iR C624');
insert into Model(Id, ManufacturerId, Name) values(133,	3,	'МФУ IR-1600');
insert into Model(Id, ManufacturerId, Name) values(134,	3,	'МФУ IR-2000');
insert into Model(Id, ManufacturerId, Name) values(135,	3,	'МФУ IR-2200');
insert into Model(Id, ManufacturerId, Name) values(136,	3,	'МФУ IR-2800');
insert into Model(Id, ManufacturerId, Name) values(137,	3,	'МФУ IR-3300');
insert into Model(Id, ManufacturerId, Name) values(138,	3,	'МФУ iR2016');
insert into Model(Id, ManufacturerId, Name) values(139,	3,	'МФУ iR2016i');
insert into Model(Id, ManufacturerId, Name) values(140,	3,	'МФУ iR2020');
insert into Model(Id, ManufacturerId, Name) values(141,	3,	'МФУ iR2020i');
insert into Model(Id, ManufacturerId, Name) values(142,	3,	'МФУ L-280');
insert into Model(Id, ManufacturerId, Name) values(143,	3,	'МФУ L-295');
insert into Model(Id, ManufacturerId, Name) values(144,	3,	'МФУ L-350');
insert into Model(Id, ManufacturerId, Name) values(145,	3,	'МФУ L-360');
insert into Model(Id, ManufacturerId, Name) values(146,	3,	'МФУ MF3110');
insert into Model(Id, ManufacturerId, Name) values(147,	3,	'МФУ MF5630');
insert into Model(Id, ManufacturerId, Name) values(148,	3,	'МФУ MF5650');
insert into Model(Id, ManufacturerId, Name) values(149,	3,	'МФУ MF5730');
insert into Model(Id, ManufacturerId, Name) values(150,	3,	'МФУ MF5750');
insert into Model(Id, ManufacturerId, Name) values(151,	3,	'МФУ MF5770');
insert into Model(Id, ManufacturerId, Name) values(152,	3,	'МФУ PC-D320');
insert into Model(Id, ManufacturerId, Name) values(153,	3,	'МФУ PC-D340');
insert into Model(Id, ManufacturerId, Name) values(154,	3,	'МФУ Pixma MP110');
insert into Model(Id, ManufacturerId, Name) values(155,	3,	'МФУ Pixma MP130');
insert into Model(Id, ManufacturerId, Name) values(156,	3,	'МФУ Pixma MP150');
insert into Model(Id, ManufacturerId, Name) values(157,	3,	'МФУ Pixma MP170');
insert into Model(Id, ManufacturerId, Name) values(158,	3,	'МФУ Pixma MP450');
insert into Model(Id, ManufacturerId, Name) values(159,	3,	'МФУ Pixma MP750');
insert into Model(Id, ManufacturerId, Name) values(160,	3,	'МФУ Pixma MP760');
insert into Model(Id, ManufacturerId, Name) values(161,	3,	'МФУ Pixma MP780');
insert into Model(Id, ManufacturerId, Name) values(162,	3,	'МФУ SmartBase MP700Photo');
insert into Model(Id, ManufacturerId, Name) values(163,	3,	'МФУ SmartBase MP730Photo');
insert into Model(Id, ManufacturerId, Name) values(164,	3,	'МФУ SmartBase MPC190');
insert into Model(Id, ManufacturerId, Name) values(165,	3,	'МФУ SmartBase MPC200');
insert into Model(Id, ManufacturerId, Name) values(166,	3,	'МФУ SmartBase MPC400');
insert into Model(Id, ManufacturerId, Name) values(167,	3,	'МФУ SmartBase MPC600F');


insert into Model(Id, ManufacturerId, Name) values(168,4, 'Принтер AcuLaser C1900');
insert into Model(Id, ManufacturerId, Name) values(169,4, 'Принтер AcuLaser C1900D');
insert into Model(Id, ManufacturerId, Name) values(170,4, 'Принтер AcuLaser C1900PS');
insert into Model(Id, ManufacturerId, Name) values(171,4, 'Принтер AcuLaser C1900S');
insert into Model(Id, ManufacturerId, Name) values(172,4, 'Принтер AcuLaser C1900WiFi');
insert into Model(Id, ManufacturerId, Name) values(173,4, 'Принтер AcuLaser C3000');
insert into Model(Id, ManufacturerId, Name) values(174,4, 'Принтер AcuLaser C3000N');
insert into Model(Id, ManufacturerId, Name) values(175,4, 'Принтер AcuLaser C4100');
insert into Model(Id, ManufacturerId, Name) values(176,4, 'Принтер AcuLaser C4100PS');
insert into Model(Id, ManufacturerId, Name) values(177,4, 'Принтер AcuLaser C4100T');
insert into Model(Id, ManufacturerId, Name) values(178,4, 'Принтер AcuLaser C900');
insert into Model(Id, ManufacturerId, Name) values(179,4, 'Принтер AcuLaser C900N');
insert into Model(Id, ManufacturerId, Name) values(180,4, 'Принтер Epson FX-1170');
insert into Model(Id, ManufacturerId, Name) values(181,4, 'Принтер Epson FX-1180');
insert into Model(Id, ManufacturerId, Name) values(182,4, 'Принтер Epson FX-1180+');
insert into Model(Id, ManufacturerId, Name) values(183,4, 'Принтер Epson LX-1170');
insert into Model(Id, ManufacturerId, Name) values(184,4, 'Принтер FX-890');
insert into Model(Id, ManufacturerId, Name) values(185,4, 'Принтер LQ-100');
insert into Model(Id, ManufacturerId, Name) values(186,4, 'Принтер LX-1050');
insert into Model(Id, ManufacturerId, Name) values(187,4, 'Принтер LX-300+');
insert into Model(Id, ManufacturerId, Name) values(188,4, 'Принтер Stylus 1000');
insert into Model(Id, ManufacturerId, Name) values(189,4, 'Принтер Stylus 200');
insert into Model(Id, ManufacturerId, Name) values(190,4, 'Принтер Stylus 300');
insert into Model(Id, ManufacturerId, Name) values(191,4, 'Принтер Stylus 400');
insert into Model(Id, ManufacturerId, Name) values(192,4, 'Принтер Stylus 800');
insert into Model(Id, ManufacturerId, Name) values(193,4, 'Принтер Stylus 800+');
insert into Model(Id, ManufacturerId, Name) values(194,4, 'Принтер Stylus C41');
insert into Model(Id, ManufacturerId, Name) values(195,4, 'Принтер Stylus C43SX');
insert into Model(Id, ManufacturerId, Name) values(196,4, 'Принтер Stylus C43UX');
insert into Model(Id, ManufacturerId, Name) values(197,4, 'Принтер Stylus C45');
insert into Model(Id, ManufacturerId, Name) values(198,4, 'Принтер Stylus C64');
insert into Model(Id, ManufacturerId, Name) values(199,4, 'Принтер Stylus C64 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(200,4, 'Принтер Stylus C66');
insert into Model(Id, ManufacturerId, Name) values(201,4, 'Принтер Stylus C66 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(202,4, 'Принтер Stylus C67 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(203,4, 'Принтер Stylus C84');
insert into Model(Id, ManufacturerId, Name) values(204,4, 'Принтер Stylus C84 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(205,4, 'Принтер Stylus C86');
insert into Model(Id, ManufacturerId, Name) values(206,4, 'Принтер Stylus C86 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(207,4, 'Принтер Stylus C87 Photo Edition');
insert into Model(Id, ManufacturerId, Name) values(208,4, 'Принтер Stylus C87 Plus');
insert into Model(Id, ManufacturerId, Name) values(209,4, 'Принтер Stylus Color');
insert into Model(Id, ManufacturerId, Name) values(210,4, 'Принтер Stylus Color 1160');
insert into Model(Id, ManufacturerId, Name) values(211,4, 'Принтер Stylus Color 1500');
insert into Model(Id, ManufacturerId, Name) values(212,4, 'Принтер Stylus Color 1520');
insert into Model(Id, ManufacturerId, Name) values(213,4, 'Принтер Stylus Color 3000');
insert into Model(Id, ManufacturerId, Name) values(214,4, 'Принтер Stylus Color 740');
insert into Model(Id, ManufacturerId, Name) values(215,4, 'Принтер Stylus Color 760');
insert into Model(Id, ManufacturerId, Name) values(216,4, 'Принтер Stylus Color 820');
insert into Model(Id, ManufacturerId, Name) values(217,4, 'Принтер Stylus Color 860');
insert into Model(Id, ManufacturerId, Name) values(218,4, 'Принтер Stylus Photo 1200');
insert into Model(Id, ManufacturerId, Name) values(219,4, 'Принтер Stylus Photo 1270');
insert into Model(Id, ManufacturerId, Name) values(220,4, 'Принтер Stylus Photo 1290');
insert into Model(Id, ManufacturerId, Name) values(221,4, 'Принтер Stylus Photo 1410');
insert into Model(Id, ManufacturerId, Name) values(222,4, 'Принтер Stylus Photo 2100');
insert into Model(Id, ManufacturerId, Name) values(223,4, 'Принтер Stylus Photo 700');
insert into Model(Id, ManufacturerId, Name) values(224,4, 'Принтер Stylus Photo 750');
insert into Model(Id, ManufacturerId, Name) values(225,4, 'Принтер Stylus Photo 790');
insert into Model(Id, ManufacturerId, Name) values(226,4, 'Принтер Stylus Photo 810');
insert into Model(Id, ManufacturerId, Name) values(227,4, 'Принтер Stylus Photo 830');
insert into Model(Id, ManufacturerId, Name) values(228,4, 'Принтер Stylus Photo 830U');
insert into Model(Id, ManufacturerId, Name) values(229,4, 'Принтер Stylus Photo 870');
insert into Model(Id, ManufacturerId, Name) values(230,4, 'Принтер Stylus Photo 890');
insert into Model(Id, ManufacturerId, Name) values(231,4, 'Принтер Stylus Photo 895');
insert into Model(Id, ManufacturerId, Name) values(232,4, 'Принтер Stylus Photo 900');
insert into Model(Id, ManufacturerId, Name) values(233,4, 'Принтер Stylus Photo 915');
insert into Model(Id, ManufacturerId, Name) values(234,4, 'Принтер Stylus Photo 925');
insert into Model(Id, ManufacturerId, Name) values(235,4, 'Принтер Stylus Photo 935');
insert into Model(Id, ManufacturerId, Name) values(236,4, 'Принтер Stylus Photo 950');
insert into Model(Id, ManufacturerId, Name) values(237,4, 'Принтер Stylus Photo R1800');
insert into Model(Id, ManufacturerId, Name) values(238,4, 'Принтер Stylus Photo R200');
insert into Model(Id, ManufacturerId, Name) values(239,4, 'Принтер Stylus Photo R220');
insert into Model(Id, ManufacturerId, Name) values(240,4, 'Принтер Stylus Photo R270');
insert into Model(Id, ManufacturerId, Name) values(241,4, 'Принтер Stylus Photo R300');
insert into Model(Id, ManufacturerId, Name) values(242,4, 'Принтер Stylus Photo R300ME');
insert into Model(Id, ManufacturerId, Name) values(243,4, 'Принтер Stylus Photo R320');
insert into Model(Id, ManufacturerId, Name) values(244,4, 'Принтер Stylus Photo R340');
insert into Model(Id, ManufacturerId, Name) values(245,4, 'Принтер Stylus Photo R390');
insert into Model(Id, ManufacturerId, Name) values(246,4, 'Принтер Stylus Photo R800');
insert into Model(Id, ManufacturerId, Name) values(247,4, 'Принтер Stylus PRO');
insert into Model(Id, ManufacturerId, Name) values(248,4, 'Принтер Stylus Pro 4000');
insert into Model(Id, ManufacturerId, Name) values(249,4, 'Принтер Stylus Pro 4400');
insert into Model(Id, ManufacturerId, Name) values(250,4, 'Принтер Stylus Pro 4800');
insert into Model(Id, ManufacturerId, Name) values(251,4, 'Принтер Stylus Pro 5000');
insert into Model(Id, ManufacturerId, Name) values(252,4, 'Принтер Stylus PRO XL');
insert into Model(Id, ManufacturerId, Name) values(253,4, 'Плоттер Stylus Pro 10600');
insert into Model(Id, ManufacturerId, Name) values(254,4, 'Плоттер Stylus Pro 7400');
insert into Model(Id, ManufacturerId, Name) values(255,4, 'Плоттер Stylus Pro 7600');
insert into Model(Id, ManufacturerId, Name) values(256,4, 'Плоттер Stylus Pro 7800');
insert into Model(Id, ManufacturerId, Name) values(257,4, 'Плоттер Stylus Pro 9400');
insert into Model(Id, ManufacturerId, Name) values(258,4, 'Плоттер Stylus Pro 9600');
insert into Model(Id, ManufacturerId, Name) values(259,4, 'Плоттер Stylus Pro 9800');
insert into Model(Id, ManufacturerId, Name) values(260,4, 'МФУ Stylus CX3700');
insert into Model(Id, ManufacturerId, Name) values(261,4, 'МФУ Stylus CX4100');
insert into Model(Id, ManufacturerId, Name) values(262,4, 'МФУ Stylus CX4700');
insert into Model(Id, ManufacturerId, Name) values(263,4, 'МФУ Stylus CX6400');
insert into Model(Id, ManufacturerId, Name) values(264,4, 'МФУ Stylus CX6600');
insert into Model(Id, ManufacturerId, Name) values(265,4, 'МФУ Stylus Photo RX500');
insert into Model(Id, ManufacturerId, Name) values(266,4, 'МФУ Stylus Photo RX590');
insert into Model(Id, ManufacturerId, Name) values(267,4, 'МФУ Stylus Photo RX600');
insert into Model(Id, ManufacturerId, Name) values(268,4, 'МФУ Stylus Photo RX620');
insert into Model(Id, ManufacturerId, Name) values(269,4, 'МФУ Stylus Photo RX640');
insert into Model(Id, ManufacturerId, Name) values(270,4, 'МФУ Stylus Photo RX700');

insert into Model(Id, ManufacturerId, Name) values(271,5, 'Принтер Color LaserJet 1500');
insert into Model(Id, ManufacturerId, Name) values(272,5, 'Принтер Color LaserJet 1500L');
insert into Model(Id, ManufacturerId, Name) values(273,5, 'Принтер Color LaserJet 1600');
insert into Model(Id, ManufacturerId, Name) values(274,5, 'Принтер Color LaserJet 2500');
insert into Model(Id, ManufacturerId, Name) values(275,5, 'Принтер Color LaserJet 2500L');
insert into Model(Id, ManufacturerId, Name) values(276,5, 'Принтер Color LaserJet 2500N');
insert into Model(Id, ManufacturerId, Name) values(277,5, 'Принтер Color LaserJet 2550L');
insert into Model(Id, ManufacturerId, Name) values(278,5, 'Принтер Color LaserJet 2550Ln');
insert into Model(Id, ManufacturerId, Name) values(279,5, 'Принтер Color LaserJet 2550n');
insert into Model(Id, ManufacturerId, Name) values(280,5, 'Принтер Color LaserJet 2600n');
insert into Model(Id, ManufacturerId, Name) values(281,5, 'Принтер Color LaserJet 2605');
insert into Model(Id, ManufacturerId, Name) values(282,5, 'Принтер Color LaserJet 2605dn');
insert into Model(Id, ManufacturerId, Name) values(283,5, 'Принтер Color LaserJet 2605dtn');
insert into Model(Id, ManufacturerId, Name) values(284,5, 'Принтер Color LaserJet 3000');
insert into Model(Id, ManufacturerId, Name) values(285,5, 'Принтер Color LaserJet 3000dn');
insert into Model(Id, ManufacturerId, Name) values(286,5, 'Принтер Color LaserJet 3000dtn');
insert into Model(Id, ManufacturerId, Name) values(287,5, 'Принтер Color LaserJet 3000n');
insert into Model(Id, ManufacturerId, Name) values(288,5, 'Принтер Color LaserJet 3500');
insert into Model(Id, ManufacturerId, Name) values(289,5, 'Принтер Color LaserJet 3500n');
insert into Model(Id, ManufacturerId, Name) values(290,5, 'Принтер Color LaserJet 3550');
insert into Model(Id, ManufacturerId, Name) values(291,5, 'Принтер Color LaserJet 3550n');
insert into Model(Id, ManufacturerId, Name) values(292,5, 'Принтер Color LaserJet 3600');
insert into Model(Id, ManufacturerId, Name) values(293,5, 'Принтер Color LaserJet 3600dn');
insert into Model(Id, ManufacturerId, Name) values(294,5, 'Принтер Color LaserJet 3600n');
insert into Model(Id, ManufacturerId, Name) values(295,5, 'Принтер Color LaserJet 3700');
insert into Model(Id, ManufacturerId, Name) values(296,5, 'Принтер Color LaserJet 3700dn');
insert into Model(Id, ManufacturerId, Name) values(297,5, 'Принтер Color LaserJet 3700dtn');
insert into Model(Id, ManufacturerId, Name) values(298,5, 'Принтер Color LaserJet 3700n');
insert into Model(Id, ManufacturerId, Name) values(299,5, 'Принтер Color LaserJet 3800');
insert into Model(Id, ManufacturerId, Name) values(300,5, 'Принтер Color LaserJet 3800dn');
insert into Model(Id, ManufacturerId, Name) values(301,5, 'Принтер Color LaserJet 3800dtn');
insert into Model(Id, ManufacturerId, Name) values(302,5, 'Принтер Color LaserJet 3800n');
insert into Model(Id, ManufacturerId, Name) values(303,5, 'Принтер Color LaserJet 4500');
insert into Model(Id, ManufacturerId, Name) values(304,5, 'Принтер Color LaserJet 4500dn');
insert into Model(Id, ManufacturerId, Name) values(305,5, 'Принтер Color LaserJet 4500n');
insert into Model(Id, ManufacturerId, Name) values(306,5, 'Принтер Color LaserJet 4550');
insert into Model(Id, ManufacturerId, Name) values(307,5, 'Принтер Color LaserJet 4550n');
insert into Model(Id, ManufacturerId, Name) values(308,5, 'Принтер Color LaserJet 4600');
insert into Model(Id, ManufacturerId, Name) values(309,5, 'Принтер Color LaserJet 4600dn');
insert into Model(Id, ManufacturerId, Name) values(310,5, 'Принтер Color LaserJet 4600dtn');
insert into Model(Id, ManufacturerId, Name) values(311,5, 'Принтер Color LaserJet 4600hdn');
insert into Model(Id, ManufacturerId, Name) values(312,5, 'Принтер Color LaserJet 4600n');
insert into Model(Id, ManufacturerId, Name) values(313,5, 'Принтер Color LaserJet 4650');
insert into Model(Id, ManufacturerId, Name) values(314,5, 'Принтер Color LaserJet 4650dn');
insert into Model(Id, ManufacturerId, Name) values(315,5, 'Принтер Color LaserJet 4650dtn');
insert into Model(Id, ManufacturerId, Name) values(316,5, 'Принтер Color LaserJet 4650hdn');
insert into Model(Id, ManufacturerId, Name) values(317,5, 'Принтер Color LaserJet 4650n');
insert into Model(Id, ManufacturerId, Name) values(318,5, 'Принтер Color LaserJet 4700');
insert into Model(Id, ManufacturerId, Name) values(319,5, 'Принтер Color LaserJet 4700dn');
insert into Model(Id, ManufacturerId, Name) values(320,5, 'Принтер Color LaserJet 4700dtn');
insert into Model(Id, ManufacturerId, Name) values(321,5, 'Принтер Color LaserJet 4700n');
insert into Model(Id, ManufacturerId, Name) values(322,5, 'Принтер Color LaserJet 5');
insert into Model(Id, ManufacturerId, Name) values(323,5, 'Принтер Color LaserJet 5500');
insert into Model(Id, ManufacturerId, Name) values(324,5, 'Принтер Color LaserJet 5500dn');
insert into Model(Id, ManufacturerId, Name) values(325,5, 'Принтер Color LaserJet 5500dtn');
insert into Model(Id, ManufacturerId, Name) values(326,5, 'Принтер Color LaserJet 5500hdn');
insert into Model(Id, ManufacturerId, Name) values(327,5, 'Принтер Color LaserJet 5500n');
insert into Model(Id, ManufacturerId, Name) values(328,5, 'Принтер Color LaserJet 5550');
insert into Model(Id, ManufacturerId, Name) values(329,5, 'Принтер Color LaserJet 5550dn');
insert into Model(Id, ManufacturerId, Name) values(330,5, 'Принтер Color LaserJet 5550dtn');
insert into Model(Id, ManufacturerId, Name) values(331,5, 'Принтер Color LaserJet 5550hdn');
insert into Model(Id, ManufacturerId, Name) values(332,5, 'Принтер Color LaserJet 5550n');
insert into Model(Id, ManufacturerId, Name) values(333,5, 'Принтер Color LaserJet 5M');
insert into Model(Id, ManufacturerId, Name) values(334,5, 'Принтер Color LaserJet 5N');
insert into Model(Id, ManufacturerId, Name) values(335,5, 'Принтер DeskJet 1100c');
insert into Model(Id, ManufacturerId, Name) values(336,5, 'Принтер DeskJet 1120c');
insert into Model(Id, ManufacturerId, Name) values(337,5, 'Принтер DeskJet 1180');
insert into Model(Id, ManufacturerId, Name) values(338,5, 'Принтер DeskJet 1220c');
insert into Model(Id, ManufacturerId, Name) values(339,5, 'Принтер DeskJet 1280');
insert into Model(Id, ManufacturerId, Name) values(340,5, 'Принтер DeskJet 3650');
insert into Model(Id, ManufacturerId, Name) values(341,5, 'Принтер DeskJet 3845');
insert into Model(Id, ManufacturerId, Name) values(342,5, 'Принтер DeskJet 460c');
insert into Model(Id, ManufacturerId, Name) values(343,5, 'Принтер DeskJet 460cb');
insert into Model(Id, ManufacturerId, Name) values(344,5, 'Принтер DeskJet 460wbt');
insert into Model(Id, ManufacturerId, Name) values(345,5, 'Принтер DeskJet 5550');
insert into Model(Id, ManufacturerId, Name) values(346,5, 'Принтер DeskJet 5652');
insert into Model(Id, ManufacturerId, Name) values(347,5, 'Принтер DeskJet 5743');
insert into Model(Id, ManufacturerId, Name) values(348,5, 'Принтер DeskJet 610c');
insert into Model(Id, ManufacturerId, Name) values(349,5, 'Принтер DeskJet 615c');
insert into Model(Id, ManufacturerId, Name) values(350,5, 'Принтер DeskJet 640c');
insert into Model(Id, ManufacturerId, Name) values(351,5, 'Принтер DeskJet 6543');
insert into Model(Id, ManufacturerId, Name) values(352,5, 'Принтер DeskJet 656c');
insert into Model(Id, ManufacturerId, Name) values(353,5, 'Принтер DeskJet 6623');
insert into Model(Id, ManufacturerId, Name) values(354,5, 'Принтер DeskJet 670C');
insert into Model(Id, ManufacturerId, Name) values(355,5, 'Принтер DeskJet 6843');
insert into Model(Id, ManufacturerId, Name) values(356,5, 'Принтер DeskJet 690C');
insert into Model(Id, ManufacturerId, Name) values(357,5, 'Принтер DeskJet 695С');
insert into Model(Id, ManufacturerId, Name) values(358,5, 'Принтер DeskJet 710c');
insert into Model(Id, ManufacturerId, Name) values(359,5, 'Принтер DeskJet 720c');
insert into Model(Id, ManufacturerId, Name) values(360,5, 'Принтер DeskJet 815c');
insert into Model(Id, ManufacturerId, Name) values(361,5, 'Принтер DeskJet 880c');
insert into Model(Id, ManufacturerId, Name) values(362,5, 'Принтер DeskJet 890c');
insert into Model(Id, ManufacturerId, Name) values(363,5, 'Принтер DeskJet 895cxi');
insert into Model(Id, ManufacturerId, Name) values(364,5, 'Принтер DeskJet 9300');
insert into Model(Id, ManufacturerId, Name) values(365,5, 'Принтер DeskJet 930c');
insert into Model(Id, ManufacturerId, Name) values(366,5, 'Принтер DeskJet 950c');
insert into Model(Id, ManufacturerId, Name) values(367,5, 'Принтер DeskJet 959c');
insert into Model(Id, ManufacturerId, Name) values(368,5, 'Принтер DeskJet 960c');
insert into Model(Id, ManufacturerId, Name) values(369,5, 'Принтер DeskJet 970cxi');
insert into Model(Id, ManufacturerId, Name) values(370,5, 'Принтер DeskJet 9803');
insert into Model(Id, ManufacturerId, Name) values(371,5, 'Принтер DeskJet 9803d');
insert into Model(Id, ManufacturerId, Name) values(372,5, 'Принтер DeskJet 980cxi');
insert into Model(Id, ManufacturerId, Name) values(373,5, 'Принтер DeskJet 990cxi');
insert into Model(Id, ManufacturerId, Name) values(374,5, 'Принтер InkJet CP1700');
insert into Model(Id, ManufacturerId, Name) values(375,5, 'Принтер InkJet CP1700d');
insert into Model(Id, ManufacturerId, Name) values(376,5, 'Принтер InkJet CP1700ps');
insert into Model(Id, ManufacturerId, Name) values(377,5, 'Принтер LaserJet 1000W');
insert into Model(Id, ManufacturerId, Name) values(378,5, 'Принтер LaserJet 1005W');
insert into Model(Id, ManufacturerId, Name) values(379,5, 'Принтер LaserJet 1010');
insert into Model(Id, ManufacturerId, Name) values(380,5, 'Принтер LaserJet 1012');
insert into Model(Id, ManufacturerId, Name) values(381,5, 'Принтер LaserJet 1015');
insert into Model(Id, ManufacturerId, Name) values(382,5, 'Принтер LaserJet 1018');
insert into Model(Id, ManufacturerId, Name) values(383,5, 'Принтер LaserJet 1020');
insert into Model(Id, ManufacturerId, Name) values(384,5, 'Принтер LaserJet 1022');
insert into Model(Id, ManufacturerId, Name) values(385,5, 'Принтер LaserJet 1022N');
insert into Model(Id, ManufacturerId, Name) values(386,5, 'Принтер LaserJet 1022NW');
insert into Model(Id, ManufacturerId, Name) values(387,5, 'Принтер LaserJet 1100');
insert into Model(Id, ManufacturerId, Name) values(388,5, 'Принтер LaserJet 1150');
insert into Model(Id, ManufacturerId, Name) values(389,5, 'Принтер LaserJet 1160');
insert into Model(Id, ManufacturerId, Name) values(390,5, 'Принтер LaserJet 1200');
insert into Model(Id, ManufacturerId, Name) values(391,5, 'Принтер LaserJet 1200N');
insert into Model(Id, ManufacturerId, Name) values(392,5, 'Принтер LaserJet 1300');
insert into Model(Id, ManufacturerId, Name) values(393,5, 'Принтер LaserJet 1300N');
insert into Model(Id, ManufacturerId, Name) values(394,5, 'Принтер LaserJet 1320');
insert into Model(Id, ManufacturerId, Name) values(395,5, 'Принтер LaserJet 1320N');
insert into Model(Id, ManufacturerId, Name) values(396,5, 'Принтер LaserJet 1320NW');
insert into Model(Id, ManufacturerId, Name) values(397,5, 'Принтер LaserJet 1320TN');
insert into Model(Id, ManufacturerId, Name) values(398,5, 'Принтер LaserJet 2100');
insert into Model(Id, ManufacturerId, Name) values(399,5, 'Принтер LaserJet 2100m');
insert into Model(Id, ManufacturerId, Name) values(400,5, 'Принтер LaserJet 2100tn');
insert into Model(Id, ManufacturerId, Name) values(401,5, 'Принтер LaserJet 2200');
insert into Model(Id, ManufacturerId, Name) values(402,5, 'Принтер LaserJet 2200d');
insert into Model(Id, ManufacturerId, Name) values(403,5, 'Принтер LaserJet 2200dn');
insert into Model(Id, ManufacturerId, Name) values(404,5, 'Принтер LaserJet 2200dt');
insert into Model(Id, ManufacturerId, Name) values(405,5, 'Принтер LaserJet 2200dtn');
insert into Model(Id, ManufacturerId, Name) values(406,5, 'Принтер LaserJet 2300');
insert into Model(Id, ManufacturerId, Name) values(407,5, 'Принтер LaserJet 2300D');
insert into Model(Id, ManufacturerId, Name) values(408,5, 'Принтер LaserJet 2300DN');
insert into Model(Id, ManufacturerId, Name) values(409,5, 'Принтер LaserJet 2300DTN');
insert into Model(Id, ManufacturerId, Name) values(410,5, 'Принтер LaserJet 2300L');
insert into Model(Id, ManufacturerId, Name) values(411,5, 'Принтер LaserJet 2300N');
insert into Model(Id, ManufacturerId, Name) values(412,5, 'Принтер LaserJet 2410');
insert into Model(Id, ManufacturerId, Name) values(413,5, 'Принтер LaserJet 2420');
insert into Model(Id, ManufacturerId, Name) values(414,5, 'Принтер LaserJet 2420D');
insert into Model(Id, ManufacturerId, Name) values(415,5, 'Принтер LaserJet 2420DN');
insert into Model(Id, ManufacturerId, Name) values(416,5, 'Принтер LaserJet 2420N');
insert into Model(Id, ManufacturerId, Name) values(417,5, 'Принтер LaserJet 2430DTN');
insert into Model(Id, ManufacturerId, Name) values(418,5, 'Принтер LaserJet 2430N');
insert into Model(Id, ManufacturerId, Name) values(419,5, 'Принтер LaserJet 2430T');
insert into Model(Id, ManufacturerId, Name) values(420,5, 'Принтер LaserJet 2430TN');
insert into Model(Id, ManufacturerId, Name) values(421,5, 'Принтер LaserJet 4000');
insert into Model(Id, ManufacturerId, Name) values(422,5, 'Принтер LaserJet 4000N');
insert into Model(Id, ManufacturerId, Name) values(423,5, 'Принтер LaserJet 4000T');
insert into Model(Id, ManufacturerId, Name) values(424,5, 'Принтер LaserJet 4000TN');
insert into Model(Id, ManufacturerId, Name) values(425,5, 'Принтер LaserJet 4050');
insert into Model(Id, ManufacturerId, Name) values(426,5, 'Принтер LaserJet 4050n');
insert into Model(Id, ManufacturerId, Name) values(427,5, 'Принтер LaserJet 4050tn');
insert into Model(Id, ManufacturerId, Name) values(428,5, 'Принтер LaserJet 4100');
insert into Model(Id, ManufacturerId, Name) values(429,5, 'Принтер LaserJet 4100dtn');
insert into Model(Id, ManufacturerId, Name) values(430,5, 'Принтер LaserJet 4100n');
insert into Model(Id, ManufacturerId, Name) values(431,5, 'Принтер LaserJet 4100tn');
insert into Model(Id, ManufacturerId, Name) values(432,5, 'Принтер LaserJet 4200');
insert into Model(Id, ManufacturerId, Name) values(433,5, 'Принтер LaserJet 4200DTN');
insert into Model(Id, ManufacturerId, Name) values(434,5, 'Принтер LaserJet 4200N');
insert into Model(Id, ManufacturerId, Name) values(435,5, 'Принтер LaserJet 4200TN');
insert into Model(Id, ManufacturerId, Name) values(436,5, 'Принтер LaserJet 4240n');
insert into Model(Id, ManufacturerId, Name) values(437,5, 'Принтер LaserJet 4250');
insert into Model(Id, ManufacturerId, Name) values(438,5, 'Принтер LaserJet 4250dtn');
insert into Model(Id, ManufacturerId, Name) values(439,5, 'Принтер LaserJet 4250dtnsl');
insert into Model(Id, ManufacturerId, Name) values(440,5, 'Принтер LaserJet 4250n');
insert into Model(Id, ManufacturerId, Name) values(441,5, 'Принтер LaserJet 4250tn');
insert into Model(Id, ManufacturerId, Name) values(442,5, 'Принтер LaserJet 4300');
insert into Model(Id, ManufacturerId, Name) values(443,5, 'Принтер LaserJet 4300dtn');
insert into Model(Id, ManufacturerId, Name) values(444,5, 'Принтер LaserJet 4300n');
insert into Model(Id, ManufacturerId, Name) values(445,5, 'Принтер LaserJet 4300tn');
insert into Model(Id, ManufacturerId, Name) values(446,5, 'Принтер LaserJet 4350');
insert into Model(Id, ManufacturerId, Name) values(447,5, 'Принтер LaserJet 4350dtn');
insert into Model(Id, ManufacturerId, Name) values(448,5, 'Принтер LaserJet 4350dtnsl');
insert into Model(Id, ManufacturerId, Name) values(449,5, 'Принтер LaserJet 4350n');
insert into Model(Id, ManufacturerId, Name) values(450,5, 'Принтер LaserJet 4350tn');
insert into Model(Id, ManufacturerId, Name) values(451,5, 'Принтер LaserJet 4L');
insert into Model(Id, ManufacturerId, Name) values(452,5, 'Принтер LaserJet 4ML');
insert into Model(Id, ManufacturerId, Name) values(453,5, 'Принтер LaserJet 4MP');
insert into Model(Id, ManufacturerId, Name) values(454,5, 'Принтер LaserJet 4MV');
insert into Model(Id, ManufacturerId, Name) values(455,5, 'Принтер LaserJet 4P');
insert into Model(Id, ManufacturerId, Name) values(456,5, 'Принтер LaserJet 4V');
insert into Model(Id, ManufacturerId, Name) values(457,5, 'Принтер LaserJet 5000');
insert into Model(Id, ManufacturerId, Name) values(458,5, 'Принтер LaserJet 5000gn');
insert into Model(Id, ManufacturerId, Name) values(459,5, 'Принтер LaserJet 5000n');
insert into Model(Id, ManufacturerId, Name) values(460,5, 'Принтер LaserJet 5100');
insert into Model(Id, ManufacturerId, Name) values(461,5, 'Принтер LaserJet 5100dtn');
insert into Model(Id, ManufacturerId, Name) values(462,5, 'Принтер LaserJet 5100tn');
insert into Model(Id, ManufacturerId, Name) values(463,5, 'Принтер LaserJet 5L');
insert into Model(Id, ManufacturerId, Name) values(464,5, 'Принтер LaserJet 5MP');
insert into Model(Id, ManufacturerId, Name) values(465,5, 'Принтер LaserJet 5P');
insert into Model(Id, ManufacturerId, Name) values(466,5, 'Принтер LaserJet 6L');
insert into Model(Id, ManufacturerId, Name) values(467,5, 'Принтер LaserJet 6MP');
insert into Model(Id, ManufacturerId, Name) values(468,5, 'Принтер LaserJet 6P');
insert into Model(Id, ManufacturerId, Name) values(469,5, 'Плоттер DesignJet 130');
insert into Model(Id, ManufacturerId, Name) values(470,5, 'Плоттер DesignJet 130gp');
insert into Model(Id, ManufacturerId, Name) values(471,5, 'Плоттер DesignJet 130nr');
insert into Model(Id, ManufacturerId, Name) values(472,5, 'Плоттер DesignJet 30');
insert into Model(Id, ManufacturerId, Name) values(473,5, 'Плоттер DesignJet 30gp');
insert into Model(Id, ManufacturerId, Name) values(474,5, 'Плоттер DesignJet 30n');
insert into Model(Id, ManufacturerId, Name) values(475,5, 'Плоттер DesignJet 450C 24"');
insert into Model(Id, ManufacturerId, Name) values(476,5, 'Плоттер DesignJet 450C 42"');
insert into Model(Id, ManufacturerId, Name) values(477,5, 'Плоттер DesignJet 500 24"');
insert into Model(Id, ManufacturerId, Name) values(478,5, 'Плоттер DesignJet 500 42"');
insert into Model(Id, ManufacturerId, Name) values(479,5, 'Плоттер DesignJet 500 PLUS 24"');
insert into Model(Id, ManufacturerId, Name) values(480,5, 'Плоттер DesignJet 500 PLUS 42"');
insert into Model(Id, ManufacturerId, Name) values(481,5, 'Плоттер DesignJet 500PS 24"');
insert into Model(Id, ManufacturerId, Name) values(482,5, 'Плоттер DesignJet 500PS 42"');
insert into Model(Id, ManufacturerId, Name) values(483,5, 'Плоттер DesignJet 500PS PLUS 24"');
insert into Model(Id, ManufacturerId, Name) values(484,5, 'Плоттер DesignJet 500PS PLUS 42"');
insert into Model(Id, ManufacturerId, Name) values(485,5, 'Плоттер DesignJet 90');
insert into Model(Id, ManufacturerId, Name) values(486,5, 'Плоттер DesignJet 90gp');
insert into Model(Id, ManufacturerId, Name) values(487,5, 'Плоттер DesignJet 90r');
insert into Model(Id, ManufacturerId, Name) values(488,5, 'МФУ Color LaserJet 2820');
insert into Model(Id, ManufacturerId, Name) values(489,5, 'МФУ Color LaserJet 2840');
insert into Model(Id, ManufacturerId, Name) values(490,5, 'МФУ LaserJet 1100A');
insert into Model(Id, ManufacturerId, Name) values(491,5, 'МФУ LaserJet 1220');
insert into Model(Id, ManufacturerId, Name) values(492,5, 'МФУ LaserJet 3015');
insert into Model(Id, ManufacturerId, Name) values(493,5, 'МФУ LaserJet 3020');
insert into Model(Id, ManufacturerId, Name) values(494,5, 'МФУ LaserJet 3030');
insert into Model(Id, ManufacturerId, Name) values(495,5, 'МФУ LaserJet 3050');
insert into Model(Id, ManufacturerId, Name) values(496,5, 'МФУ LaserJet 3052');
insert into Model(Id, ManufacturerId, Name) values(497,5, 'МФУ LaserJet 3055');
insert into Model(Id, ManufacturerId, Name) values(498,5, 'МФУ LaserJet 3100');
insert into Model(Id, ManufacturerId, Name) values(499,5, 'МФУ LaserJet 3150');
insert into Model(Id, ManufacturerId, Name) values(500,5, 'МФУ LaserJet 3200');
insert into Model(Id, ManufacturerId, Name) values(501,5, 'МФУ LaserJet 3300mfp');
insert into Model(Id, ManufacturerId, Name) values(502,5, 'МФУ LaserJet 3320mfp');
insert into Model(Id, ManufacturerId, Name) values(503,5, 'МФУ LaserJet 3320n');
insert into Model(Id, ManufacturerId, Name) values(504,5, 'МФУ LaserJet 3330mfp');
insert into Model(Id, ManufacturerId, Name) values(505,5, 'МФУ LaserJet 3380');
insert into Model(Id, ManufacturerId, Name) values(506,5, 'МФУ LaserJet 3390');
insert into Model(Id, ManufacturerId, Name) values(507,5, 'МФУ LaserJet 3392');
insert into Model(Id, ManufacturerId, Name) values(508,5, 'МФУ LaserJet 4100mfp');
insert into Model(Id, ManufacturerId, Name) values(509,5, 'МФУ OfficeJet 6213');
insert into Model(Id, ManufacturerId, Name) values(510,5, 'МФУ OfficeJet 7413');
insert into Model(Id, ManufacturerId, Name) values(511,5, 'МФУ PSC 1110');
insert into Model(Id, ManufacturerId, Name) values(512,5, 'МФУ PSC 1210');
insert into Model(Id, ManufacturerId, Name) values(513,5, 'МФУ PSC 1215');

insert into Model(Id, ManufacturerId, Name) values(514,6, 'Копир 1015');
insert into Model(Id, ManufacturerId, Name) values(515,6, 'Копир 1120');
insert into Model(Id, ManufacturerId, Name) values(516,6, 'Копир 1212');
insert into Model(Id, ManufacturerId, Name) values(517,6, 'Копир 1216');
insert into Model(Id, ManufacturerId, Name) values(518,6, 'Копир 2223');
insert into Model(Id, ManufacturerId, Name) values(519,6, 'Копир 7022');
insert into Model(Id, ManufacturerId, Name) values(520,6, 'Копир Di1611');
insert into Model(Id, ManufacturerId, Name) values(521,6, 'Копир Di2011');
insert into Model(Id, ManufacturerId, Name) values(522,6, 'Копир EP 1030');

insert into Model(Id, ManufacturerId, Name) values(523,	7, 'Копир KM-1500');
insert into Model(Id, ManufacturerId, Name) values(524,	7, 'Копир KM-1650');

insert into Model(Id, ManufacturerId, Name) values(525,	8, 'Принтер Z-11');
insert into Model(Id, ManufacturerId, Name) values(526,	8, 'Принтер Z605');

insert into Model(Id, ManufacturerId, Name) values(527,9, 'Принтер CLP-2001');
insert into Model(Id, ManufacturerId, Name) values(528,9, 'Принтер CLP-521');
insert into Model(Id, ManufacturerId, Name) values(529,9, 'Копир SF-2116');
insert into Model(Id, ManufacturerId, Name) values(530,9, 'МФУ OfficeCenter 316');
insert into Model(Id, ManufacturerId, Name) values(531,9, 'МФУ OfficeCenter 320');
insert into Model(Id, ManufacturerId, Name) values(532,9, 'МФУ OfficeCenter 420');

insert into Model(Id, ManufacturerId, Name) values(533,10, 'Принтер B4100');
insert into Model(Id, ManufacturerId, Name) values(534,10, 'Принтер B4200L');
insert into Model(Id, ManufacturerId, Name) values(535,10, 'Принтер B4250');
insert into Model(Id, ManufacturerId, Name) values(536,10, 'Принтер B4250n');
insert into Model(Id, ManufacturerId, Name) values(537,10, 'Принтер B4300');
insert into Model(Id, ManufacturerId, Name) values(538,10, 'Принтер B4350');
insert into Model(Id, ManufacturerId, Name) values(539,10, 'Принтер B4350n');
insert into Model(Id, ManufacturerId, Name) values(540,10, 'Принтер B4350nPS');
insert into Model(Id, ManufacturerId, Name) values(541,10, 'Принтер C3200N');

insert into Model(Id, ManufacturerId, Name) values(542,11, 'Факс KX-F 580');
insert into Model(Id, ManufacturerId, Name) values(543,11, 'Факс KX-FL 513RU');
insert into Model(Id, ManufacturerId, Name) values(544,11, 'Факс KX-FT 72');
insert into Model(Id, ManufacturerId, Name) values(545,11, 'Факс KX-FT 74');
insert into Model(Id, ManufacturerId, Name) values(546,11, 'Факс KX-FT 76');
insert into Model(Id, ManufacturerId, Name) values(547,11, 'Факс KX-FT 78');
insert into Model(Id, ManufacturerId, Name) values(548,11, 'Факс KX-FT 902');
insert into Model(Id, ManufacturerId, Name) values(549,11, 'Факс KX-FT 904');

insert into Model(Id, ManufacturerId, Name) values(550,12, 'Копир Aficio 1113');
insert into Model(Id, ManufacturerId, Name) values(551,12, 'Копир FW770');
insert into Model(Id, ManufacturerId, Name) values(552,12, 'Копир FW780');
insert into Model(Id, ManufacturerId, Name) values(553,12, 'МФУ Aficio 1015');
insert into Model(Id, ManufacturerId, Name) values(554,12, 'МФУ Aficio 1018');
insert into Model(Id, ManufacturerId, Name) values(555,12, 'МФУ Aficio 1018D');
insert into Model(Id, ManufacturerId, Name) values(556,12, 'МФУ Aficio 1035');
insert into Model(Id, ManufacturerId, Name) values(557,12, 'МФУ Aficio 1060');
insert into Model(Id, ManufacturerId, Name) values(558,12, 'МФУ Aficio 1075');
insert into Model(Id, ManufacturerId, Name) values(559,12, 'МФУ Aficio 1224C');
insert into Model(Id, ManufacturerId, Name) values(560,12, 'МФУ Aficio 1232C');
insert into Model(Id, ManufacturerId, Name) values(561,12, 'МФУ Aficio 1515');
insert into Model(Id, ManufacturerId, Name) values(562,12, 'МФУ Aficio 1515F');
insert into Model(Id, ManufacturerId, Name) values(563,12, 'МФУ Aficio 1515MF');
insert into Model(Id, ManufacturerId, Name) values(564,12, 'МФУ Aficio 1515PS');
insert into Model(Id, ManufacturerId, Name) values(565,12, 'МФУ Aficio 2015');
insert into Model(Id, ManufacturerId, Name) values(566,12, 'МФУ Aficio 2016');
insert into Model(Id, ManufacturerId, Name) values(567,12, 'МФУ Aficio 2018');
insert into Model(Id, ManufacturerId, Name) values(568,12, 'МФУ Aficio 2018D');
insert into Model(Id, ManufacturerId, Name) values(569,12, 'МФУ Aficio 2020');
insert into Model(Id, ManufacturerId, Name) values(570,12, 'МФУ Aficio 2020D');
insert into Model(Id, ManufacturerId, Name) values(571,12, 'МФУ Aficio 2051');
insert into Model(Id, ManufacturerId, Name) values(572,12, 'МФУ Aficio 2060');
insert into Model(Id, ManufacturerId, Name) values(573,12, 'МФУ Aficio 2075');

insert into Model(Id, ManufacturerId, Name) values(574,13, 'Принтер ML 1615');
insert into Model(Id, ManufacturerId, Name) values(575,13, 'Принтер ML-1210');
insert into Model(Id, ManufacturerId, Name) values(576,13, 'Принтер ML-1250');
insert into Model(Id, ManufacturerId, Name) values(577,13, 'Принтер ML-1440');
insert into Model(Id, ManufacturerId, Name) values(578,13, 'Принтер ML-1450');
insert into Model(Id, ManufacturerId, Name) values(579,13, 'Принтер ML-1510');
insert into Model(Id, ManufacturerId, Name) values(580,13, 'Принтер ML-1520P');
insert into Model(Id, ManufacturerId, Name) values(581,13, 'Принтер ML-1710P');
insert into Model(Id, ManufacturerId, Name) values(582,13, 'Принтер ML-1750');
insert into Model(Id, ManufacturerId, Name) values(583,13, 'Принтер ML-2150');
insert into Model(Id, ManufacturerId, Name) values(584,13, 'Принтер ML-2150');
insert into Model(Id, ManufacturerId, Name) values(585,13, 'Принтер ML-2151N');
insert into Model(Id, ManufacturerId, Name) values(586,13, 'Принтер ML-2152W');
insert into Model(Id, ManufacturerId, Name) values(587,13, 'Принтер ML-2250');
insert into Model(Id, ManufacturerId, Name) values(588,13, 'Принтер ML-2251N');
insert into Model(Id, ManufacturerId, Name) values(589,13, 'Принтер ML-2251NP');
insert into Model(Id, ManufacturerId, Name) values(590,13, 'Принтер ML-2252W');
insert into Model(Id, ManufacturerId, Name) values(591,13, 'Принтер ML-2550');
insert into Model(Id, ManufacturerId, Name) values(592,13, 'Принтер ML-2551N');
insert into Model(Id, ManufacturerId, Name) values(593,13, 'Принтер ML-2552W');
insert into Model(Id, ManufacturerId, Name) values(594,13, 'Принтер ML-4500');
insert into Model(Id, ManufacturerId, Name) values(595,13, 'Принтер ML-6040');
insert into Model(Id, ManufacturerId, Name) values(596,13, 'Принтер ML-6060');
insert into Model(Id, ManufacturerId, Name) values(597,13, 'Принтер ML-6060N');
insert into Model(Id, ManufacturerId, Name) values(598,13, 'Принтер ML-6060S');
insert into Model(Id, ManufacturerId, Name) values(599,13, 'МФУ SCX-4100');
insert into Model(Id, ManufacturerId, Name) values(600,13, 'МФУ SCX-5112');
insert into Model(Id, ManufacturerId, Name) values(601,13, 'МФУ SCX-5115');
insert into Model(Id, ManufacturerId, Name) values(602,13, 'МФУ SCX-5315F');
insert into Model(Id, ManufacturerId, Name) values(603,13, 'МФУ SCX-6220');
insert into Model(Id, ManufacturerId, Name) values(604,13, 'МФУ SF-330');
insert into Model(Id, ManufacturerId, Name) values(605,13, 'МФУ SF-331P');
insert into Model(Id, ManufacturerId, Name) values(606,13, 'МФУ SF-335T');
insert into Model(Id, ManufacturerId, Name) values(607,13, 'МФУ SF-340');
insert into Model(Id, ManufacturerId, Name) values(608,13, 'МФУ SF-345TP');
insert into Model(Id, ManufacturerId, Name) values(609,13, 'МФУ SF-560');
insert into Model(Id, ManufacturerId, Name) values(610,13, 'МФУ SF-565P');

insert into Model(Id, ManufacturerId, Name) values(611,14, 'Копир Z-810');
insert into Model(Id, ManufacturerId, Name) values(612,14, 'Копир Z-830');
insert into Model(Id, ManufacturerId, Name) values(613,14, 'Копир Z-840');

insert into Model(Id, ManufacturerId, Name) values(614,15, 'Принтер DocuPrint N2125');
insert into Model(Id, ManufacturerId, Name) values(615,15, 'Принтер DocuPrint N2825');
insert into Model(Id, ManufacturerId, Name) values(616,15, 'Принтер DocuPrint P1210');
insert into Model(Id, ManufacturerId, Name) values(617,15, 'Принтер DocuPrint P8e');
insert into Model(Id, ManufacturerId, Name) values(618,15, 'Принтер DocuPrint P8ex');
insert into Model(Id, ManufacturerId, Name) values(619,15, 'Принтер Phaser 3110');
insert into Model(Id, ManufacturerId, Name) values(620,15, 'Принтер Phaser 3115');
insert into Model(Id, ManufacturerId, Name) values(621,15, 'Принтер Phaser 3116');
insert into Model(Id, ManufacturerId, Name) values(622,15, 'Принтер Phaser 3117');
insert into Model(Id, ManufacturerId, Name) values(623,15, 'Принтер Phaser 3120');
insert into Model(Id, ManufacturerId, Name) values(624,15, 'Принтер Phaser 3121');
insert into Model(Id, ManufacturerId, Name) values(625,15, 'Принтер Phaser 3122');
insert into Model(Id, ManufacturerId, Name) values(626,15, 'Принтер Phaser 3124');
insert into Model(Id, ManufacturerId, Name) values(627,15, 'Принтер Phaser 3125');
insert into Model(Id, ManufacturerId, Name) values(628,15, 'Принтер Phaser 3125N');
insert into Model(Id, ManufacturerId, Name) values(629,15, 'Принтер Phaser 3130');
insert into Model(Id, ManufacturerId, Name) values(630,15, 'Принтер Phaser 3150');
insert into Model(Id, ManufacturerId, Name) values(631,15, 'Принтер Phaser 3150N');
insert into Model(Id, ManufacturerId, Name) values(632,15, 'Принтер Phaser 3210');
insert into Model(Id, ManufacturerId, Name) values(633,15, 'Принтер Phaser 3310');
insert into Model(Id, ManufacturerId, Name) values(634,15, 'Принтер Phaser 3400');
insert into Model(Id, ManufacturerId, Name) values(635,15, 'Принтер Phaser 3400N');
insert into Model(Id, ManufacturerId, Name) values(636,15, 'Принтер Phaser 3420');
insert into Model(Id, ManufacturerId, Name) values(637,15, 'Принтер Phaser 3425');
insert into Model(Id, ManufacturerId, Name) values(638,15, 'Принтер Phaser 3425ps');
insert into Model(Id, ManufacturerId, Name) values(639,15, 'Принтер Phaser 3450d');
insert into Model(Id, ManufacturerId, Name) values(640,15, 'Принтер Phaser 3450dn');
insert into Model(Id, ManufacturerId, Name) values(641,15, 'Принтер Phaser 3500B');
insert into Model(Id, ManufacturerId, Name) values(642,15, 'Принтер Phaser 3500DN');
insert into Model(Id, ManufacturerId, Name) values(643,15, 'Принтер Phaser 3500N');
insert into Model(Id, ManufacturerId, Name) values(644,15, 'Принтер Phaser 4400B');
insert into Model(Id, ManufacturerId, Name) values(645,15, 'Принтер Phaser 4400DT');
insert into Model(Id, ManufacturerId, Name) values(646,15, 'Принтер Phaser 4400DX');
insert into Model(Id, ManufacturerId, Name) values(647,15, 'Принтер Phaser 4400N');
insert into Model(Id, ManufacturerId, Name) values(648,15, 'Принтер Phaser 4500B');
insert into Model(Id, ManufacturerId, Name) values(649,15, 'Принтер Phaser 4500DT');
insert into Model(Id, ManufacturerId, Name) values(650,15, 'Принтер Phaser 4500DX');
insert into Model(Id, ManufacturerId, Name) values(651,15, 'Принтер Phaser 4500N');
insert into Model(Id, ManufacturerId, Name) values(652,15, 'Принтер Phaser 5400DT');
insert into Model(Id, ManufacturerId, Name) values(653,15, 'Принтер Phaser 5400DX');
insert into Model(Id, ManufacturerId, Name) values(654,15, 'Принтер Phaser 5400N');
insert into Model(Id, ManufacturerId, Name) values(655,15, 'Принтер Phaser 5500B');
insert into Model(Id, ManufacturerId, Name) values(656,15, 'Принтер Phaser 5500DN');
insert into Model(Id, ManufacturerId, Name) values(657,15, 'Принтер Phaser 5500DT');
insert into Model(Id, ManufacturerId, Name) values(658,15, 'Принтер Phaser 5500DX');
insert into Model(Id, ManufacturerId, Name) values(659,15, 'Принтер Phaser 5500N');
insert into Model(Id, ManufacturerId, Name) values(660,15, 'Принтер Phaser 6100BD');
insert into Model(Id, ManufacturerId, Name) values(661,15, 'Принтер Phaser 6100DN');
insert into Model(Id, ManufacturerId, Name) values(662,15, 'Принтер Phaser 6300DN');
insert into Model(Id, ManufacturerId, Name) values(663,15, 'Принтер Phaser 6300N');
insert into Model(Id, ManufacturerId, Name) values(664,15, 'Принтер Phaser 6350DP');
insert into Model(Id, ManufacturerId, Name) values(665,15, 'Принтер Phaser 6350DT');
insert into Model(Id, ManufacturerId, Name) values(666,15, 'Принтер Phaser 6350DX');
insert into Model(Id, ManufacturerId, Name) values(667,15, 'Принтер Phaser 7300B');
insert into Model(Id, ManufacturerId, Name) values(668,15, 'Принтер Phaser 7300DN');
insert into Model(Id, ManufacturerId, Name) values(669,15, 'Принтер Phaser 7300DT');
insert into Model(Id, ManufacturerId, Name) values(670,15, 'Принтер Phaser 7300DX');
insert into Model(Id, ManufacturerId, Name) values(671,15, 'Принтер Phaser 7300N');
insert into Model(Id, ManufacturerId, Name) values(672,15, 'Копир 3030');
insert into Model(Id, ManufacturerId, Name) values(673,15, 'Копир 3040');
insert into Model(Id, ManufacturerId, Name) values(674,15, 'Копир 3050');
insert into Model(Id, ManufacturerId, Name) values(675,15, 'Копир 3060');
insert into Model(Id, ManufacturerId, Name) values(676,15, 'Копир 5915');
insert into Model(Id, ManufacturerId, Name) values(677,15, 'Копир 5918');
insert into Model(Id, ManufacturerId, Name) values(678,15, 'Копир 5921');
insert into Model(Id, ManufacturerId, Name) values(679,15, 'Копир DocuColor 30');
insert into Model(Id, ManufacturerId, Name) values(680,15, 'Копир DocuColor 40');
insert into Model(Id, ManufacturerId, Name) values(681,15, 'Копир Xerox WorkCentre Pro 423');
insert into Model(Id, ManufacturerId, Name) values(682,15, 'Копир Xerox WorkCentre Pro 423DC');
insert into Model(Id, ManufacturerId, Name) values(683,15, 'Копир Xerox WorkCentre Pro 428');
insert into Model(Id, ManufacturerId, Name) values(684,15, 'Копир Xerox WorkCentre Pro 428DC');
insert into Model(Id, ManufacturerId, Name) values(685,15, 'МФУ DocuColor 4CP');
insert into Model(Id, ManufacturerId, Name) values(686,15, 'МФУ WorkCentre 3119');
insert into Model(Id, ManufacturerId, Name) values(687,15, 'МФУ WorkCentre 312');
insert into Model(Id, ManufacturerId, Name) values(688,15, 'МФУ WorkCentre 4118p');
insert into Model(Id, ManufacturerId, Name) values(689,15, 'МФУ WorkCentre 4118x');
insert into Model(Id, ManufacturerId, Name) values(690,15, 'МФУ WorkCentre M118');
insert into Model(Id, ManufacturerId, Name) values(691,15, 'МФУ WorkCentre M118i');
insert into Model(Id, ManufacturerId, Name) values(692,15, 'МФУ WorkCentre M15');
insert into Model(Id, ManufacturerId, Name) values(693,15, 'МФУ WorkCentre M15i');
insert into Model(Id, ManufacturerId, Name) values(694,15, 'МФУ WorkCentre M20');
insert into Model(Id, ManufacturerId, Name) values(695,15, 'МФУ WorkCentre M20i');
insert into Model(Id, ManufacturerId, Name) values(696,15, 'МФУ WorkCentre PE114e');
insert into Model(Id, ManufacturerId, Name) values(697,15, 'МФУ WorkCentre PE120');
insert into Model(Id, ManufacturerId, Name) values(698,15, 'МФУ WorkCentre PE120i');
insert into Model(Id, ManufacturerId, Name) values(699,15, 'МФУ WorkCentre PE220');
insert into Model(Id, ManufacturerId, Name) values(700,15, 'МФУ WorkCentre Pro 315');
insert into Model(Id, ManufacturerId, Name) values(701,15, 'МФУ WorkCentre Pro 320');


insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(1000, null,						'Рассмотрение',				'#6B8E23',	'2000, 2100');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(1100, null,						'Дата окончания',			null,		null);
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(2000,'Принять',					'Принята',					'#FFE4B5',	'2200, 2300, 2400');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(2100,'Отказать',					'Отказано',					'#FFA07A',	'3200');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(2200,'Отказать',					'Отказ после принятия',		'#ADD8E6',	'3200');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(2300,'Перенести',				'Перенос',					'#FFC0CB',	'2300, 2400, 3400');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(2400,'Выполнена',				'Выполнена',				'#7B68EE',	'3000, 3100, 3300');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(3000,'Перенести подтверждение',	'Перенос подтверждения',	'#FF4500',	'3000, 3100, 3300, 3400');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(3100,'Отказать в готовности',	'Отказано в готовности',	'#FFA500',	'2300, 2400, 3400');
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(3200,'Подтвердить отказ',		'Подтверждение отказа',		'#D2B48C',	null);
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(3300,'Подтвердить готовность',	'Подтверждение выполнения',	'#40E0D0',	null);
insert into StatusRequest(Id, ActionName, Name, BackColor, AllowableStates) values(3400,'В пассив',					'Пасив',					'#F0E68C',	null);

insert into CabinetUser(Id, Email, Password, Subscribe) values(1, 'admin@mail.ru',	'admin@mail.ru', 1);
insert into CabinetUser(Id, Email, Password, Subscribe) values(2, 'user@mail.ru',	'user@mail.ru', 1);

insert into Worker(Id, Name) values(1, 'ООО "Автоматика"');
insert into Worker(Id, Name) values(2, 'ООО "Старт"');
insert into Worker(Id, Name) values(3, 'ООО "Пусковой комплекс"');
insert into Worker(Id, Name) values(4, 'ООО "Комплексные решения"');

insert into TypeWorkerUser(Id, Name, AllowableStates, TypeCode) values(1,'Исполнитель',			'2000, 2100, 2200, 2300, 2400', 0);
insert into TypeWorkerUser(Id, Name, AllowableStates, TypeCode) values(2,'Диспетчер',				'3000, 3100, 3200, 3300, 3400', 1);
insert into TypeWorkerUser(Id, Name, AllowableStates, TypeCode) values(3,'Исполнитель-диспетчер',	'2000, 2100, 2200, 2300, 2400, 3000, 3100, 3200, 3300, 3400', 2);

insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId, Subscribe) values(1, 'worker@mail.ru',		'worker@mail.ru',		'Иванов И.И.',	1, 1, 1);
insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId, Subscribe) values(2, 'disp@mail.ru',			'disp@mail.ru',			'Петров П.П.',	2, NULL, 0);
insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId, Subscribe) values(3, 'worker-disp@mail.ru',	'worker-disp@mail.ru',	'Сидоров С.С.', 3, 2, 1);

insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(1, 'Департамент строительства',	'ул. Мира 1, стр 2',		null,	1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(2, 'Департамент связи',			'ул. Ленина 22',			null,	1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(3, 'Руководство',				'ул. Энгельса 3/4',			null,	1, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(4, 'СМУ Спецмонтажтрой',		'ул. 12-я Линия 41',		1,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(5, 'СМУ 1',						'ул. Производственная 2',	1,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(6, 'СМУ 2',						'ул. Береговая 2, стр.4',	1,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(7, 'Управление спутниковой связи',		'ул. Ленина 22',	2,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(8, 'СМУ ВысотМонтаж',			'ул. 12-я Линия 41',		2,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(9, 'НИИ Связи',					'пр. Маркса 6',				2,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(10, 'ООО ВысотСвязь',			'пр. Маркса 6',				2,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(11, 'Филиал Самара',			'ул. Заводская 55, стр.2',	2,		0, 0);
insert into Organization(Id, Name, Address, ParentId, HasChild, Archive) values(12, 'Филиал Нижний Новгород',	'ул. Первый переулок 9',	2,		0, 0);

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