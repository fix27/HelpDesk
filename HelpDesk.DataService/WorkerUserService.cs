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
    /// Для работы с пользователями исполнителями/диспетчерами
    /// </summary>
    public class WorkerUserService : BaseService, IWorkerUserService
    {
        
        private readonly IBaseRepository<WorkerUser>    userRepository;
        private readonly IBaseRepository<UserSession>   userSessionRepository;
        private readonly IBaseRepository<Worker>        workerRepository;
        private readonly ISettingsRepository            settingsRepository;
        private readonly IDateTimeService               dateTimeService;
        private readonly IRepository                    repository;

        public WorkerUserService(IBaseRepository<WorkerUser> userRepository,
            IBaseRepository<UserSession> userSessionRepository,
            IBaseRepository<Worker> workerRepository,
            ISettingsRepository settingsRepository,
            IDateTimeService dateTimeService,
            IRepository repository)
        {
            this.userRepository = userRepository;
            this.userSessionRepository = userSessionRepository;
            this.workerRepository = workerRepository;
            this.settingsRepository = settingsRepository;
            this.dateTimeService = dateTimeService;
            this.repository = repository;
        }



        private WorkerUserDTO getUserDTO(Expression<Func<WorkerUser, bool>> predicate)
        {

            WorkerUser user = userRepository.Get(predicate);
            if (user == null)
                return null;

            WorkerUserDTO dto = new WorkerUserDTO()
            {
                Email = user.Email,
                Password = user.Password,
                Id = user.Id,
                Name = user.Name,
                Worker = user.Worker,
                UserType = user.UserType

            };
            return dto;
        }

        public WorkerUser Get(long id)
        {
            return userRepository.Get(id);
        }

        public WorkerUserDTO GetDTO(long id)
        {
            return getUserDTO(u => u.Id == id);
        }

        public WorkerUserDTO GetDTO(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
                return null;

            if (userName.IndexOf("@") > 0)
                return getUserDTO(u => u.Email.ToUpper() == userName.ToUpper());

            return getUserDTO(u => u.Email.ToUpper().StartsWith(userName.ToUpper() + "@"));
        }

        [Transaction]
        public void Create(string email, string password)
        {

            WorkerUserDTO existsUser = getUserDTO(u => u.Email.ToUpper() == email.ToUpper());
            if (existsUser!=null)
                setErrorMsg("Email", Resource.UniqueEmailConstraintMsg);

            if(!FormatConfirm.IsEmail(email))
                setErrorMsg("Email", Resource.EmailConstraintMsg);

            
            checkStringConstraint("Password", password, true, 100, 5);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);


            WorkerUser user = new WorkerUser()
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
                ApplicationType = ApplicationType.Worker,
                UserId = userId,
                DateInsert = dateTimeService.GetCurrent(),
                IP = ip
            });
            repository.SaveChanges();
        }

        public void ChangeSubscribeRequestState(long requestStateId, bool add)
        {

        }
    }
}
