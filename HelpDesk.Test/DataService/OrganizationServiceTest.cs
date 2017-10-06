using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService;
using System;
using System.Linq;
using Moq;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using HelpDesk.DataService.DTO;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов OrganizationServiceTest
    /// </summary>
    [TestClass]
    public class OrganizationServiceTest
    {
        IEnumerable<Organization> organizations = new List<Organization>()
        {
            new Organization() {Id = 1, ParentId = null, HasChild = true, Name="Name1" },
            new Organization() {Id = 2, ParentId = null, HasChild = true, Name="Name2" },
            new Organization() {Id = 3, ParentId = 1, HasChild = false, Name="Name3" },
            new Organization() {Id = 4, ParentId = 1, HasChild = false, Name="Name4" },
            new Organization() {Id = 5, ParentId = 2, HasChild = false, Name="Name5" },
            new Organization() {Id = 6, ParentId = 2, HasChild = false, Name="Name6" }
        };

        IEnumerable<OrganizationObjectTypeWorker> organizationObjectTypeWorkers
            = new List<OrganizationObjectTypeWorker>()
        {
            new OrganizationObjectTypeWorker()
            {
                 Organization = new Organization() { Id = 5 },
                 Worker = new Worker() { Id = 1 }
            },
            new OrganizationObjectTypeWorker()
            {
                 Organization = new Organization() { Id = 1 },
                 Worker = new Worker() { Id = 1 }
            }
        };

        [TestMethod]
        public void OrganizationServiceTest_GetListByWorkerUser_TopByParentId()
        {
            Mock<IBaseRepository<Organization>> organizationRepository = new Mock<IBaseRepository<Organization>>(MockBehavior.Strict);
            organizationRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Organization, bool>>>()))
               .Returns((Expression<Func<Organization, bool>> predicate) =>
               {
                   return organizations
                        .AsQueryable()
                        .Where(predicate);
               });
            organizationRepository.Setup(x => x.GetList())
               .Returns(() =>
               {
                   return organizations.AsQueryable();
               });


            Mock<IBaseRepository<OrganizationObjectTypeWorker>> organizationObjectTypeWorkerRepository = new Mock<IBaseRepository<OrganizationObjectTypeWorker>>(MockBehavior.Strict);
            organizationObjectTypeWorkerRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<OrganizationObjectTypeWorker, bool>>>()))
               .Returns((Expression<Func<OrganizationObjectTypeWorker, bool>> predicate) =>
               {
                   return organizationObjectTypeWorkers.AsQueryable();
               });

            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) =>
               {
                   return new WorkerUser()
                   {
                       Id = 1,
                       Worker = new Worker() { Id = 1 }
                   };
               });


            OrganizationService s = new OrganizationService(
                organizationRepository.Object,
                organizationObjectTypeWorkerRepository.Object,
                workerUserRepository.Object);

            IEnumerable<OrganizationDTO> list = s.GetListByWorkerUser(1, (long?)null);

            Assert.IsTrue(list.Any(o => o.Id == 1 && o.Selectable));
            Assert.IsTrue(list.Any(o => o.Id == 2 && !o.Selectable));

        }

        [TestMethod]
        public void OrganizationServiceTest_GetListByWorkerUser_ByName()
        {
            Mock<IBaseRepository<Organization>> organizationRepository = new Mock<IBaseRepository<Organization>>(MockBehavior.Strict);
            organizationRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<Organization, bool>>>()))
               .Returns((Expression<Func<Organization, bool>> predicate) =>
               {
                   return organizations
                        .AsQueryable()
                        .Where(predicate);
               });
            organizationRepository.Setup(x => x.GetList())
               .Returns(() =>
               {
                   return organizations.AsQueryable();
               });


            Mock<IBaseRepository<OrganizationObjectTypeWorker>> organizationObjectTypeWorkerRepository = new Mock<IBaseRepository<OrganizationObjectTypeWorker>>(MockBehavior.Strict);
            organizationObjectTypeWorkerRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<OrganizationObjectTypeWorker, bool>>>()))
               .Returns((Expression<Func<OrganizationObjectTypeWorker, bool>> predicate) =>
               {
                   return organizationObjectTypeWorkers.AsQueryable();
               });

            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) =>
               {
                   return new WorkerUser()
                   {
                       Id = 1,
                       Worker = new Worker() { Id = 1 }
                   };
               });


            OrganizationService s = new OrganizationService(
                organizationRepository.Object,
                organizationObjectTypeWorkerRepository.Object,
                workerUserRepository.Object);

            IEnumerable<OrganizationDTO> list = s.GetListByWorkerUser(1, "Name5");

            Assert.IsTrue(list.Any(o => o.Id == 5 && o.Selectable));
            
        }
    }
}
