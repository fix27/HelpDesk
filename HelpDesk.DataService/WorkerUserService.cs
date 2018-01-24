using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System;
using System.Linq;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using System.Linq.Expressions;
using HelpDesk.Common.Aspects;
using System.Collections.Generic;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с пользователями исполнителями/диспетчерами
    /// </summary>
    [Cache]
    public class WorkerUserService : BaseService, IWorkerUserService
    {
        
        private readonly IBaseRepository<WorkerUser>    userRepository;
        private readonly IBaseRepository<UserSession>   userSessionRepository;
        private readonly IBaseRepository<Worker>        workerRepository;
        private readonly ISettingsService               settingsService;
        private readonly IDateTimeService               dateTimeService;
        private readonly IBaseRepository<WorkerUserEventSubscribe> userEventSubscribeRepository;
        private readonly IBaseRepository<StatusRequest> statusRepository;
        private readonly IRepository                    repository;
        
        public WorkerUserService(IBaseRepository<WorkerUser> userRepository,
            IBaseRepository<UserSession> userSessionRepository,
            IBaseRepository<Worker> workerRepository,
            ISettingsService settingsService,
            IDateTimeService dateTimeService,
            IBaseRepository<WorkerUserEventSubscribe> userEventSubscribeRepository,
            IBaseRepository<StatusRequest> statusRepository,
            IRepository repository)
        {
            this.userRepository = userRepository;
            this.userSessionRepository = userSessionRepository;
            this.workerRepository = workerRepository;
            this.settingsService = settingsService;
            this.dateTimeService = dateTimeService;
            this.userEventSubscribeRepository = userEventSubscribeRepository;
            this.statusRepository = statusRepository;
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

        /// <summary>
        /// События заявки, на которые подписан пользователь
        /// </summary>
        [Cache(CacheKeyTemplate = "IEnumerable<RawStatusRequestDTO>(userId={0})")]
        public IEnumerable<RawStatusRequestDTO> GetListSubscribeStatus(long userId)
        {
            IEnumerable<RawStatusRequestDTO> list = statusRepository.GetList(t => !RequestService.IgnoredRawRequestStates.Contains(t.Id))
                .OrderBy(s => s.Name)
                .Select(s => new RawStatusRequestDTO { Id = s.Id, Name = s.Name })
                .ToList();

            IEnumerable<WorkerUserEventSubscribe> listSubscribe = userEventSubscribeRepository.GetList(e => e.User.Id == userId).ToList();

            var q = from s in list
                    join ss in listSubscribe on s.Id equals ss.StatusRequest.Id into jss
                    from ss in jss.DefaultIfEmpty()
                    select
                    new RawStatusRequestDTO { Id = s.Id, Name = s.Name, Checked = ss != null };

            return q.ToList();
        }

        /// <summary>
        /// Подписка/отписка пользователя на события заявки
        /// </summary>
        [Cache(Invalidate = true, InvalidateCacheKeyTemplates = "IEnumerable<RawStatusRequestDTO>(userId={0})")]
        public void ChangeSubscribeRequestState(long userId, long requestStateId)
        {
            WorkerUserEventSubscribe subscribe = userEventSubscribeRepository.Get(s => s.User.Id == userId && s.StatusRequest.Id == requestStateId);
            if (subscribe == null)
            {
                subscribe = new WorkerUserEventSubscribe()
                {
                    StatusRequest = statusRepository.Get(requestStateId),
                    User = userRepository.Get(userId)
                };
                userEventSubscribeRepository.Save(subscribe);
            }
            else
            {
                userEventSubscribeRepository.Delete(subscribe);
            }

            repository.SaveChanges();
        }

        /// <summary>
        /// Подписка/отписка пользователя на E-mail-рассылку
        /// </summary>
        [Cache(Invalidate = true, InvalidateCacheKeyTemplates = "IEnumerable<RawStatusRequestDTO>(userId={0})")]
        public void ChangeSubscribe(long userId)
        {
            WorkerUser user = userRepository.Get(userId);
            user.Subscribe = !user.Subscribe;
            userRepository.Save(user);

            repository.SaveChanges();
        }
    }
}
