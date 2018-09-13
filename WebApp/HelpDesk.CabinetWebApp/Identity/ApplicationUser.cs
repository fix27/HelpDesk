using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HelpDesk.CabinetWebApp.Identity
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IUser<long>
    {
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
                        
            return userIdentity;
        }

        public string Password { get; set; }
        public string UserName { get; set; }
        
        public long Id { get; set; }
        public string Email { get; set; }

    }

    
}