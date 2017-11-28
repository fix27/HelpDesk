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
using HelpDesk.DataService.Common.Interface;
using HelpDesk.DataService.Common.DTO;
using System.Collections.Generic;
using HelpDesk.Common.Helpers;
using HelpDesk.Data.Command;
using HelpDesk.DataService.Command;

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
        private readonly IBaseRepository<CabinetUserEventSubscribe> userEventSubscribeRepository;
        private readonly IBaseRepository<StatusRequest> statusRepository;
        private readonly IStatusRequestMapService       statusRequestMapService;
        private readonly ICommandRunner                 commandRunner;
        private readonly IRepository                    repository;

        public CabinetUserService(IBaseRepository<CabinetUser> userRepository,
            IBaseRepository<UserSession> userSessionRepository,
            IBaseRepository<Employee> employeeRepository,
            ISettingsRepository settingsRepository,
            IDateTimeService dateTimeService,
            IBaseRepository<CabinetUserEventSubscribe> userEventSubscribeRepository,
            IBaseRepository<StatusRequest> statusRepository,
            IStatusRequestMapService statusRequestMapService,
            ICommandRunner commandRunner,
            IRepository repository)
        {
            this.userRepository = userRepository;
            this.userSessionRepository = userSessionRepository;
            this.employeeRepository = employeeRepository;
            this.settingsRepository = settingsRepository;
            this.dateTimeService = dateTimeService;
            this.userEventSubscribeRepository = userEventSubscribeRepository;
            this.statusRepository = statusRepository;
            this.statusRequestMapService = statusRequestMapService;
            this.commandRunner = commandRunner;
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
                return getUserDTO(u => u.Email.ToUpper() == userName.ToUpper());

            return getUserDTO(u => u.Email.ToUpper().StartsWith(userName.ToUpper() + "@"));
        }

        [Transaction]
        public void Create(string email, string password)
        {

            CabinetUserDTO existsUser = getUserDTO(u => u.Email.ToUpper() == email.ToUpper());
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



        /// <summary>
        /// События заявки, на которые подписан пользователь
        /// </summary>
        public IEnumerable<StatusRequestDTO> GetListSubscribeStatus(long userId)
        {
            IEnumerable<RawStatusRequestDTO> list = statusRepository.GetList(t => !RequestService.IgnoredRawRequestStates.Contains(t.Id))
                .OrderBy(s => s.Name)
                .Select(s => new RawStatusRequestDTO { Id = s.Id, Name = s.Name })
                .ToList();

            IEnumerable<CabinetUserEventSubscribe> listSubscribe = userEventSubscribeRepository.GetList(e => e.User.Id == userId).ToList();

            var q = (from s in list
                     join ss in listSubscribe on s.Id equals ss.StatusRequest.Id into jss
                     from ss in jss.DefaultIfEmpty()
                     select new { s, Checked = (ss != null) })
                    .ToList()
                    .Select(t =>
                        new StatusRequestDTO
                        {
                            Id = statusRequestMapService.GetEquivalenceByElement(t.s.Id),
                            Name = statusRequestMapService.GetEquivalenceByElement(t.s.Id).GetDisplayName(),
                            Checked = t.Checked
                        });

            return q.Distinct();
        }

        /// <summary>
        /// Подписка/отписка пользователя на события заявки
        /// </summary>
        public void ChangeSubscribeRequestState(long userId, StatusRequestEnum requestState)
        {
            IEnumerable<long> statusRequestIds = statusRequestMapService.GetElementsByEquivalence(requestState);
            IEnumerable<CabinetUserEventSubscribe> subscribes = userEventSubscribeRepository.GetList(s => s.User.Id == userId 
                && statusRequestIds.Contains(s.StatusRequest.Id));
            
            if (!subscribes.Any())
            {
                foreach (long statusRequestId in statusRequestIds)
                {
                    var subscribe = new CabinetUserEventSubscribe()
                    {
                        StatusRequest = statusRepository.Get(statusRequestId),
                        User = userRepository.Get(userId)
                    };
                    userEventSubscribeRepository.Save(subscribe);
                }
                
            }
            else
            {
                commandRunner.Run(new DeleteCabinetUserEventSubscribeCommand(userId, statusRequestIds));
            }

            repository.SaveChanges();
        }

        /// <summary>
        /// Подписка/отписка пользователя на E-mail-рассылку
        /// </summary>
        public void ChangeSubscribe(long userId)
        {
            CabinetUser user = userRepository.Get(userId);
            user.Subscribe = !user.Subscribe;
            userRepository.Save(user);

            repository.SaveChanges();
        }


    }
}
