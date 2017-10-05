using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService.Interface;
using HelpDesk.DataService;
using HelpDesk.Data.Repository;
using Moq;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using System;
using System.Linq.Expressions;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов UserService
    /// </summary>
    [TestClass]
    public class CabinetUserServiceTest
    {
        IList<CabinetUser> listUser = new List<CabinetUser>()
        {
            new CabinetUser {Id = 1, Email="email1@mail.ru" },
            new CabinetUser {Id = 2, Email="email2@mail.ru" },
            new CabinetUser {Id = 3, Email="email3@mail.ru" }
        };

        IList<Employee> listPersonalProfile = new List<Employee>()
        {
            new Employee {Id = 1, User = new CabinetUser {Id = 1, Email="email1@mail.ru" } },
            new Employee {Id = 2, User = new CabinetUser {Id = 2, Email="email2@mail.ru" } },
            new Employee {Id = 3, User = new CabinetUser {Id = 3, Email="email3@mail.ru" } }
        };


        ICabinetUserService userService;
        public CabinetUserServiceTest()
        {
            Mock<IBaseRepository<CabinetUser>> mockUserRepository = new Mock<IBaseRepository<CabinetUser>>(MockBehavior.Strict);
            mockUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.FirstOrDefault(t => t.Id == id); });
            mockUserRepository.Setup(x => x.Get(It.IsAny<Expression<Func<CabinetUser, bool>>>()))
               .Returns((Expression<Func<CabinetUser, bool>> predicate) => { return listUser.AsQueryable().FirstOrDefault(predicate); });
            mockUserRepository.Setup(x => x.GetList())
               .Returns(() => { return listUser.AsQueryable(); });

            Mock<IBaseRepository<Employee>> mockPersonalProfileRepository = new Mock<IBaseRepository<Employee>>(MockBehavior.Strict);
            mockPersonalProfileRepository.Setup(x => x.GetList())
                .Returns(() => { return listPersonalProfile.AsQueryable(); });


            userService = new CabinetUserService(
                mockUserRepository.Object,
                Mock.Of<IBaseRepository<UserSession>>(),
                mockPersonalProfileRepository.Object,
                Mock.Of<ISettingsRepository>(),
                Mock.Of<IDateTimeService>(),
                Mock.Of<IRepository>());
        }
       

        [TestMethod]
        public void CabinetUserService_GetDTO_ByUserId()
        {
            CabinetUserDTO u = userService.GetDTO(1);

            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);
                      

            u = userService.GetDTO(2);

            Assert.AreEqual(u.Id, listUser[1].Id);
            Assert.AreEqual(u.Email, listUser[1].Email);
                       

            u = userService.GetDTO(3);

            Assert.AreEqual(u.Id, listUser[2].Id);
            Assert.AreEqual(u.Email, listUser[2].Email);
                      
        }

        [TestMethod]
        public void CabinetUserService_GetDTO_ByUserEmail()
        {
            
            CabinetUserDTO u = userService.GetDTO("email1@mail.ru");

            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);
                      
        }

        [TestMethod]
        public void CabinetUserService_GetDTO_ByUserName()
        {

            CabinetUserDTO u = userService.GetDTO("email1");
            
            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);
                      
        }

        [TestMethod]
        public void CabinetUserService_Get()
        {
            CabinetUser u = userService.Get(1);

            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);                      
        }

       
    }
}
