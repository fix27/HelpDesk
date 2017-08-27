using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpDesk.DataService.Interface;
using HelpDesk.DataService;
using HelpDesk.Data.Repository;
using Moq;
using HelpDesk.Entity;
using HelpDesk.DTO;
using System;
using System.Linq.Expressions;

namespace HelpDesk.Test.DataService
{
    /// <summary>
    /// Тесты методов UserService
    /// </summary>
    [TestClass]
    public class UserServiceTest
    {
        IList<User> listUser = new List<User>()
        {
            new User {Id = 1, Email="email1@mail.ru" },
            new User {Id = 2, Email="email2@mail.ru" },
            new User {Id = 3, Email="email3@mail.ru" }
        };

        IList<PersonalProfile> listPersonalProfile = new List<PersonalProfile>()
        {
            new PersonalProfile {Id = 1, User = new User {Id = 1, Email="email1@mail.ru" } },
            new PersonalProfile {Id = 2, User = new User {Id = 2, Email="email2@mail.ru" } },
            new PersonalProfile {Id = 3, User = new User {Id = 3, Email="email3@mail.ru" } }
        };


        IUserService userService;
        public UserServiceTest()
        {
            Mock<IBaseRepository<User>> mockUserRepository = new Mock<IBaseRepository<User>>(MockBehavior.Strict);
            mockUserRepository.Setup(x => x.Get(It.IsAny<long>()))
               .Returns((long id) => { return listUser.FirstOrDefault(t => t.Id == id); });
            mockUserRepository.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>()))
               .Returns((Expression<Func<User, bool>> predicate) => { return listUser.AsQueryable().FirstOrDefault(predicate); });
            mockUserRepository.Setup(x => x.GetList())
               .Returns(() => { return listUser.AsQueryable(); });

            Mock<IBaseRepository<PersonalProfile>> mockPersonalProfileRepository = new Mock<IBaseRepository<PersonalProfile>>(MockBehavior.Strict);
            mockPersonalProfileRepository.Setup(x => x.GetList())
                .Returns(() => { return listPersonalProfile.AsQueryable(); });


            userService = new UserService(
                mockUserRepository.Object,
                mockPersonalProfileRepository.Object,
                Mock.Of<ISettingsRepository>(),
                Mock.Of<IRepository>());
        }
       

        [TestMethod]
        public void UserService_GetDTO_ByUserId()
        {
            UserDTO u = userService.GetDTO(1);

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
        public void UserService_GetDTO_ByUserEmail()
        {
            
            UserDTO u = userService.GetDTO("email1@mail.ru");

            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);
                      
        }

        [TestMethod]
        public void UserService_GetDTO_ByUserName()
        {

            UserDTO u = userService.GetDTO("email1");
            
            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);
                      
        }

        [TestMethod]
        public void UserService_Get()
        {
            User u = userService.Get(1);

            Assert.AreEqual(u.Id, listUser[0].Id);
            Assert.AreEqual(u.Email, listUser[0].Email);                      
        }

       
    }
}
