using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService;
using HelpDesk.Data.Repository;
using Moq;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using System;
using System.Linq.Expressions;
using HelpDesk.Data.Query;
using HelpDesk.Data.NHibernate;
using NHibernate;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов EmployeeObjectService
    /// </summary>
    [TestClass]
    public class EmployeeObjectServiceTest
    {
        IList<WorkerUser> listUser = new List<WorkerUser>()
        {
            new WorkerUser { Id = 1, Worker = new Worker { Id = 1 } },
            new WorkerUser { Id = 2, Worker = new Worker { Id = 2 } },
            new WorkerUser { Id = 3 }
        };

        IList<AccessWorkerUser> listAccessUser = new List<AccessWorkerUser>()
        {
            new AccessWorkerUser { User = new WorkerUser { Id = 1 }, Worker = new Worker { Id = 1 }, Type= TypeAccessWorkerUserEnum.Worker  },
            new AccessWorkerUser { User = new WorkerUser { Id = 1 }, Worker = new Worker { Id = 3 }, Type= TypeAccessWorkerUserEnum.Worker  }
        };

        IList<OrganizationObjectTypeWorker> listOrganizationObjectTypeWorker = new List<OrganizationObjectTypeWorker>()
        {
            new OrganizationObjectTypeWorker { ObjectType = new ObjectType { Id = 1 }, Organization = new Organization { Id = 1 }, Worker = new Worker { Id = 1 } },
            new OrganizationObjectTypeWorker { ObjectType = new ObjectType { Id = 1 }, Organization = new Organization { Id = 2 }, Worker = new Worker { Id = 1 } },
            new OrganizationObjectTypeWorker { ObjectType = new ObjectType { Id = 2 }, Organization = new Organization { Id = 2 }, Worker = new Worker { Id = 1 } },
            new OrganizationObjectTypeWorker { ObjectType = new ObjectType { Id = 3 }, Organization = new Organization { Id = 2 }, Worker = new Worker { Id = 2 } },
            new OrganizationObjectTypeWorker { ObjectType = new ObjectType { Id = 3 }, Organization = new Organization { Id = 1 }, Worker = new Worker { Id = 3 } }
        };

        IList<Employee> listEmployee = new List<Employee>()
        {
            new Employee {Id = 1, Organization = new Organization { Id = 1 } },
            new Employee {Id = 2, Organization = new Organization { Id = 1 } },
            new Employee {Id = 3, Organization = new Organization { Id = 2 } },
            new Employee {Id = 4, Organization = new Organization { Id = 2 } },
            new Employee {Id = 5, Organization = new Organization { Id = 3 } }
        };

        IList<RequestObject> listObject = new List<RequestObject>()
        {
            new RequestObject { Id = 1, ObjectType = new ObjectType { Id = 5, Soft = true } },
            new RequestObject { Id = 2, ObjectType = new ObjectType { Id = 5, Soft = true } },
            new RequestObject { Id = 3, ObjectType = new ObjectType { Id = 5, Soft = true } },
            new RequestObject { Id = 4, ObjectType = new ObjectType { Id = 5, Soft = true } }
        };

        IList<EmployeeObject> listEmployeeObject = new List<EmployeeObject>()
        {
            new EmployeeObject {  Employee = new Employee { Id = 1 }, Object = new RequestObject { Id = 1 } },
            new EmployeeObject {  Employee = new Employee { Id = 1 }, Object = new RequestObject { Id = 2 } },
            new EmployeeObject {  Employee = new Employee { Id = 2 }, Object = new RequestObject { Id = 3 } }
        };

        [TestMethod]
        public void EmployeeObjectService_GetListAllowableObjectType_Worker()
        {
            Mock<ISession> session = new Mock<ISession>(MockBehavior.Strict);
            session.Setup(x => x.Query<OrganizationObjectTypeWorker>())
               .Returns(() => { return listOrganizationObjectTypeWorker.AsQueryable(); });
            session.Setup(x => x.Query<Employee>())
               .Returns(() => { return listEmployee.AsQueryable(); });

            IQueryRunner queryRunner = new QueryRunner(session.Object);
            
            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.AsQueryable().FirstOrDefault(u => u.Id == id); });

            Mock<IBaseRepository<AccessWorkerUser>> accessWorkerUserRepository = new Mock<IBaseRepository<AccessWorkerUser>>(MockBehavior.Strict);
            accessWorkerUserRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<AccessWorkerUser, bool>>>()))
               .Returns((Expression<Func<AccessWorkerUser, bool>> predicate) => { return listAccessUser.AsQueryable().Where(predicate); });

            EmployeeObjectService employeeObjectService =
                new EmployeeObjectService(
                    queryRunner,//*
                    Mock.Of<IBaseRepository<EmployeeObject>>(),
                    Mock.Of<IBaseRepository<Employee>>(),
                    Mock.Of<IBaseRepository<RequestObject>>(),
                    Mock.Of<IBaseRepository<HardType>>(),
                    Mock.Of<IBaseRepository<Manufacturer>>(),
                    Mock.Of<IBaseRepository<Model>>(),
                    Mock.Of<IBaseRepository<ObjectType>>(),
                    workerUserRepository.Object,//*
                    accessWorkerUserRepository.Object,//*
                    new AccessWorkerUserExpressionService(),//*
                    Mock.Of<IRepository>());

            IEnumerable<SimpleDTO> list = employeeObjectService.GetListAllowableObjectType(1, 1);

            Assert.IsTrue(list.Count() == 2);
            Assert.IsTrue(list.Any(t => t.Id == 1));
            Assert.IsTrue(list.Any(t => t.Id == 3));
        }

        [TestMethod]
        public void EmployeeObjectService_GetListAllowableObjectType_Dispatcher()
        {
            Mock<ISession> session = new Mock<ISession>(MockBehavior.Strict);
            session.Setup(x => x.Query<OrganizationObjectTypeWorker>())
               .Returns(() => { return listOrganizationObjectTypeWorker.AsQueryable(); });
            session.Setup(x => x.Query<Employee>())
               .Returns(() => { return listEmployee.AsQueryable(); });

            IQueryRunner queryRunner = new QueryRunner(session.Object);

            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.AsQueryable().FirstOrDefault(u => u.Id == id); });

            Mock<IBaseRepository<AccessWorkerUser>> accessWorkerUserRepository = new Mock<IBaseRepository<AccessWorkerUser>>(MockBehavior.Strict);
            accessWorkerUserRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<AccessWorkerUser, bool>>>()))
               .Returns((Expression<Func<AccessWorkerUser, bool>> predicate) => { return listAccessUser.AsQueryable().Where(predicate); });

            EmployeeObjectService employeeObjectService =
                new EmployeeObjectService(
                    queryRunner,//*
                    Mock.Of<IBaseRepository<EmployeeObject>>(),
                    Mock.Of<IBaseRepository<Employee>>(),
                    Mock.Of<IBaseRepository<RequestObject>>(),
                    Mock.Of<IBaseRepository<HardType>>(),
                    Mock.Of<IBaseRepository<Manufacturer>>(),
                    Mock.Of<IBaseRepository<Model>>(),
                    Mock.Of<IBaseRepository<ObjectType>>(),
                    workerUserRepository.Object,//*
                    accessWorkerUserRepository.Object,//*
                    new AccessWorkerUserExpressionService(),//*
                    Mock.Of<IRepository>());

            IEnumerable<SimpleDTO> list = employeeObjectService.GetListAllowableObjectType(3, 3);

            Assert.IsTrue(list.Count() == 3);
            Assert.IsTrue(list.Any(t => t.Id == 1));
            Assert.IsTrue(list.Any(t => t.Id == 2));
            Assert.IsTrue(list.Any(t => t.Id == 3));
        }

        [TestMethod]
        public void EmployeeObjectService_GetListAllowableObjectIS_Worker()
        {
            Mock<ISession> session = new Mock<ISession>(MockBehavior.Strict);
            session.Setup(x => x.Query<RequestObject>())
               .Returns(() => { return listObject.AsQueryable(); });
            session.Setup(x => x.Query<OrganizationObjectTypeWorker>())
               .Returns(() => { return listOrganizationObjectTypeWorker.AsQueryable(); });
            session.Setup(x => x.Query<Employee>())
               .Returns(() => { return listEmployee.AsQueryable(); });
            session.Setup(x => x.Query<EmployeeObject>())
               .Returns(() => { return listEmployeeObject.AsQueryable(); });
            
            IQueryRunner queryRunner = new QueryRunner(session.Object);

            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.AsQueryable().FirstOrDefault(u => u.Id == id); });

            Mock<IBaseRepository<AccessWorkerUser>> accessWorkerUserRepository = new Mock<IBaseRepository<AccessWorkerUser>>(MockBehavior.Strict);
            accessWorkerUserRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<AccessWorkerUser, bool>>>()))
               .Returns((Expression<Func<AccessWorkerUser, bool>> predicate) => { return listAccessUser.AsQueryable().Where(predicate); });

            EmployeeObjectService employeeObjectService =
                new EmployeeObjectService(
                    queryRunner,//*
                    Mock.Of<IBaseRepository<EmployeeObject>>(),
                    Mock.Of<IBaseRepository<Employee>>(),
                    Mock.Of<IBaseRepository<RequestObject>>(),
                    Mock.Of<IBaseRepository<HardType>>(),
                    Mock.Of<IBaseRepository<Manufacturer>>(),
                    Mock.Of<IBaseRepository<Model>>(),
                    Mock.Of<IBaseRepository<ObjectType>>(),
                    workerUserRepository.Object,//*
                    accessWorkerUserRepository.Object,//*
                    new AccessWorkerUserExpressionService(),//*
                    Mock.Of<IRepository>());

            IEnumerable<RequestObjectISDTO> list = employeeObjectService.GetListAllowableObjectIS(1, 3);
        }

        [TestMethod]
        public void EmployeeObjectService_GetListAllowableObjectIS_Dispatcher()
        {
            Mock<ISession> session = new Mock<ISession>(MockBehavior.Strict);
            session.Setup(x => x.Query<RequestObject>())
               .Returns(() => { return listObject.AsQueryable(); });
            session.Setup(x => x.Query<OrganizationObjectTypeWorker>())
               .Returns(() => { return listOrganizationObjectTypeWorker.AsQueryable(); });
            session.Setup(x => x.Query<Employee>())
               .Returns(() => { return listEmployee.AsQueryable(); });
            session.Setup(x => x.Query<EmployeeObject>())
               .Returns(() => { return listEmployeeObject.AsQueryable(); });

            IQueryRunner queryRunner = new QueryRunner(session.Object);

            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.AsQueryable().FirstOrDefault(u => u.Id == id); });

            Mock<IBaseRepository<AccessWorkerUser>> accessWorkerUserRepository = new Mock<IBaseRepository<AccessWorkerUser>>(MockBehavior.Strict);
            accessWorkerUserRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<AccessWorkerUser, bool>>>()))
               .Returns((Expression<Func<AccessWorkerUser, bool>> predicate) => { return listAccessUser.AsQueryable().Where(predicate); });

            EmployeeObjectService employeeObjectService =
                new EmployeeObjectService(
                    queryRunner,//*
                    Mock.Of<IBaseRepository<EmployeeObject>>(),
                    Mock.Of<IBaseRepository<Employee>>(),
                    Mock.Of<IBaseRepository<RequestObject>>(),
                    Mock.Of<IBaseRepository<HardType>>(),
                    Mock.Of<IBaseRepository<Manufacturer>>(),
                    Mock.Of<IBaseRepository<Model>>(),
                    Mock.Of<IBaseRepository<ObjectType>>(),
                    workerUserRepository.Object,//*
                    accessWorkerUserRepository.Object,//*
                    new AccessWorkerUserExpressionService(),//*
                    Mock.Of<IRepository>());

            IEnumerable<RequestObjectISDTO> list = employeeObjectService.GetListAllowableObjectIS(3, 3);
        }  
    }
}
