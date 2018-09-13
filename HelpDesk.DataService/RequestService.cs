using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System;
using System.Linq;
using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Query;
using HelpDesk.DataService.DTO.FileUpload;
using HelpDesk.Common.Helpers;
using HelpDesk.Common.Aspects;
using System.Linq.Expressions;
using HelpDesk.DataService.DTO.Parameters;
using HelpDesk.EventBus.Common.Interface;
using HelpDesk.EventBus.Common.AppEvents;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.DataService.Common.Interface;
using HelpDesk.DataService.Common.DTO;
using HelpDesk.Common.Cache;

namespace HelpDesk.DataService
{

    /// <summary>
    /// Для работы с заявками
    /// </summary>
    [Transaction]
    [Cache]
    public class RequestService : BaseService, IRequestService
    {
        /// <summary>
        /// Состояния заявок, которые не отображаются в UI
        /// </summary>
        public readonly static long[] IgnoredRawRequestStates = new long[]
        {
            (long)RawStatusRequestEnum.DateEnd
        };

        /// <summary>
        /// Архивные состояния
        /// </summary>
        private readonly long[] arсhiveRawRequestStates = new long[]
        {
            (long)RawStatusRequestEnum.ApprovedRejected,
            (long)RawStatusRequestEnum.ApprovedComplete,
            (long)RawStatusRequestEnum.Passive
        };

        private readonly IQueryHandler queryHandler;
        private readonly IBaseRepository<RequestObject> objectRepository;
        private readonly IBaseRepository<DescriptionProblem> descriptionProblemRepository;
        private readonly ISettingsService settingsService;
        private readonly IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository;
        private readonly IBaseRepository<Employee> employeeRepository;
        private readonly IBaseRepository<StatusRequest> statusRepository;
        private readonly IBaseRepository<Request> requestRepository;
        private readonly IBaseRepository<RequestArch> requestArchRepository;
        private readonly IBaseRepository<RequestEvent> requestEventRepository;
        private readonly IBaseRepository<RequestEventArch> requestEventArchRepository;
        private readonly IBaseRepository<RequestFile> requestFileRepository;
        private readonly IBaseRepository<Worker> workerRepository;
        private readonly IBaseRepository<WorkerUser> workerUserRepository;
        private readonly IRepository repository;
        private readonly IDateTimeService dateTimeService;
        private readonly IRequestConstraintsService requestConstraintsService;
        private readonly IStatusRequestMapService statusRequestMapService;
        private readonly IBaseRepository<AccessWorkerUser> accessWorkerUserRepository;
        private readonly IAccessWorkerUserExpressionService accessWorkerUserExpressionService;
        private readonly IQueue<IRequestAppEvent> queue;

		private readonly IQuery<DescriptionProblemQueryParam, IEnumerable<SimpleDTO>> _descriptionProblemQuery;
		private readonly IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>> _requestLastEventQuery;
		private readonly IQuery<ArchiveYearQueryParam, IEnumerable<Year>> _archiveYearQuery;
		private readonly IQuery<EmployeeArchiveYearQueryParam, IEnumerable<Year>> _employeeArchiveYearQuery;
		private readonly IQuery<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>> _requestStateCountQuery;
		private readonly IEmployeeRequestQuery<Request> _employeeRequestQuery;
		private readonly IEmployeeRequestQuery<RequestArch> _employeeRequestArchQuery;
		private readonly IRequestQuery<Request> _requestQuery;
		private readonly IRequestQuery<RequestArch> _requestArchQuery;

		public RequestService(IQueryHandler queryHandler,

			IQuery<DescriptionProblemQueryParam, IEnumerable<SimpleDTO>> descriptionProblemQuery,
			IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>> requestLastEventQuery,
			IQuery<ArchiveYearQueryParam, IEnumerable<Year>> archiveYearQuery,
			IQuery<EmployeeArchiveYearQueryParam, IEnumerable<Year>> employeeArchiveYearQuery,
			IQuery<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>> requestStateCountQuery,
			IEmployeeRequestQuery<Request> employeeRequestQuery,
			IEmployeeRequestQuery<RequestArch> employeeRequestArchQuery,
			IRequestQuery<Request> requestQuery,
			IRequestQuery<RequestArch> requestArchQuery,

			IBaseRepository<RequestObject> objectRepository,
            IBaseRepository<DescriptionProblem> descriptionProblemRepository,
            ISettingsService settingsService,
            IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<StatusRequest> statusRepository,
            IBaseRepository<Request> requestRepository,
            IBaseRepository<RequestArch> requestArchRepository,
            IBaseRepository<RequestEvent> requestEventRepository,
            IBaseRepository<RequestEventArch> requestEventArchRepository,
            IBaseRepository<RequestFile> requestFileRepository,
            IBaseRepository<Worker> workerRepository,
            IBaseRepository<WorkerUser> workerUserRepository,
            IRepository repository,
            IDateTimeService dateTimeService,
            IRequestConstraintsService requestConstraintsService,
            IStatusRequestMapService statusRequestMapService,
            IBaseRepository<AccessWorkerUser> accessWorkerUserRepository,
            IAccessWorkerUserExpressionService accessWorkerUserExpressionService,
            IQueue<IRequestAppEvent> queue,
            ICache memoryCache)
        {
            this.queryHandler            = queryHandler;

			_descriptionProblemQuery = descriptionProblemQuery;
			_requestLastEventQuery = requestLastEventQuery;
			_archiveYearQuery = archiveYearQuery;
			_employeeArchiveYearQuery = employeeArchiveYearQuery;
			_requestStateCountQuery = requestStateCountQuery;
			_employeeRequestQuery = employeeRequestQuery;
			_employeeRequestArchQuery = employeeRequestArchQuery;
			_requestQuery = requestQuery;
			_requestArchQuery = requestArchQuery;

			this.objectRepository       = objectRepository;
            this.descriptionProblemRepository = descriptionProblemRepository;
            this.settingsService     = settingsService;
            this.organizationObjectTypeWorkerRepository = organizationObjectTypeWorkerRepository;
            this.employeeRepository = employeeRepository;
            this.statusRepository       = statusRepository;
            this.requestRepository      = requestRepository;
            this.requestArchRepository  = requestArchRepository;
            this.requestEventRepository = requestEventRepository;
            this.requestEventArchRepository = requestEventArchRepository;
            this.requestFileRepository  = requestFileRepository;
            this.workerRepository       = workerRepository;
            this.workerUserRepository   = workerUserRepository;
            this.repository             = repository;
            this.dateTimeService        = dateTimeService;
            this.requestConstraintsService = requestConstraintsService;
            this.statusRequestMapService = statusRequestMapService;
            this.accessWorkerUserRepository = accessWorkerUserRepository;
            this.accessWorkerUserExpressionService = accessWorkerUserExpressionService;
            this.queue          = queue;           
        }
        
        private RequestParameter getCreateOrUpdateRequest(long id)
        {
            BaseRequest request = requestRepository.Get(id);
            if (request == null)
                request = requestArchRepository.Get(id);

            if (request == null)
                throw new DataServiceException(Resource.NoDataFoundMsg);

            
            return new RequestParameter()
            {
                Id = request.Id,
                ObjectId = request.Object.Id,
                ObjectName = RequestObjectDTO.GetObjectName(request.Object.ObjectType.Name,
                    request.Object.SoftName,
                    request.Object.HardType,
                    request.Object.Model),
                TempRequestKey = Guid.NewGuid(),
                DescriptionProblem = request.DescriptionProblem,
                EmployeeId = request.Employee.Id,
                EmployeeInfo = EmployeeDTO.GetEmployeeInfo(request.Employee.FM, request.Employee.IM, request.Employee.OT, request.Employee.Phone, 
                    request.Employee.Organization.Name, request.Employee.Organization.Address),
                Version = request.Version
            };
        }

        /// <summary>
        /// Параметры для создания (id = 0)/редактирования заявки
        /// </summary>
        public RequestParameter Get(long id = 0)
        {
            if(id == 0)
                return new RequestParameter()
                {
                    TempRequestKey = Guid.NewGuid()
                };

            return getCreateOrUpdateRequest(id);
        }

        /// <summary>
        /// Параметры для создания новой заявки на основе существующей заявки
        /// </summary>
        public RequestParameter GetNewByRequestId(long requestId)
        {
            RequestParameter r = getCreateOrUpdateRequest(requestId);
            r.Id = 0;
            r.ByRequestId = requestId;
            return r;

        }

        /// <summary>
        /// Параметры для создания новой заявки на основе существующего объекта (ПО/ТО)
        /// </summary>
        public RequestParameter GetNewByObjectId(long objectId)
        {
            RequestObject edsObject = objectRepository.Get(objectId);

            return new RequestParameter()
            {
                ObjectId = objectId,
                ObjectName = RequestObjectDTO.GetObjectName(edsObject.ObjectType.Name,
                    edsObject.SoftName,
                    edsObject.HardType,
                    edsObject.Model),
                TempRequestKey = Guid.NewGuid()
            };
        }

        private IDictionary<long, IEnumerable<StatusRequest>> getGraphState(IEnumerable<long> allowableUserStates)
        {
            IEnumerable<StatusRequest> statuses = statusRepository.GetList().ToList();
            IDictionary<long, IEnumerable<StatusRequest>> graphState = new Dictionary<long, IEnumerable<StatusRequest>>();
            foreach (StatusRequest status in statuses)
                if (status.AllowableStates != null)
                {
                    graphState[status.Id] = statuses.Where(s => status.AllowableStates.ToEnumerable<long>().Contains(s.Id));
                    graphState[status.Id] = graphState[status.Id].Where(t => allowableUserStates.Contains(t.Id));
                }
                    

            return graphState;
        }

        /// <summary>
        /// Удаление заявки
        /// </summary>
        public void Delete(long id)
        {
            requestConstraintsService.CheckExistsRequest(id);

            requestFileRepository.Delete(f => f.RequestId == id);
            requestRepository.Delete(id);
            repository.SaveChanges();
        }

        #region GetList
        public delegate IEnumerable<RequestDTO> GetListRequestDelegate(PageInfo pageInfo);
        private IEnumerable<RequestDTO> getList(GetListRequestDelegate getListActive, GetListRequestDelegate getListArchive, 
            RequestFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
        {
            IEnumerable<RequestDTO> list = null;

            if (filter.Ids != null && filter.Ids.Any())
            {
                PageInfo pageInfoActive = new PageInfo()
                {
                    CurrentPage = 0,
                    PageSize = int.MaxValue
                };

                PageInfo pageInfoArchive = new PageInfo()
                {
                    CurrentPage = 0,
                    PageSize = int.MaxValue
                };

                IEnumerable<RequestDTO> listActive = getListActive(pageInfoActive);
                IEnumerable<RequestDTO> listArchive = getListArchive(pageInfoArchive);
                foreach (RequestDTO r in listArchive)
                    r.Archive = true;

                pageInfo.Count = pageInfoActive.Count + pageInfoArchive.Count;
                pageInfo.TotalCount = pageInfoActive.TotalCount + pageInfoArchive.TotalCount;

                list = listActive.Union(listArchive);

            }
            else if (filter.Archive)
            {
                list = getListArchive(pageInfo);
                foreach (RequestDTO r in list)
                    r.Archive = true;
            }
            else
            {
                list = getListActive(pageInfo);
            }

            IEnumerable<long> requestIds = list.Select(r => r.Id).ToList();
            #region files 
            IEnumerable<RequestFileInfoDTO> files = requestFileRepository.GetList(t => t.RequestId != null && requestIds.Contains(t.RequestId.Value))
                .Select(t => new RequestFileInfoDTO()
                {
                    Id = t.Id,
                    ForignKeyId = t.RequestId,
                    Name = t.Name,
                    Size = t.Size,
                    TempRequestKey = t.TempRequestKey,
                    Type = t.Type
                });

            IDictionary<long, IList<RequestFileInfoDTO>> fileIndex = new Dictionary<long, IList<RequestFileInfoDTO>>();
            foreach (RequestFileInfoDTO r in files)
            {
                if (fileIndex.ContainsKey(r.ForignKeyId.Value))
                    fileIndex[r.ForignKeyId.Value].Add(r);
                else
                    fileIndex[r.ForignKeyId.Value] = new List<RequestFileInfoDTO>() { r };
            }
            #endregion files

            #region LastEvent
            //IEnumerable<RequestEventDTO> events = queryHandler.Run(new RequestLastEventQuery(requestIds));


			var events = queryHandler.Handle<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>, IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>>>
				(new RequestLastEventQueryParam
				{
					RequestIds = requestIds
				}, _requestLastEventQuery);

			IDictionary<long, RequestEventDTO> eventIndex = new Dictionary<long, RequestEventDTO>();
            foreach (RequestEventDTO e in events)
            {
                eventIndex[e.RequestId] = e;
            }
            #endregion LastEvent

            foreach (RequestDTO r in list)
            {
                r.Files = fileIndex.ContainsKey(r.Id) ? fileIndex[r.Id] : null;
                r.LastEvent = eventIndex.ContainsKey(r.Id) ? eventIndex[r.Id] : null;
                r.StatusRequest = statusRequestMapService.GetEquivalenceByElement(r.Status.Id);
            }
            return list;
        }
        
        /// <summary>
        /// Заявки пользователя личного кабинета
        /// </summary>
        public IEnumerable<RequestDTO> GetListByEmployee(long employeeId, RequestFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
        {
            if (filter.StatusIds != null && filter.StatusIds.Any())
            {
                filter.RawStatusIds = new List<long>();
                foreach (StatusRequestEnum s in filter.StatusIds)
                    foreach (long statuId in statusRequestMapService.GetElementsByEquivalence(s))
                        filter.RawStatusIds.Add(statuId);
            }
            
            return getList(
                delegate (PageInfo _pageInfo)
                {
                    return queryHandler.Handle<EmployeeRequestQueryParam, IEnumerable<RequestDTO>, IEmployeeRequestQuery<Request>>
					(new EmployeeRequestQueryParam
					{
						 EmployeeId = employeeId,
						  Filter = filter,
						   OrderInfo = orderInfo,
						    PageInfo = _pageInfo
					}, _employeeRequestQuery);
                },
                delegate (PageInfo _pageInfo)
                {
					return queryHandler.Handle<EmployeeRequestQueryParam, IEnumerable<RequestDTO>, IEmployeeRequestQuery<RequestArch>>
					(new EmployeeRequestQueryParam
					{
						EmployeeId = employeeId,
						Filter = filter,
						OrderInfo = orderInfo,
						PageInfo = _pageInfo
					}, _employeeRequestArchQuery);
				},
                filter, orderInfo, pageInfo);

        }

        /// <summary>
        /// Заявки Исполнителя/Диспетчера
        /// </summary>
        public IEnumerable<RequestDTO> GetList(long userId, RequestFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
        {
            Expression<Func<BaseRequest, bool>> accessPredicate = accessWorkerUserExpressionService
                .GetAccessRequestPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));

            IEnumerable<RequestDTO> list = getList(
                delegate (PageInfo _pageInfo)
                {
					return queryHandler.Handle<RequestQueryParam, IEnumerable<RequestDTO>, IRequestQuery<Request>>
					(new RequestQueryParam
					{
						AccessPredicate = accessPredicate,
						Filter = filter,
						OrderInfo = orderInfo,
						PageInfo = _pageInfo
					}, _requestQuery);
				},
                delegate (PageInfo _pageInfo)
                {
					return queryHandler.Handle<RequestQueryParam, IEnumerable<RequestDTO>, IRequestQuery<RequestArch>>
					(new RequestQueryParam
					{
						AccessPredicate = accessPredicate,
						Filter = filter,
						OrderInfo = orderInfo,
						PageInfo = _pageInfo
					}, _requestArchQuery);
				},
                filter, orderInfo, pageInfo);

            if(filter.Archive)
                return list;

            #region AllowableStates
            WorkerUser user = workerUserRepository.Get(userId);
            IEnumerable<long> allowableUserStates = user.UserType.AllowableStates.ToEnumerable<long>();
            IDictionary<long, IEnumerable<StatusRequest>> graphState = getGraphState(allowableUserStates);
            
            foreach (RequestDTO r in list)
                if(graphState.ContainsKey(r.Status.Id))
                    r.AllowableStates = graphState[r.Status.Id];
            #endregion AllowableStates
            
            return list;

        }
        #endregion GetList

        [Cache(CacheKeyTemplate = "IEnumerable<StatusRequestDTO>(archive={0})")]
        public IEnumerable<StatusRequestDTO> GetListStatus(bool archive)
        {
            IEnumerable<StatusRequestDTO> list = statusRepository.GetList(t => !IgnoredRawRequestStates.Contains(t.Id)).OrderBy(s => s.Name)
                .ToList()
                .Select(s => new StatusRequestDTO()
                {
                    Id = statusRequestMapService.GetEquivalenceByElement(s.Id),
                    Name = statusRequestMapService.GetEquivalenceByElement(s.Id).GetDisplayName()
                });

            IEnumerable<StatusRequestEnum> archiveStates = new List<StatusRequestEnum>()
                {
                    StatusRequestEnum.ApprovedComplete,
                    StatusRequestEnum.Passive
                };

            return list
                .Where(t => (archive) ? archiveStates.Contains(t.Id) : !archiveStates.Contains(t.Id))
                .Distinct()
                .ToList();

        }

        [Cache(CacheKeyTemplate = "IEnumerable<StatusRequest>(archive={0})")]
        public IEnumerable<StatusRequest> GetListRawStatus(bool archive)
        {
            IEnumerable<StatusRequest> list = statusRepository.GetList(t => !IgnoredRawRequestStates.Contains(t.Id))
                .OrderBy(s => s.Name)
                .ToList();

            return list
                .Where(t => (archive) ? arсhiveRawRequestStates.Contains(t.Id) : !arсhiveRawRequestStates.Contains(t.Id))
                .ToList();
        }
        
        /// <summary>
        /// Количество заявок в состоянии, для которого требуется подтверждение Заявителя
        /// </summary>
        public int GetCountRequiresConfirmationEmployee(long employeeId)
        {
            return requestRepository.Count(t => t.Employee.Id == employeeId && 
                t.Status.Id == (long)RawStatusRequestEnum.Closing);
        }

        /// <summary>
        /// Количество заявок в состоянии, для которого требуется реакция Исполнителя/Диспетчера
        /// </summary>
        public int GetCountRequiresReaction(long userId)
        {
            int c = 0;
            WorkerUser user = workerUserRepository.Get(userId);
            Expression<Func<BaseRequest, bool>> accessPredicate = null;
            switch (user.UserType.TypeCode)
            {
                case TypeWorkerUserEnum.Worker:
                    c = requestRepository.Count(t => t.Worker.Id == user.Worker.Id &&
                        (t.Status.Id == (long)RawStatusRequestEnum.New ||
                         t.Status.Id == (long)RawStatusRequestEnum.NotApprovedComplete));
                    break;
                    
                case TypeWorkerUserEnum.Dispatcher:
                    accessPredicate = accessWorkerUserExpressionService
                        .GetAccessRequestPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));
                    c = requestRepository.GetList()
                        .Where(accessPredicate)
                        .Count(t => (t.Status.Id == (long)RawStatusRequestEnum.Closing ||
                            t.Status.Id == (long)RawStatusRequestEnum.Rejected ||
                            t.Status.Id == (long)RawStatusRequestEnum.RejectedAfterAccepted));
                    break;

                case TypeWorkerUserEnum.WorkerAndDispatcher:
                    accessPredicate = accessWorkerUserExpressionService
                        .GetAccessRequestPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));
                    c = requestRepository.GetList()
                        .Where(accessPredicate)
                        .Count(t => (t.Status.Id == (long)RawStatusRequestEnum.Closing ||
                            t.Status.Id == (long)RawStatusRequestEnum.Rejected ||
                            t.Status.Id == (long)RawStatusRequestEnum.RejectedAfterAccepted ||

                            t.Status.Id == (long)RawStatusRequestEnum.New ||
                            t.Status.Id == (long)RawStatusRequestEnum.NotApprovedComplete));
                    break;
            }
            
            return c;
        }

        /// <summary>
        /// Архивные года для пользователя личного кабинета
        /// </summary>
        public IEnumerable<Year> GetListEmployeeArchiveYear(long employeeId)
        {
			var list = queryHandler.Handle<EmployeeArchiveYearQueryParam, IEnumerable<Year>, IQuery<EmployeeArchiveYearQueryParam, IEnumerable<Year>>>
				(new EmployeeArchiveYearQueryParam
				{
					EmployeeId = employeeId
				}, _employeeArchiveYearQuery);

			return list;
        }

        /// <summary>
        /// Архивные года для Исполнителя/Диспетчера
        /// </summary>
        [Cache(CacheKeyTemplate = "IEnumerable<Year>(userId={0})", ExpirationSeconds = 100)]
        public IEnumerable<Year> GetListArchiveYear(long userId)
        {
            Expression<Func<BaseRequest, bool>> accessPredicate = accessWorkerUserExpressionService
                .GetAccessRequestPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));
            
			var list = queryHandler.Handle<ArchiveYearQueryParam, IEnumerable<Year>, IQuery<ArchiveYearQueryParam, IEnumerable<Year>>>
				(new ArchiveYearQueryParam
				{
					 AccessPredicate = accessPredicate
				}, _archiveYearQuery);

			return list;
        }

        /// <summary>
        /// События заявки
        /// </summary>
        [Cache(CacheKeyTemplate = "IEnumerable<RequestEventDTO>(requestId={0})", ExpirationSeconds = 1000)]
        public IEnumerable<RequestEventDTO> GetListRequestEvent(long requestId)
        {
            bool archive = false;
            BaseRequest request = requestRepository.Get(requestId);
            if (request == null)
            {
                request = requestArchRepository.Get(requestId);
                archive = true;
            }

            if (request == null)
                return null;
            
            IEnumerable<BaseRequestEvent> list = null;

            if (archive)
                list = requestEventArchRepository.GetList(t => t.RequestId == requestId)
                    .OrderBy(t => t.Id)
                    .ToList();
            else
                list = requestEventRepository.GetList(t => t.RequestId == requestId)
                    .OrderBy(t => t.Id)
                    .ToList();

            if(list != null)
                return list.Select(t => new RequestEventDTO
                {
                     DateEvent  = t.DateEvent,
                     Note       = t.Note,
                     DateEnd    = t.StatusRequest.Id == (long)RawStatusRequestEnum.DateEnd,
                     Status     = t.StatusRequest,
                     RequestId  = t.RequestId,
                     Transfer   = t.StatusRequest.Id == (long)RawStatusRequestEnum.ExtendedDeadLine,
                     StatusRequest = statusRequestMapService.GetEquivalenceByElement(t.StatusRequest.Id),
                     User = t.User              
                })
                .ToList();

            return null;
        }
                
        /// <summary>
        /// Сохранение заявки
        /// </summary>
        [Transaction]
        public long Save(RequestParameter dto)
        {
            Settings settings = settingsService.Get();
            if (dto.Id == 0)
            {
                DateTime currentDate = dateTimeService.GetCurrent();
                
                IList<Request> list = requestRepository.GetList(t => t.DateInsert.Date == currentDate.Date &&
                t.Employee.Id == dto.EmployeeId && t.User == null)
                    .OrderByDescending(t => t.Id)
                    .ToList();

                if (list != null && list.Any())
                {

                    if (list[0].DateInsert.AddMinutes(Convert.ToDouble(settings.MinInterval)) > currentDate)
                        throw new DataServiceException(String.Format(Resource.MinIntervalRequestConstraintMsg, settings.MinInterval.ToString("0.00")));

                    if (list.Count > settings.LimitRequestCount)
                        throw new DataServiceException(String.Format(Resource.LimitRequestCountConstraintMsg, settings.LimitRequestCount));
                }

            }


            checkStringConstraint("DescriptionProblem", dto.DescriptionProblem, true, 2000, 3);

            if (dto.ObjectId == 0)
                setErrorMsg("ObjectId", Resource.RequiredConstraintMsg);

            if (dto.EmployeeId == 0)
                setErrorMsg("EmployeeId", Resource.RequiredConstraintMsg);



            Employee employee = employeeRepository.Get(dto.EmployeeId);
            RequestObject requestObject = objectRepository.Get(dto.ObjectId);


            OrganizationObjectTypeWorker organizationObjectTypeWorker
                = organizationObjectTypeWorkerRepository.Get(t => t.Organization.Id == employee.Organization.Id && t.ObjectType.Id == requestObject.ObjectType.Id);

            if (organizationObjectTypeWorker == null)
                setErrorMsg("Worker", Resource.WorkerNotDefinedConstraintMsg);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            Request r = null;
            WorkerUser user = null;
            if (dto.UserId > 0)
                user = workerUserRepository.Get(dto.UserId);

            DateTime currentDateTime = dateTimeService.GetCurrent();
            if (dto.Id > 0)
            {
            
                requestConstraintsService.CheckExistsRequest(dto.Id);

				r = requestRepository.Get(dto.Id);

				if (r.Version > dto.Version)
					throw new DataServiceException(Resource.ConcurrencyConstraintMsg);

				r.DateUpdate = currentDateTime;
                r.DescriptionProblem = dto.DescriptionProblem;
                r.User = user;

                requestRepository.Save(r);
                
                requestFileRepository.Update(new { RequestId = dto.Id }, f => f.TempRequestKey == dto.TempRequestKey);

                repository.SaveChanges();
                return dto.Id;
            }

            
            StatusRequest newStatusRequest = statusRepository.Get((long)RawStatusRequestEnum.New);
            r = new Request()
            {
                CountCorrectionDateEndPlan = 0,
                DateEndPlan = dateTimeService.GetRequestDateEnd(currentDateTime, requestObject.ObjectType.CountHour),
                DateInsert = currentDateTime,
                DateUpdate = currentDateTime,
                DescriptionProblem = dto.DescriptionProblem,
                Worker = workerRepository.Get(organizationObjectTypeWorker.Worker.Id),
                Object = requestObject,
                Employee = employee,
                Status = newStatusRequest,
                User = user                
            };
            requestRepository.Save(r);

            RequestEvent newEvent = new RequestEvent()
            {
                StatusRequest = newStatusRequest,
                DateEvent = currentDateTime,
                DateInsert = currentDateTime,
                RequestId = r.Id,
                User = user
            };
            requestEventRepository.Save(newEvent);

            RequestEvent dateEndRequestEvent = new RequestEvent()
            {
                StatusRequest = statusRepository.Get((long)RawStatusRequestEnum.DateEnd),
                DateEvent = r.DateEndPlan,
                DateInsert = currentDateTime,
                RequestId = r.Id,
                User = user
            };
            requestEventRepository.Save(dateEndRequestEvent);

            #region DescriptionProblem
            DescriptionProblem descriptionProblem = null;
            if (r.Object.ObjectType.Soft)
            {
                descriptionProblem = descriptionProblemRepository.Get(t => t.Name.ToUpper() == r.DescriptionProblem.ToUpper() &&
                        t.RequestObject.Id == r.Object.Id);

                if (descriptionProblem == null)
                {
                    descriptionProblem = new DescriptionProblem()
                    {
                        Name = r.DescriptionProblem,
                        RequestObject = r.Object
                    };

                    descriptionProblemRepository.Save(descriptionProblem);
                }
            }
            else
            {
                descriptionProblem = descriptionProblemRepository.Get(t => t.Name.ToUpper() == r.DescriptionProblem.ToUpper() &&
                    t.HardType.Id == r.Object.HardType.Id);

                if (descriptionProblem == null)
                {
                    descriptionProblem = new DescriptionProblem()
                    {
                        Name = r.DescriptionProblem,
                        HardType = r.Object.HardType
                    };

                    descriptionProblemRepository.Save(descriptionProblem);
                }
            }
            #endregion DescriptionProblem

            requestFileRepository.Update(new { RequestId = r.Id }, f => f.TempRequestKey == dto.TempRequestKey);
            
            repository.SaveChanges();
                        
            queue.Push(new RequestAppEvent() { RequestEventId = newEvent.Id, Archive = false });

            return r.Id;
        }

        /// <summary>
        /// Создание события заявки (изменение состояния заявки)
        /// </summary>
        [Transaction]
        [Cache(Invalidate = true, SkippedParameterIndexes = new int[] {0}, 
            InvalidateCacheKeyTemplates = "IEnumerable<RequestEventDTO>(requestId={0})")]
        public void CreateRequestEvent(long userId, RequestEventParameter dto)
        {
            DateTime currentDate = dateTimeService.GetCurrent();
            Request request = requestRepository.Get(dto.RequestId);
            if (request.Version > dto.RequestVersion)
            {
                throw new DataServiceException(Resource.ConcurrencyConstraintMsg);
            }

            if(dto.StatusRequestId == (long)RawStatusRequestEnum.ExtendedDeadLine && !dto.NewDeadLineDate.HasValue)
                setErrorMsg("NewDeadLineDate", Resource.EmptyConstraintMsg);

            if (dto.NewDeadLineDate.HasValue && dto.NewDeadLineDate.Value.Date <= currentDate.Date)
                setErrorMsg("NewDeadLineDate", String.Format(Resource.NewDeadLineDateConstraintMsg, 
                    dto.NewDeadLineDate.Value.Date.ToShortDateString(), request.DateEndPlan.Date.ToShortDateString()));

            checkStringConstraint("Note", dto.Note,
                    (dto.StatusRequestId == (long)RawStatusRequestEnum.Rejected ||
                     dto.StatusRequestId == (long)RawStatusRequestEnum.RejectedAfterAccepted ||
                     dto.StatusRequestId == (long)RawStatusRequestEnum.NotApprovedComplete),
                    2000, 5);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);


            WorkerUser user = workerUserRepository.Get(userId);
            IEnumerable<long> allowableUserStates = user.UserType.AllowableStates.ToEnumerable<long>();
            IDictionary<long, IEnumerable<StatusRequest>> graphState = getGraphState(allowableUserStates);

            StatusRequest statusRequest = statusRepository.Get(dto.StatusRequestId);
            
			var lastEvent = queryHandler.Handle<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>, IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>>>
				(new RequestLastEventQueryParam
				{
					RequestIds = new long[] { dto.RequestId },
					WithDateEnd = false }, _requestLastEventQuery).FirstOrDefault();

			RequestEvent newEvent = new RequestEvent()
            {
                 RequestId  = request.Id,
                 DateInsert = currentDate,
                 DateEvent  = currentDate,
                 OrdGroup   = lastEvent.OrdGroup,
                 User       = user,
                 StatusRequest = statusRequest,
                 Note       = dto.Note
            };
            requestEventRepository.Save(newEvent);

            RequestEvent dateEndEvent = null;
            if (dto.StatusRequestId == (long)RawStatusRequestEnum.ExtendedDeadLine)
            {
                StatusRequest dateEndStatusRequest = statusRepository.Get((long)RawStatusRequestEnum.DateEnd);
                dateEndEvent = new RequestEvent()
                {
                    RequestId = request.Id,
                    DateInsert = currentDate,
                    DateEvent = dto.NewDeadLineDate.Value,
                    OrdGroup = lastEvent.OrdGroup + 1,
                    User = user,
                    StatusRequest = dateEndStatusRequest
                };
                request.CountCorrectionDateEndPlan++;
                request.DateEndPlan = dto.NewDeadLineDate.Value;
                requestEventRepository.Save(dateEndEvent);
            }
            else if (dto.StatusRequestId == (long)RawStatusRequestEnum.ExtendedConfirmation)
            {
                StatusRequest dateEndStatusRequest = statusRepository.Get((long)RawStatusRequestEnum.DateEnd);
                dateEndEvent = new RequestEvent()
                {
                    RequestId = request.Id,
                    DateInsert = currentDate,
                    DateEvent = request.DateEndPlan.AddDays(1),
                    OrdGroup = lastEvent.OrdGroup + 1,
                    User = user,
                    StatusRequest = dateEndStatusRequest
                };
                request.DateEndPlan = dateEndEvent.DateEvent;
                requestEventRepository.Save(dateEndEvent);
            }
            
            request.User        = user;
            request.DateUpdate  = currentDate;
            request.Status      = statusRequest;
            requestRepository.Save(request);
                       
            

            bool transferRequestToArchive = dto.StatusRequestId == (long)RawStatusRequestEnum.ApprovedComplete ||
                dto.StatusRequestId == (long)RawStatusRequestEnum.ApprovedRejected ||
                dto.StatusRequestId == (long)RawStatusRequestEnum.Passive;
            //перенос заявки в архив
            if (transferRequestToArchive)
            {
                var requestArch = new RequestArch
                {
                    CountCorrectionDateEndPlan = request.CountCorrectionDateEndPlan,
                    DateEndFact = currentDate,
                    DateEndPlan = request.DateEndPlan,
                    DateInsert = request.DateInsert,
                    DateUpdate = request.DateUpdate,
                    DescriptionProblem = request.DescriptionProblem,
                    Employee = request.Employee,
                    Object = request.Object,
                    Status = request.Status,
                    Worker = request.Worker,
                    Version = request.Version,
                    User = request.User,
                    Id = request.Id
                };
                requestArchRepository.Insert(requestArch, requestArch.Id);
                var requestEvents = requestEventRepository.GetList(r => r.RequestId == request.Id).ToList();
                foreach (var evnt in requestEvents)
                {
                    var evntArch = new RequestEventArch
                    {
                        DateEvent = evnt.DateEvent,
                        DateInsert = evnt.DateInsert,
                        Note = evnt.Note,
                        OrdGroup = evnt.OrdGroup,
                        RequestId = evnt.RequestId,
                        StatusRequest = evnt.StatusRequest,
                        User = evnt.User,
                        Id = evnt.Id
                    };
                    requestEventArchRepository.Insert(evntArch, evntArch.Id);
                }
                requestEventRepository.Delete(e => e.RequestId == request.Id);
                requestRepository.Delete(request.Id);
            }
            

            repository.SaveChanges();

            queue.Push(new RequestAppEvent()
            {
                RequestEventId = newEvent.Id,
                Archive = transferRequestToArchive
            });
        }
        
        /// <summary>
        /// Допустимый перенос срока заявки
        /// </summary>
        public Interval<DateTime, DateTime?> GetAllowableDeadLine(long requestId)
        {
            Request r = requestRepository.Get(requestId);
            Interval<DateTime, DateTime?> interval = new Interval<DateTime, DateTime?>();

            Settings settings = settingsService.Get();
            int minCountHour = 0;
            int maxCountHour = 0;
            if (settings.StartWorkDay.HasValue && settings.EndWorkDay.HasValue && settings.StartWorkDay < settings.EndWorkDay)
            {
                int countHourWorkDay = settings.EndWorkDay.Value - settings.StartWorkDay.Value;
                if (settings.StartLunchBreak.HasValue && settings.EndLunchBreak.HasValue && settings.StartLunchBreak < settings.EndLunchBreak)
                    countHourWorkDay -= (settings.EndLunchBreak.Value - settings.StartLunchBreak.Value);

                if (settings.MinCountTransferDay.HasValue)
                    minCountHour = countHourWorkDay * settings.MinCountTransferDay.Value;
                if (settings.MaxCountTransferDay.HasValue)
                    maxCountHour = countHourWorkDay * settings.MaxCountTransferDay.Value;
            }

            DateTime startingPointDateTime = dateTimeService.GetCurrent().Date.AddHours(r.DateEndPlan.Hour);

            if (minCountHour > 0)
                interval.Value1 = dateTimeService.GetRequestDateEnd(startingPointDateTime, minCountHour);
            else
                interval.Value1 = dateTimeService.GetCurrent();

            if (maxCountHour > 0)
                interval.Value2 = dateTimeService.GetRequestDateEnd(startingPointDateTime, maxCountHour);

            return interval;
        }

        /// <summary>
        /// Список проблем, зафиксированных для некоторого объекта (для ПО)/типа ТО (для ТО)
        /// </summary>
        public IEnumerable<SimpleDTO> GetListDescriptionProblem(string name, long objectId)
        {
            return queryHandler.Handle<DescriptionProblemQueryParam, IEnumerable<SimpleDTO> , IQuery<DescriptionProblemQueryParam, IEnumerable<SimpleDTO>>>(
				new DescriptionProblemQueryParam
				{
					Name = name,
					ObjectId = objectId
				}, _descriptionProblemQuery);
        }

        /// <summary>
        /// Статистика активных заявок по состояниям
        /// </summary>
        public IEnumerable<RequestStateCountDTO> GetListRequestStateCount(long userId)
        {
            Expression<Func<BaseRequest, bool>> accessPredicate = accessWorkerUserExpressionService
                        .GetAccessRequestPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));

            return queryHandler.Handle<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>, IQuery<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>>>(
				new RequestStateCountQueryParam
				{
					 AccessPredicate = accessPredicate
				}, _requestStateCountQuery);
        }
    }
}
