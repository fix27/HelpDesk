    create table TypeWorkerUser (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       AllowableStates VARCHAR(200) null,
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
       WorkerId BIGINT null,
       TypeWorkerUserId BIGINT not null,
       primary key (Id)
    )

    create table RequestFile (
        Id BIGINT not null,
       Name VARCHAR(200) not null,
       Body VARBINARY(MAX) not null,
       Thumbnail VARBINARY(MAX) not null,
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
	
    create table Request (
        Id BIGINT not null,
       Version INT not null,
       DateInsert DATETIME not null,
       DateUpdate DATETIME not null,
       DateEndPlan DATETIME not null,
       DateEndFact DATETIME null,
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
       DateInsert DATETIME not null,
       DateUpdate DATETIME not null,
       DateEndPlan DATETIME not null,
       DateEndFact DATETIME null,
       DescriptionProblem VARCHAR(2000) not null,
       CountCorrectionDateEndPlan INT not null,
       StatusId BIGINT not null,
       ObjectId BIGINT not null,
       EmployeeId BIGINT not null,
       WorkerId BIGINT not null,
       UserId BIGINT null,
       primary key (Id)
    )
	
    create table RequestEvent (
        Id BIGINT not null,
       RequestId BIGINT not null,
       Note VARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME not null,
       DateInsert DATETIME not null,
       StatusRequestId BIGINT null,
       UserId BIGINT null,
       primary key (Id)
    )

    create table RequestEventArch (
        Id BIGINT not null,
       RequestId BIGINT not null,
       Note VARCHAR(2000) null,
       OrdGroup INT not null,
       DateEvent DATETIME not null,
       DateInsert DATETIME not null,
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
       Subscribe BIT not null,
       PostId BIGINT not null,
       OrganizationId BIGINT not null,
       primary key (Id)
    )

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

    create sequence SQ_REQUEST as int start with 100000 increment by 1



insert into Settings(Id, MinInterval, LimitRequestCount,  MaxRequestFileCount,  MaxRequestFileSize,  MaxFileNameLength) values(1, 1, 10, 5, 500, 200);

insert into ObjectType(Id, Name, Soft, Archive) values(1, 'Сопровождение ИС',			1, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(2, 'ТО ВТ',						0, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(3, 'ТО КМТ',						0, 0);
insert into ObjectType(Id, Name, Soft, Archive) values(4, 'Сетевое администрирование',	0, 0);

insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(1, 'ПП "Галактика"',			1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(2, 'ПП "1C: Кадры"',			1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(3, 'ПП "Документооборот"',	1, 0);
insert into RequestObject (Id, SoftName, ObjectTypeId, Archive) values(4, 'ПП "Компас"',			1, 0);

insert into Manufacturer(Id, Name) values(1,'SONY');
insert into Manufacturer(Id, Name) values(2,'HP');
insert into Manufacturer(Id, Name) values(3,'CANNON');

insert into HardType(Id, Name) values(1,'ПРИНТЕР');
insert into HardType(Id, Name) values(2,'МОНИТОР');
insert into HardType(Id, Name) values(3,'СКАНЕР');
insert into HardType(Id, Name) values(4,'КЛАВИАТУРА');

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

insert into CabinetUser(Id, Email, Password) values(1, 'admin@mail.ru',	'admin@mail.ru');
insert into CabinetUser(Id, Email, Password) values(2, 'user@mail.ru',	'user@mail.ru');

insert into Worker(Id, Name) values(1, 'ООО "Автоматика"');
insert into Worker(Id, Name) values(2, 'ООО "Старт"');
insert into Worker(Id, Name) values(3, 'ООО "Пусковой комплекс"');
insert into Worker(Id, Name) values(4, 'ООО "Комплексные решения"');

insert into TypeWorkerUser(Id, Name, AllowableStates) values(1,'Исполнитель',			'2000, 2100, 2200, 2300, 2400');
insert into TypeWorkerUser(Id, Name, AllowableStates) values(2,'Диспетчер',				'3000, 3100, 3200, 3300, 3400');
insert into TypeWorkerUser(Id, Name, AllowableStates) values(3,'Исполнитель-диспетчер',	'2000, 2100, 2200, 2300, 2400, 3000, 3100, 3200, 3300, 3400');

insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId) values(1, 'worker@mail.ru',		'worker@mail.ru',		'Иванов И.И.',	1, 1);
insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId) values(2, 'disp@mail.ru',			'disp@mail.ru',			'Петров П.П.',	2, NULL);
insert into WorkerUser(Id, Email, Password, Name, TypeWorkerUserId, WorkerId) values(3, 'worker-disp@mail.ru',	'worker-disp@mail.ru',	'Сидоров С.С.', 3, 2);

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