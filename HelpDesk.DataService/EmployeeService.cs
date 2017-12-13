using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System;
using System.Linq;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using HelpDesk.Common.Aspects;
using HelpDesk.Common.Helpers;
using System.Text.RegularExpressions;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Specification;
using System.Collections.Generic;
using HelpDesk.DataService.Query;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с сотрудниками
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IQueryRunner queryRunner;
        private readonly IBaseRepository<CabinetUser> userRepository;
        private readonly IBaseRepository<Employee> employeeRepository;
        private readonly IBaseRepository<EmployeeObject> employeeObjectRepository;
        private readonly IBaseRepository<Post> postRepository;
        private readonly IBaseRepository<Organization> organizationRepository;
        private readonly IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository;
        private readonly IBaseRepository<WorkerUser> workerUserRepository;
        private readonly IRepository repository;

        public EmployeeService(
            IQueryRunner queryRunner,
            IBaseRepository<CabinetUser> userRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<EmployeeObject> employeeObjectRepository,
            IBaseRepository<Post> postRepository,
            IBaseRepository<Organization> organizationRepository,
            IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository,
            IBaseRepository<WorkerUser> workerUserRepository,
            IRepository repository)
        {
            this.queryRunner        = queryRunner;
            this.userRepository     = userRepository;
            this.employeeRepository = employeeRepository;
            this.employeeObjectRepository = employeeObjectRepository;
            this.postRepository     = postRepository;
            this.organizationRepository = organizationRepository;
            this.organizationObjectTypeWorkerRepository = organizationObjectTypeWorkerRepository;
            this.workerUserRepository   = workerUserRepository;
            this.repository         = repository;
        }
        
        public EmployeeDTO Get(long id)
        {
            Employee entity = employeeRepository.Get(id);
            
            if (entity == null)
                return new EmployeeDTO();

            EmployeeDTO dto = new EmployeeDTO()
            {
                Id = entity.Id,
                FM = entity.FM,
                IM = entity.IM,
                OT = entity.OT,
                Cabinet = entity.Cabinet,
                Phone = entity.Phone,
                PostName = entity.Post!=null? entity.Post.Name: null
            };

            if (entity.Organization != null)
            {
                dto.OrganizationId      = entity.Organization.Id;
                dto.OrganizationName    = entity.Organization.Name;
                dto.OrganizationAddress = entity.Organization.Address;
            }

            return dto;
        }

        public IEnumerable<EmployeeDTO> GetList(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            name = name.ToUpper();

            return employeeRepository
                .GetList(e => e.Organization!=null && 
                    (e.FM.ToUpper().Contains(name) || 
                     e.Phone == name || 
                     e.Organization.Name.ToUpper().Contains(name)))
                .Select(e => new EmployeeDTO()
                {
                    Id = e.Id,
                    FM = e.FM,
                    IM = e.IM,
                    OT = e.OT,
                    Cabinet = e.Cabinet,
                    Phone = e.Phone,
                    PostName = e.Post != null ? e.Post.Name : null,
                    OrganizationId = e.Organization.Id,
                    OrganizationName = e.Organization.Name,
                    OrganizationAddress = e.Organization.Address
                })
                .OrderBy(t => t.FM)
                .OrderBy(t => t.IM)
                .OrderBy(t => t.OT)
                .ToList();
        }

        private bool inOrganizationObjectTypeWorker(Organization root, IEnumerable<Organization> items, IEnumerable<long> ids)
        {
            var stack = new Stack<Organization>();
            stack.Push(root);
            while (stack.Any())
            {
                var next = stack.Pop();
                if (ids != null && ids.Contains(next.Id))
                    return true;
                foreach (var child in items.Where(t => t.ParentId == next.Id))
                    stack.Push(child);
            }

            return false;
        }

        public IEnumerable<EmployeeDTO> GetListByWorkerUser(long userId, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                return null;

            name = name.ToUpper();

            IEnumerable<Employee> list = employeeRepository
                .GetList(e => e.Organization != null &&
                    (e.FM.ToUpper().Contains(name) ||
                     e.Phone == name ||
                     e.Organization.Name.ToUpper().Contains(name))).ToList();

            if (string.IsNullOrWhiteSpace(name))
                return null;

            WorkerUser user = workerUserRepository.Get(userId);
            long workerId = 0;
            if (user.Worker == null)
                return list
                    .OrderBy(t => t.FM)
                    .OrderBy(t => t.IM)
                    .OrderBy(t => t.OT)
                    .Select(e => new EmployeeDTO()
                    {
                        Id = e.Id,
                        FM = e.FM,
                        IM = e.IM,
                        OT = e.OT,
                        Cabinet = e.Cabinet,
                        Phone = e.Phone,
                        PostName = e.Post != null ? e.Post.Name : null,
                        OrganizationId = e.Organization.Id,
                        OrganizationName = e.Organization.Name,
                        OrganizationAddress = e.Organization.Address
                    });
                    

            workerId = user.Worker.Id;

            IEnumerable<long> ids = organizationObjectTypeWorkerRepository
                .GetList(t => workerId == 0 || t.Worker.Id == workerId)
                .Select(t => t.Organization.Id)
                .Distinct()
                .ToList();

            IEnumerable<Organization> list2 = organizationRepository.GetList()
                .OrderBy(p => p.Name).ToList();

            return list
                .Where(t => inOrganizationObjectTypeWorker(t.Organization, list2, ids))
                .OrderBy(t => t.FM)
                .OrderBy(t => t.IM)
                 .OrderBy(t => t.OT)
                .Select(e => new EmployeeDTO()
                {
                    Id = e.Id,
                    FM = e.FM,
                    IM = e.IM,
                    OT = e.OT,
                    Cabinet = e.Cabinet,
                    Phone = e.Phone,
                    PostName = e.Post != null ? e.Post.Name : null,
                    OrganizationId = e.Organization.Id,
                    OrganizationName = e.Organization.Name,
                    OrganizationAddress = e.Organization.Address
                });
        }
        

        public IEnumerable<EmployeeDTO> GetListByOrganization(long organizationId, long? workerUserId = null)
        {
            if (workerUserId.HasValue)
            {
                WorkerUser workerUser = workerUserRepository.Get(workerUserId.Value);

                if (workerUser.Worker != null)
                {
                    if (!organizationObjectTypeWorkerRepository.GetList().Any(t => t.Organization.Id == organizationId && t.Worker.Id == workerUser.Id))
                        return null;
                }
            }
            
            return employeeRepository
                    .GetList(e => e.Organization.Id == organizationId)
                    .Select(e => new EmployeeDTO()
                    {
                        Id = e.Id,
                        FM = e.FM,
                        IM = e.IM,
                        OT = e.OT,
                        Cabinet = e.Cabinet,
                        Phone = e.Phone,
                        PostName = e.Post != null ? e.Post.Name : null,
                        OrganizationId = e.Organization.Id,
                        OrganizationName = e.Organization.Name,
                        OrganizationAddress = e.Organization.Address
                    })
                    .ToList();

        }

        public IEnumerable<EmployeeDTO> GetListByOrganization(IEnumerable<long> organizationIds, long? workerUserId = null)
        {
            if (workerUserId.HasValue)
            {
                WorkerUser workerUser = workerUserRepository.Get(workerUserId.Value);

                if (workerUser.Worker != null)
                {
                    if (!organizationObjectTypeWorkerRepository.GetList()
                        .Any(t => organizationIds.Contains(t.Organization.Id) && t.Worker.Id == workerUser.Worker.Id))
                        return null;
                }
            }

            return employeeRepository
                    .GetList(e => organizationIds.Contains(e.Organization.Id))
                    .Select(e => new EmployeeDTO()
                    {
                        Id = e.Id,
                        FM = e.FM,
                        IM = e.IM,
                        OT = e.OT,
                        Cabinet = e.Cabinet,
                        Phone = e.Phone,
                        PostName = e.Post != null ? e.Post.Name : null,
                        OrganizationId = e.Organization.Id,
                        OrganizationName = e.Organization.Name,
                        OrganizationAddress = e.Organization.Address
                    })
                    .ToList();

        }


        /// <summary>
        /// Метод вызывается из личного кабинета (когда он вызывается из личного кабинета, то 
        /// существует CabinetUser, и Employee создается с Employee.Id = CabinetUser.Id) и из АРМ Диспетчера 
        /// </summary>
        /// <param name="dto"></param>
        [Transaction]
        public void Save(EmployeeDTO dto)
        {

            checkStringConstraint("FM", dto.FM, true, 100, 2);
            checkStringConstraint("IM", dto.IM, true, 100, 2);
            checkStringConstraint("OT", dto.OT, true, 100, 2);

            string pattern = "[a-zA-Z]";
            var results = Regex.Matches(dto.FM, pattern);
            foreach (Match result in results)
                setErrorMsg("FM", String.Format(Resource.NotCyrillicConstraintMsg, result.Value.ToUpper(), result.Index + 1));

            results = Regex.Matches(dto.IM, pattern);
            foreach (Match result in results)
                setErrorMsg("IM", String.Format(Resource.NotCyrillicConstraintMsg, result.Value.ToUpper(), result.Index + 1));

            results = Regex.Matches(dto.OT, pattern);
            foreach (Match result in results)
                setErrorMsg("OT", String.Format(Resource.NotCyrillicConstraintMsg, result.Value.ToUpper(), result.Index + 1));

            checkStringConstraint("Cabinet", dto.Cabinet, true, 100, 1);
            checkStringConstraint("Phone", dto.Phone, true, 100, 5);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            Employee entity = employeeRepository.Get(dto.Id);
            CabinetUser user = null;
            if(dto.Id > 0)
                user = userRepository.Get(dto.Id);

            Post post = null;
            if (!String.IsNullOrWhiteSpace(dto.PostName))
                post = postRepository.Get(new SimpleEntityByNameEqualSpecification<Post>(dto.PostName));

            bool isNew = entity == null;
            if (isNew)
            {
                //проверяем, не был ли ранее создан сотрудник
                entity = employeeRepository.Get(
                    e => e.FM.ToUpper() == dto.FM.ToUpper() &&
                    e.IM.ToUpper()      == dto.IM.ToUpper() &&
                    e.OT.ToUpper()      == dto.OT.ToUpper() &&
                    e.Organization.Id   == dto.OrganizationId &&
                    e.Post.Id           == (post != null? post.Id: 0) &&
                    e.Phone.ToUpper()   == dto.Phone.ToPhoneList());

                if (entity != null)                 //сотрудник был создан ранее
                {
                    dto.Id = entity.Id;
                    if (user != null)               //1. ф-я вызывается из личного кабинета
                    {
                        if (entity.Id == user.Id)   //1.1. сотрудник был создан ранее из личного кабинета
                        {
                            return;
                        }
                        else                        //1.2. сотрудник был создан ранее из АРМ Диспетчера
                        {
                            //Требуется вручную в БД изменить Id у user, за счет чего произойдёт привязка 1-1 к employee
                            //Вообще говоря, это может быть каким-то злономеренным действием, 
                            //когда пользователь пытается привязаться к существующему другому сотруднику
                            throw new DataServiceException(Resource.UniqueEmployeeConstraintMsg);
                        }
                    }
                    else                            //2. ф-я вызывается из АРМ Диспетчера
                    {
                        if (entity.User != null)    //2.1. сотрудник был создан ранее из личного кабинета
                            return;
                        else                        //2.2. сотрудник был создан ранее из АРМ Диспетчера
                            return;
                    }
                }
                else
                    entity = new Employee();
            }
            else
            {
                if (entity.Organization != null && dto.OrganizationId.HasValue &&
                    entity.Organization.Id != dto.OrganizationId)
                {
                    //удаляем из профиля заявителя объекты, на которые он не сможет подавать заявки,
                    //так как для них не определится Исполнитель
                    IEnumerable<EmployeeObjectDTO> listPersonalProfileObject = queryRunner.Run(new EmployeeObjectQuery(
                        entity.User.Id));

                    IEnumerable<OrganizationObjectTypeWorker> listOrganizationObjectTypeWorker =
                        organizationObjectTypeWorkerRepository.GetList(t => t.Organization.Id == dto.OrganizationId).ToList();

                    IEnumerable<EmployeeObjectDTO> toRemoval = listPersonalProfileObject
                        .Where(t => !listOrganizationObjectTypeWorker
                            .Select(r => r.ObjectType.Id)
                            .Contains(t.ObjectType.Id));

                    foreach (var p in toRemoval)
                        employeeObjectRepository.Delete(p.Id);
                }
            };

            entity.FM = dto.FM.Trim().ToFirstLetterUpper();
            entity.IM = dto.IM.Trim().ToFirstLetterUpper();
            entity.OT = dto.OT.Trim().ToFirstLetterUpper();
            entity.Cabinet = dto.Cabinet.Trim().ToUpper();
            entity.Phone = dto.Phone.ToPhoneList();

            if (dto.OrganizationId.HasValue)
                entity.Organization = organizationRepository.Get(dto.OrganizationId.Value);
            else
                entity.Organization = null;

            if (!String.IsNullOrWhiteSpace(dto.PostName))
            {
                if (post == null)
                {
                    post = new Post() { Name = dto.PostName.Trim().ToFirstLetterUpper() };
                    postRepository.Save(post);
                }
                entity.Post = post;
            }
            else
                entity.Post = null;

            if (isNew && user != null)
                employeeRepository.Insert(entity, user.Id);
            else
                employeeRepository.Save(entity);

            repository.SaveChanges();

            dto.Id = entity.Id;
        }
        
        public bool IsComplete(long id)
        {
            Employee entity = employeeRepository.Get(id);

            return entity!=null && !(String.IsNullOrWhiteSpace(entity.Cabinet) ||
                String.IsNullOrWhiteSpace(entity.Phone) ||
                String.IsNullOrWhiteSpace(entity.FM) ||
                String.IsNullOrWhiteSpace(entity.IM) ||
                String.IsNullOrWhiteSpace(entity.OT) ||
                entity.Post == null ||
                entity.Organization == null);
        }
    }
}
