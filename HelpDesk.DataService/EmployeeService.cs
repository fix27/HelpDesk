using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System;
using System.Linq;
using HelpDesk.DTO;
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
    /// Для работы с личными данными пользователя
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
        private readonly IRepository repository;

        public EmployeeService(
            IQueryRunner queryRunner,
            IBaseRepository<CabinetUser> userRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<EmployeeObject> employeeObjectRepository,
            IBaseRepository<Post> postRepository,
            IBaseRepository<Organization> organizationRepository,
            IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository,
            IRepository repository)
        {
            this.queryRunner = queryRunner;
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
            this.employeeObjectRepository = employeeObjectRepository;
            this.postRepository     = postRepository;
            this.organizationRepository = organizationRepository;
            this.organizationObjectTypeWorkerRepository = organizationObjectTypeWorkerRepository;
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
                PostName = entity.Post!=null? entity.Post.Name: null,
                Subscribe = entity.Subscribe               
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
                .GetList(e => e.FM.ToUpper().Contains(name) || 
                    e.Phone == name || 
                    e.Organization.Name.ToUpper().Contains(name))
                .Select(e => new EmployeeDTO()
                {
                    Id = e.Id,
                    FM = e.FM,
                    IM = e.IM,
                    OT = e.OT,
                    Cabinet = e.Cabinet,
                    Phone = e.Phone,
                    PostName = e.Post != null ? e.Post.Name : null,
                    Subscribe = e.Subscribe,
                    OrganizationId = e.Organization.Id,
                    OrganizationName = e.Organization.Name,
                    OrganizationAddress = e.Organization.Address
                })
                .ToList();
        }

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

            if (entity == null)
                entity = new Employee()
                {
                    User = userRepository.Get(dto.Id)
                };
            else
            {
                if (entity.Organization!=null && dto.OrganizationId.HasValue && 
                    entity.Organization.Id != dto.OrganizationId)
                {
                    //удаляем из профиля заявителя объекты, на которые он не сможет подавать заявки,
                    //так как для них не определится исполнитель
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
            entity.Subscribe = dto.Subscribe;

            if (dto.OrganizationId.HasValue)
                entity.Organization = organizationRepository.Get(dto.OrganizationId.Value);
            else
                entity.Organization = null;

            if (!String.IsNullOrWhiteSpace(dto.PostName))
            {
                Post p = postRepository.Get(new SimpleEntityByNameEqualSpecification<Post>(dto.PostName));
                if (p == null)
                {
                    p = new Post() { Name = dto.PostName.Trim().ToFirstLetterUpper() };
                    postRepository.Save(p);
                }
                entity.Post = p;
            }
            else
                entity.Post = null;


            employeeRepository.Save(entity);
           
            repository.SaveChanges();
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
