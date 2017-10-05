using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using System.Linq.Expressions;
using HelpDesk.Common.Aspects;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с пользователями личного кабинета
    /// </summary>
    public class CabinetUserService : BaseService, ICabinetUserService
    {
        
        private readonly IBaseRepository<CabinetUser>   userRepository;
        private readonly IBaseRepository<UserSession>   userSessionRepository;
        private readonly IBaseRepository<Employee>      employeeRepository;
        private readonly ISettingsRepository            settingsRepository;
        private readonly IDateTimeService               dateTimeService;
        private readonly IRepository                    repository;

        public CabinetUserService(IBaseRepository<CabinetUser> userRepository,
            IBaseRepository<UserSession> userSessionRepository,
            IBaseRepository<Employee> employeeRepository,
            ISettingsRepository settingsRepository,
            IDateTimeService dateTimeService,
            IRepository repository)
        {
            this.userRepository = userRepository;
            this.userSessionRepository = userSessionRepository;
            this.employeeRepository = employeeRepository;
            this.settingsRepository = settingsRepository;
            this.dateTimeService = dateTimeService;
            this.repository = repository;
        }



        private CabinetUserDTO getUserDTO(Expression<Func<CabinetUser, bool>> predicate)
        {

            CabinetUser user = userRepository.Get(predicate);
            if (user == null)
                return null;

            CabinetUserDTO dto = new CabinetUserDTO()
            {
                Email = user.Email,
                Password = user.Password,
                Id = user.Id,
                FM = user.Employee != null ? user.Employee.FM : null,
                IM = user.Employee != null ? user.Employee.IM : null,
                OT = user.Employee != null ? user.Employee.OT : null

            };
            return dto;
        }

        public CabinetUser Get(long id)
        {
            return userRepository.Get(id);
        }

        public CabinetUserDTO GetDTO(long id)
        {
            return getUserDTO(u => u.Id == id);
        }

        public CabinetUserDTO GetDTO(string userName)
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

            CabinetUserDTO existsUser = getUserDTO(u => u.Email == email);
            if (existsUser!=null)
                setErrorMsg("Email", Resource.UniqueEmailConstraintMsg);

            if(!FormatConfirm.IsEmail(email))
                setErrorMsg("Email", Resource.EmailConstraintMsg);

            
            checkStringConstraint("Password", password, true, 100, 5);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            
            CabinetUser user = new CabinetUser()
            {
                 Email      = email,
                 Password   = password
            };

            userRepository.Save(user);
            
            repository.SaveChanges();
        }


        public void SaveStartSessionFact(long userId, string ip)
        {
            userSessionRepository.Save(new UserSession()
                {
                    ApplicationType = ApplicationType.Cabinet,
                    UserId = userId,
                    DateInsert = dateTimeService.GetCurrent(),
                    IP = ip
                });
            repository.SaveChanges();
        }
                
    }
}
