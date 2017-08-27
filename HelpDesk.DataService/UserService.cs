using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System;
using System.Linq;
using HelpDesk.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using System.Linq.Expressions;
using HelpDesk.Common.Aspects;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Query;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с пользователями
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        
        private readonly IBaseRepository<User>          userRepository;
        private readonly IBaseRepository<PersonalProfile> personalProfileRepository;
        private readonly ISettingsRepository            settingsRepository;
        private readonly IRepository                    repository;

        public UserService(IBaseRepository<User> userRepository,
            IBaseRepository<PersonalProfile>    personalProfileRepository,
            ISettingsRepository                 settingsRepository,
            IRepository                         repository)
        {
            this.userRepository         = userRepository;
            this.personalProfileRepository = personalProfileRepository;
            this.settingsRepository     = settingsRepository;
            this.repository             = repository;
        }



        private UserDTO getUserDTO(Expression<Func<User, bool>> predicate)
        {

            User user = userRepository.Get(predicate);
            if (user == null)
                return null;

            UserDTO dto = new UserDTO()
            {
                Email = user.Email,
                Password = user.Password,
                Id = user.Id,
                FM = user.PersonalProfile != null ? user.PersonalProfile.FM : null,
                IM = user.PersonalProfile != null ? user.PersonalProfile.IM : null,
                OT = user.PersonalProfile != null ? user.PersonalProfile.OT : null

            };
            return dto;
        }

        public User Get(long id)
        {
            return userRepository.Get(id);
        }

        public UserDTO GetDTO(long id)
        {
            return getUserDTO(u => u.Id == id);
        }

        public UserDTO GetDTO(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                return null;

            if (userName.IndexOf("@") > 0)
                return getUserDTO(u => u.Email == userName);

            return getUserDTO(u => u.Email.StartsWith(userName + "@"));
        }

        [Transaction]
        public void Create(string email, string password)
        {

            UserDTO existsUser = getUserDTO(u => u.Email == email);
            if (existsUser!=null)
                setErrorMsg("Email", Resource.UniqueEmailConstraintMsg);

            if(!FormatConfirm.IsEmail(email))
                setErrorMsg("Email", Resource.EmailConstraintMsg);

            
            checkStringConstraint("Password", password, true, 100, 5);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            
            User user = new User()
            {
                 Email      = email,
                 Password   = password
            };

            userRepository.Save(user);
            
            repository.SaveChanges();
        }
        
                
    }
}
