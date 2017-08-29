using System;
using System.Threading.Tasks;
using HelpDesk.DataService.Interface;
using Microsoft.AspNet.Identity;
using HelpDesk.DTO;

namespace HelpDesk.WorkerWebApp.Identity
{
    public class UserStore : IUserStore<ApplicationUser, long>
    {
        private readonly IWorkerUserService userService;
        public UserStore(IWorkerUserService userService)
        {
            this.userService = userService;
        }

        #region NotImplemented
        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
        #endregion NotImplemented

        public void Dispose()
        { }

        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            WorkerUserDTO u = userService.GetDTO(userId);

            if (u != null)
            {
                

                Task<ApplicationUser> t=  Task.Run(() => new ApplicationUser()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Password = u.Password,
                    Email = u.Email
                });

            }

            return Task.Run(() => (ApplicationUser)null);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            WorkerUserDTO u = userService.GetDTO(userName);

            if (u != null)
            {
                return Task.Run(() => new ApplicationUser()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Password = u.Password,
                    Email = u.Email
                });
            }

            return Task.Run(() => (ApplicationUser)null);
        }

        


    }


}