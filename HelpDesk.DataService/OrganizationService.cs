using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.DataService.Specification;
using HelpDesk.DataService.DTO;
using HelpDesk.Common.Aspects;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для запроса данных из справочника организаций
    /// </summary>
    [Cache]
    public class OrganizationService : BaseService, IOrganizationService
    {
        
        private readonly IBaseRepository<Organization> organizationRepository;
        private readonly IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository;
        private readonly IBaseRepository<WorkerUser> workerUserRepository;
        public OrganizationService(IBaseRepository<Organization> organizationRepository,
            IBaseRepository<OrganizationObjectTypeWorker> organizationObjectTypeWorkerRepository,
            IBaseRepository<WorkerUser> workerUserRepository)
        {
            this.organizationRepository = organizationRepository;
            this.organizationObjectTypeWorkerRepository = organizationObjectTypeWorkerRepository;
            this.workerUserRepository = workerUserRepository;
        }
        
        public IEnumerable<Organization> GetList(string name = null)
        {
            if(string.IsNullOrWhiteSpace(name))
                return organizationRepository.GetList().OrderBy(p => p.Name).ToList();

            return organizationRepository.GetList(new SimpleEntityByNameLikeSpecification<Organization>(name))
                .OrderBy(p => p.Name).ToList();
        }

        [Cache(CacheKeyTemplate = "IEnumerable<Organization>(parentId={0})", AbsoluteExpiration = 100)]
        public IEnumerable<Organization> GetList(long? parentId)
        {
            return organizationRepository.GetList(o => o.ParentId == parentId)
                .OrderBy(p => p.Name).ToList();
        }

        
        //private IEnumerable<T> Traverse<T>(IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        //{
        //    var stack = new Stack<T>(items);
        //    while (stack.Any())
        //    {
        //        var next = stack.Pop();
        //        yield return next;
        //        foreach (var child in childSelector(next))
        //            stack.Push(child);
        //    }
        //}

        private bool inOrganizationObjectTypeWorker(OrganizationDTO root, IEnumerable<OrganizationDTO> items, IEnumerable<long> ids)
        {
            var stack = new Stack<OrganizationDTO>();
            stack.Push(root);
            while (stack.Any())
            {
                var next = stack.Pop();
                
                if (ids != null && ids.Contains(next.Id))
                {
                    next.Selectable = true;
                    return true;
                }
                foreach (var child in items.Where(t => t.ParentId == next.Id))
                    stack.Push(child);
            }

            return false;
        }

        public IEnumerable<OrganizationDTO> GetListByWorkerUser(long userId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            WorkerUser user = workerUserRepository.Get(userId);
            long workerId = 0;
            if (user.Worker != null)
                workerId = user.Worker.Id;

            IEnumerable<long> ids = organizationObjectTypeWorkerRepository
                .GetList(t => workerId == 0 || t.Worker.Id == workerId)
                .Select(t => t.Organization.Id)
                .Distinct()
                .ToList();

            IEnumerable<OrganizationDTO> list = organizationRepository.GetList()
                .Select(t => new OrganizationDTO()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    ParentId = t.ParentId,
                    HasChild = t.HasChild
                })
                .OrderBy(p => p.Name).ToList();

            IEnumerable<OrganizationDTO> list2 = list;

            list = list.Where(t => t.Name.ToUpper().Contains(name.ToUpper()) && inOrganizationObjectTypeWorker(t, list2, ids));

            return list;
        }

        [Cache(CacheKeyTemplate = "IEnumerable<OrganizationDTO>(userId={0}, parentId={1})", AbsoluteExpiration = 100)]
        public IEnumerable<OrganizationDTO> GetListByWorkerUser(long userId, long? parentId)
        {
            WorkerUser user = workerUserRepository.Get(userId);
            
            long workerId = 0;
            if (user.Worker != null)
                workerId = user.Worker.Id;


            IEnumerable<long> ids =  organizationObjectTypeWorkerRepository
                .GetList(t => workerId == 0 || t.Worker.Id == workerId)
                .Select(t => t.Organization.Id)
                .Distinct()
                .ToList();

            IEnumerable<OrganizationDTO> list = organizationRepository.GetList()
                .Select(t => new OrganizationDTO()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    ParentId = t.ParentId,
                    HasChild = t.HasChild
                })
                .OrderBy(p => p.Name).ToList();

            IEnumerable<OrganizationDTO> list2 = list;

            list = list.Where(t => t.ParentId == parentId && inOrganizationObjectTypeWorker(t, list2, ids));

            return list;
        }

        [Cache(CacheKeyTemplate = "IEnumerable<OrganizationDTO>(userId={0})", AbsoluteExpiration = 100)]
        public IEnumerable<OrganizationDTO> GetListByWorkerUser(long userId)
        {
            WorkerUser user = workerUserRepository.Get(userId);

            long workerId = 0;
            if (user.Worker != null)
                workerId = user.Worker.Id;


            IEnumerable<long> ids = organizationObjectTypeWorkerRepository
                .GetList(t => workerId == 0 || t.Worker.Id == workerId)
                .Select(t => t.Organization.Id)
                .Distinct()
                .ToList();

            IEnumerable<OrganizationDTO> list = organizationRepository.GetList()
                .Select(t => new OrganizationDTO()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Address = t.Address,
                    ParentId = t.ParentId,
                    HasChild = t.HasChild
                })
                .OrderBy(p => p.Name).ToList();

            IEnumerable<OrganizationDTO> list2 = list;

            list = list.Where(t => inOrganizationObjectTypeWorker(t, list2, ids));

            return list;
        }

        [Cache(CacheKeyTemplate = "GetExistsByWorkerUser(userId={0})", AbsoluteExpiration = 100)]
        public bool GetExistsByWorkerUser(long userId)
        {
            WorkerUser user = workerUserRepository.Get(userId);
                       
            if (user.Worker == null)
                return true;

            return organizationObjectTypeWorkerRepository
                .GetList().Any(t => t.Worker.Id == user.Worker.Id);

        }
    }
}
