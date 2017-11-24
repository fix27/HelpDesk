using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using HelpDesk.DataService.Interface;
using HelpDesk.CabinetWebApp.App_Start;
using Unity.ServiceLocation;

namespace HelpDesk.CabinetWebApp.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser, long>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, long> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            UserStore userStore = new UserStore(new UnityServiceLocator(UnityConfig.GetConfiguredContainer()).GetInstance<ICabinetUserService>());
            var manager = new ApplicationUserManager(userStore);

            return manager;
        }
        
    }
}