using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService;
using HelpDesk.Data.Repository;
using Moq;
using HelpDesk.Entity;
using System;
using System.Linq.Expressions;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Interface;
using HelpDesk.DataService.Common.Interface;
using HelpDesk.EventBus.Common.Interface;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.Data.Command;
using HelpDesk.Common.Cache;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов RequestService
    /// </summary>
    [TestClass]
    public class RequestServiceTest
    {
        IList<WorkerUser> listUser = new List<WorkerUser>()
        {
            new WorkerUser { Id = 1, Worker = new Worker { Id = 1 }, UserType = new TypeWorkerUser { TypeCode = TypeWorkerUserEnum.Worker }  },
            new WorkerUser { Id = 2, Worker = new Worker { Id = 2 }, UserType = new TypeWorkerUser { TypeCode = TypeWorkerUserEnum.WorkerAndDispatcher } },
            new WorkerUser { Id = 3, UserType = new TypeWorkerUser { TypeCode = TypeWorkerUserEnum.Dispatcher } }
        };

        IList<AccessWorkerUser> listAccessUser = new List<AccessWorkerUser>()
        {
            new AccessWorkerUser { User = new WorkerUser { Id = 1 }, Worker = new Worker { Id = 1 }, Type= TypeAccessWorkerUserEnum.Worker  },
            new AccessWorkerUser { User = new WorkerUser { Id = 2 }, Worker = new Worker { Id = 2 }, Type= TypeAccessWorkerUserEnum.Worker  },
            new AccessWorkerUser { User = new WorkerUser { Id = 2 }, Worker = new Worker { Id = 4 }, Type= TypeAccessWorkerUserEnum.Worker  }
        };

        IList<Request> listRequest = new List<Request>()
        {
            new Request { Id = 1000, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1001, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 2000 } },
            new Request { Id = 1002, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 2100 } },
            new Request { Id = 1003, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 2400 } },
            new Request { Id = 1004, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 3000 } },
            new Request { Id = 1005, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 3100 } },
            new Request { Id = 1006, Worker = new Worker {Id = 1 }, Status = new StatusRequest { Id = 3100 } },

            new Request { Id = 1007, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 2400 } },
            new Request { Id = 1008, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 2100 } },
            new Request { Id = 1009, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 2200 } },
            new Request { Id = 1010, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1011, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 3100 } },
            new Request { Id = 1012, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 3000 } },
            new Request { Id = 1013, Worker = new Worker {Id = 2 }, Status = new StatusRequest { Id = 3000 } },

            new Request { Id = 1014, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1015, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 3100 } },
            new Request { Id = 1016, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1017, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1018, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 2100 } },
            new Request { Id = 1019, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 3000 } },
            new Request { Id = 1020, Worker = new Worker {Id = 3 }, Status = new StatusRequest { Id = 2200 } },

            new Request { Id = 1021, Worker = new Worker {Id = 4 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1022, Worker = new Worker {Id = 4 }, Status = new StatusRequest { Id = 1000 } },
            new Request { Id = 1023, Worker = new Worker {Id = 4 }, Status = new StatusRequest { Id = 2200 } }            
        };

        [TestMethod]
        public void RequestServiceTest_GetCountRequiresReaction_Worker()
        {
            Mock<IBaseRepository<WorkerUser>> workerUserRepository = new Mock<IBaseRepository<WorkerUser>>(MockBehavior.Strict);
            workerUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.AsQueryable().FirstOrDefault(u => u.Id == id); });

            Mock<IBaseRepository<AccessWorkerUser>> accessWorkerUserRepository = new Mock<IBaseRepository<AccessWorkerUser>>(MockBehavior.Strict);
            accessWorkerUserRepository.Setup(x => x.GetList(It.IsAny<Expression<Func<AccessWorkerUser, bool>>>()))
               .Returns((Expression<Func<AccessWorkerUser, bool>> predicate) => { return listAccessUser.AsQueryable().Where(predicate); });

            Mock<IBaseRepository<Request>> requestRepository = new Mock<IBaseRepository<Request>>(MockBehavior.Strict);
            requestRepository.Setup(x => x.GetList())
                .Returns(() => { return listRequest.AsQueryable(); });
            requestRepository.Setup(x => x.Count(It.IsAny<Expression<Func<Request, bool>>>()))
               .Returns((Expression<Func<Request, bool>> predicate) => { return listRequest.AsQueryable().Count(predicate); });

            RequestService requestService = new RequestService(Mock.Of<ICommandRunner>(),
                Mock.Of<IQueryRunner>(),
                Mock.Of<IBaseRepository<RequestObject>>(),
                Mock.Of<IBaseRepository<DescriptionProblem>>(),
                Mock.Of<ISettingsRepository>(),
                Mock.Of<IBaseRepository<OrganizationObjectTypeWorker>>(),
                Mock.Of<IBaseRepository<Employee>>(),
                Mock.Of<IBaseRepository<StatusRequest>>(),
                requestRepository.Object,
                Mock.Of<IBaseRepository<RequestArch>>(),
                Mock.Of<IBaseRepository<RequestEvent>>(),
                Mock.Of<IBaseRepository<RequestEventArch>>(),
                Mock.Of<IBaseRepository<RequestFile>>(),
                Mock.Of<IBaseRepository<Worker>>(),
                workerUserRepository.Object,
                Mock.Of<IRepository>(),
                Mock.Of<IDateTimeService>(),
                Mock.Of<IRequestConstraintsService>(),
                Mock.Of<IStatusRequestMapService>(),
                accessWorkerUserRepository.Object,
                new AccessWorkerUserExpressionService(),
                Mock.Of<IQueue<IRequestAppEvent>>(),
                Mock.Of<ICache>());

            int c = requestService.GetCountRequiresReaction(1);
            Assert.IsTrue(c == 3);/*1000, 3100 - Worker.Id = 1*/

            c = requestService.GetCountRequiresReaction(2);
            Assert.IsTrue(c == 8);/*1000, 3100, 2400, 2100, 2200 - Worker.Id = 2, 4*/

            c = requestService.GetCountRequiresReaction(3);
            Assert.IsTrue(c == 8);/*2400, 2100, 2200 - Worker.Id = 3*/
        }
                
    }
}
