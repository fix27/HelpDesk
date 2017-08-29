using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System;
using System.Linq;
using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using HelpDesk.Data.Command;
using HelpDesk.DataService.Command;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Query;
using HelpDesk.DTO.FileUpload;
using HelpDesk.Common.Helpers;
using HelpDesk.Common.Aspects;
using System.Linq.Expressions;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с заявками
    /// </summary>
    [Transaction]
    public class RequestService : BaseService, IRequestService
    {
        private readonly ICommandRunner commandRunner;
        private readonly IQueryRunner queryRunner;
        private readonly IBaseRepository<RequestObject> objectRepository;
        private readonly ISettingsRepository settingsRepository;
        private readonly IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository;
        private readonly IBaseRepository<Employee> employeeRepository;
        private readonly IBaseRepository<StatusRequest> statusRepository;
        private readonly IBaseRepository<Request> requestRepository;
        private readonly IBaseRepository<RequestArch> requestArchRepository;
        private readonly IBaseRepository<RequestEvent> requestEventRepository;
        private readonly IBaseRepository<RequestEventArch> requestEventArchRepository;
        private readonly IBaseRepository<RequestFile> requestFileRepository;
        private readonly IBaseRepository<Worker> workerRepository;
        private readonly IRepository repository;
        private readonly IDateTimeService dateTimeService;
        private readonly IRequestConstraintsService requestConstraintsService;
        private readonly IStatusRequestMapService statusRequestMapService;
        private readonly IConstantStatusRequestService constantService;
        private readonly IAccessWorkerUserService accessWorkerUserService;

        public RequestService(ICommandRunner commandRunner,
            IQueryRunner queryRunner,
            IBaseRepository<RequestObject> objectRepository,
            ISettingsRepository settingsRepository,
            IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<StatusRequest> statusRepository,
            IBaseRepository<Request> requestRepository,
            IBaseRepository<RequestArch> requestArchRepository,
            IBaseRepository<RequestEvent> requestEventRepository,
            IBaseRepository<RequestEventArch> requestEventArchRepository,
            IBaseRepository<RequestFile> requestFileRepository,
            IBaseRepository<Worker> workerRepository,
            IRepository repository,
            IDateTimeService dateTimeService,
            IRequestConstraintsService requestConstraintsService,
            IStatusRequestMapService statusRequestMapService,
            IConstantStatusRequestService constantService,
            IAccessWorkerUserService accessWorkerUserService)
        {
            this.queryRunner            = queryRunner;
            this.objectRepository       = objectRepository;
            this.commandRunner          = commandRunner;
            this.settingsRepository     = settingsRepository;
            this.organizationObjectTypeWorkerRepository = organizationObjectTypeWorkerRepository;
            this.employeeRepository = employeeRepository;
            this.statusRepository       = statusRepository;
            this.requestRepository      = requestRepository;
            this.requestArchRepository  = requestArchRepository;
            this.requestEventRepository = requestEventRepository;
            this.requestEventArchRepository = requestEventArchRepository;
            this.requestFileRepository  = requestFileRepository;
            this.workerRepository       = workerRepository;
            this.repository             = repository;
            this.dateTimeService        = dateTimeService;
            this.requestConstraintsService = requestConstraintsService;
            this.statusRequestMapService = statusRequestMapService;
            this.constantService        = constantService;
            this.accessWorkerUserService = accessWorkerUserService;
        }

        

        

        private CreateOrUpdateRequestDTO getCreateOrUpdateRequest(long id)
        {
            BaseRequest request = requestRepository.Get(id);
            if (request == null)
                request = requestArchRepository.Get(id);

            if (request == null)
                throw new DataServiceException(Resource.NoDataFoundMsg);

            return new CreateOrUpdateRequestDTO()
            {
                Id = request.Id,
                ObjectId = request.Object.Id,
                ObjectName = RequestObjectDTO.GetObjectName(request.Object.ObjectType.Name,
                    request.Object.SoftName,
                    request.Object.HardType,
                    request.Object.Model),
                TempRequestKey = Guid.NewGuid(),
                DescriptionProblem = request.DescriptionProblem
            };
        }


        public CreateOrUpdateRequestDTO Get(long id = 0)
        {
            if(id == 0)
                return new CreateOrUpdateRequestDTO()
                {
                    TempRequestKey = Guid.NewGuid()
                };

            return getCreateOrUpdateRequest(id);
        }
        public CreateOrUpdateRequestDTO GetNewByRequestId(long requestId)
        {
            CreateOrUpdateRequestDTO r = getCreateOrUpdateRequest(requestId);
            r.Id = 0;
            r.DescriptionProblem = String.Format(Resource.NewRequestByRequestTemplate, requestId, r.DescriptionProblem);
            return r;

        }
        public CreateOrUpdateRequestDTO GetNewByObjectId(long objectId)
        {
            RequestObject edsObject = objectRepository.Get(objectId);

            return new CreateOrUpdateRequestDTO()
            {
                ObjectId = objectId,
                ObjectName = RequestObjectDTO.GetObjectName(edsObject.ObjectType.Name,
                    edsObject.SoftName,
                    edsObject.HardType,
                    edsObject.Model),
                TempRequestKey = Guid.NewGuid()
            };
        }

        
        public void Delete(long id)
        {
            requestConstraintsService.CheckExistsRequest(id);

            commandRunner.Run(new DeleteRequestFileCommand(id));
            requestRepository.Delete(id);
            repository.SaveChanges();
        }

        public IEnumerable<RequestDTO> GetListByEmployee(long employeeId, RequestFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
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

                IEnumerable<RequestDTO> listActive = queryRunner.Run(new EmployeeRequestQuery<Request>(employeeId, filter, orderInfo, pageInfoActive));
                IEnumerable<RequestDTO> listArchive = queryRunner.Run(new EmployeeRequestQuery<RequestArch>(employeeId, filter, orderInfo, pageInfoArchive));

                pageInfo.Count = pageInfoActive.Count + pageInfoArchive.Count;
                pageInfo.TotalCount = pageInfoActive.TotalCount + pageInfoArchive.TotalCount;

                list = listActive.Union(listArchive);
                
            }
            else if (filter.Archive)
            {
                list = queryRunner.Run(new EmployeeRequestQuery<RequestArch>(employeeId, filter, orderInfo, pageInfo));
                foreach (RequestDTO r in list)
                    r.Archive = true;
            }
            else
            {
                list = queryRunner.Run(new EmployeeRequestQuery<Request>(employeeId, filter, orderInfo, pageInfo));
            }

            IEnumerable<long> requestIds = list.Select(r => r.Id).ToList();
            #region files 
            IEnumerable<RequestFileInfoDTO> files = requestFileRepository.GetList(t => t.RequestId!= null && requestIds.Contains(t.RequestId.Value))
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
                    fileIndex[r.ForignKeyId.Value] = new List<RequestFileInfoDTO>(){ r };
            }
            #endregion files

            #region LastEvent
            IEnumerable<RequestEventDTO> events = queryRunner.Run(new RequestLastEventQuery(requestIds, constantService));
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

        public IEnumerable<RequestDTO> GetList(long userId, RequestFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
        {
            Expression<Func<BaseRequest, bool>> accessPredicate = accessWorkerUserService.GetAccessExpression(userId);

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

                IEnumerable<RequestDTO> listActive = queryRunner.Run(new RequestQuery<Request>(accessPredicate, filter, orderInfo, ref pageInfoActive));
                IEnumerable<RequestDTO> listArchive = queryRunner.Run(new RequestQuery<RequestArch>(accessPredicate, filter, orderInfo, ref pageInfoArchive));

                pageInfo.Count = pageInfoActive.Count + pageInfoArchive.Count;
                pageInfo.TotalCount = pageInfoActive.TotalCount + pageInfoArchive.TotalCount;

                list = listActive.Union(listArchive);

            }
            else if (filter.Archive)
            {
                list = queryRunner.Run(new RequestQuery<RequestArch>(accessPredicate, filter, orderInfo, ref pageInfo));
                foreach (RequestDTO r in list)
                    r.Archive = true;
            }
            else
            {
                list = queryRunner.Run(new RequestQuery<Request>(accessPredicate, filter, orderInfo, ref pageInfo));
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
            IEnumerable<RequestEventDTO> events = queryRunner.Run(new RequestLastEventQuery(requestIds, constantService));
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

        public IEnumerable<StatusRequestDTO> GetListStatus(bool archive)
        {
            IEnumerable<StatusRequestDTO> list = statusRepository.GetList(t => !constantService.IgnoredRawRequestStates.Contains(t.Id)).OrderBy(s => s.Name)
                .ToList()
                .Select(s => new StatusRequestDTO()
                {
                    Id = statusRequestMapService.GetEquivalenceByElement(s.Id),
                    Name = statusRequestMapService.GetEquivalenceByElement(s.Id).GetDisplayName()
                });

            IList<StatusRequestEnum> archiveStates = new List<StatusRequestEnum>()
                {
                    StatusRequestEnum.ApprovedComplete,
                    StatusRequestEnum.Passive
                };

            return list.Where(t => (archive) ? archiveStates.Contains(t.Id) : !archiveStates.Contains(t.Id)).Distinct();

        }

        [Transaction]
        public long Save(CreateOrUpdateRequestDTO dto)
        {
            if (dto.Id == 0)
            {
                DateTime currentDate = dateTimeService.GetCurrent();
                Settings settings = settingsRepository.Get();
                IList<Request> list = requestRepository.GetList(t => t.DateInsert.Date == currentDate.Date && 
                t.Employee.Id == dto.EmployeeId && t.UserId == null)
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

            
            
            Employee personalProfile = employeeRepository.Get(dto.EmployeeId);
            RequestObject requestObject = objectRepository.Get(dto.ObjectId);


            OrganizationObjectTypeWorker organizationObjectTypeWorker 
                = organizationObjectTypeWorkerRepository.Get(t => t.Organization.Id == personalProfile.Organization.Id && t.ObjectType.Id == requestObject.ObjectType.Id);

            if (organizationObjectTypeWorker == null)
                setErrorMsg("Worker", Resource.WorkerNotDefinedConstraintMsg);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            Request r = null;
            DateTime currentDateTime = dateTimeService.GetCurrent();
            if (dto.Id > 0)
            {
                requestConstraintsService.CheckExistsRequest(dto.Id);
                                
                r = requestRepository.Get(dto.Id);

                r.DateUpdate = currentDateTime;
                r.DescriptionProblem = dto.DescriptionProblem;
                requestRepository.Save(r);
                repository.SaveChanges();
                commandRunner.Run(new UpdateRequestFileCommand(dto.TempRequestKey, dto.Id));
                return dto.Id;
            }
           
            
            StatusRequest newStatusRequest = statusRepository.Get(constantService.NewStatusRequest);
            r = new Request()
            {
                CountCorrectionDateEndPlan = 0,
                DateEndPlan = currentDateTime.AddDays(3),
                DateInsert = currentDateTime,
                DateUpdate = currentDateTime,
                DescriptionProblem = dto.DescriptionProblem,
                Worker = workerRepository.Get(organizationObjectTypeWorker.Worker.Id),
                Object = objectRepository.Get(dto.ObjectId),
                Employee = employeeRepository.Get(dto.EmployeeId),
                Status = newStatusRequest
            };
            requestRepository.Save(r);

            RequestEvent newRequestEvent = new RequestEvent()
            {
                 StatusRequest = newStatusRequest,
                 DateEvent = currentDateTime,
                 DateInsert = currentDateTime,
                 RequestId = r.Id                     
            };
            requestEventRepository.Save(newRequestEvent);

            RequestEvent dateEndRequestEvent = new RequestEvent()
            {
                StatusRequest = statusRepository.Get(constantService.DateEndStatusRequest),
                DateEvent = r.DateEndPlan.Value,
                DateInsert = currentDateTime,
                RequestId = r.Id
            };
            requestEventRepository.Save(dateEndRequestEvent);

            repository.SaveChanges();
                        
            commandRunner.Run(new UpdateRequestFileCommand(dto.TempRequestKey, r.Id));
            return r.Id;
        }

        public int GetCountRequiresConfirmation(long employeeId)
        {
            return requestRepository.Count(t => t.Employee.Id == employeeId && t.Status.Id == constantService.ConfirmationStatusRequest);
        }

        public IEnumerable<Year> GetListArchiveYear(long employeeId)
        {
            IEnumerable<Year> list = queryRunner.Run(new ArchiveYearQuery(employeeId));
            return list;
        }

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
                     Note       = t.Name,
                     DateEnd    = t.StatusRequest.Id == constantService.DateEndStatusRequest,
                     Status     = t.StatusRequest,
                     RequestId  = t.RequestId,
                     Transfer   = t.TypeRequestEventId.HasValue,
                     StatusRequest = statusRequestMapService.GetEquivalenceByElement(t.StatusRequest.Id)              
                });

            return null;
        }
    }
}
