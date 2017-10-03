using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.DataService.DTO;
using HelpDesk.DataService.Specification;
using System;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с объектами заявки
    /// </summary>
    public class ObjectService : BaseService, IObjectService
    {
        private readonly IBaseRepository<HardType> hardTypeRepository;
        private readonly IBaseRepository<Model> modelRepository;
        private readonly IBaseRepository<Manufacturer> manufacturerRepository;
        private readonly IBaseRepository<RequestObject> objectRepository;

        public ObjectService(IBaseRepository<HardType> hardTypeRepository,
            IBaseRepository<Model> modelRepository,
            IBaseRepository<Manufacturer> manufacturerRepository,
            IBaseRepository<RequestObject> objectRepository)
        {
            this.hardTypeRepository = hardTypeRepository;
            this.modelRepository = modelRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.objectRepository = objectRepository;
        }

        public RequestObjectDTO Get(long id)
        {
            RequestObject obj = objectRepository.Get(id);
            if (obj == null)
                return null;
            return new RequestObjectDTO()
            {
                 HardType = obj.HardType,
                 Model = obj.Model,
                 ObjectType = obj.ObjectType,
                 Soft = !String.IsNullOrWhiteSpace(obj.SoftName),
                 SoftName = obj.SoftName,
                 Id = obj.Id
            };
        }

        public IEnumerable<WareDTO> GetListWare()
        {
            return new List<WareDTO>()
            {
                new WareDTO() { Id = false, Name= WareDTO.GetName(false) },
                new WareDTO() { Id = true, Name= WareDTO.GetName(true) }
            };
        }

        public IEnumerable<SimpleDTO> GetListHardType(string name = null)
        {
            return hardTypeRepository.GetList(new SimpleEntityAllOrByNameLikeSpecification<HardType>(name))
                .OrderBy(t => t.Name)
                .Select(t => new SimpleDTO() { Id = t.Id, Name = t.Name }).ToList();
        }

        public IEnumerable<SimpleDTO> GetListModel(long manufacturerId, string name = null)
        {
            return modelRepository.GetList(new ModelByManufacturerAndNameLikeSpecification(manufacturerId, name))
                .OrderBy(t => t.Name)
                .Select(t => new SimpleDTO() { Id = t.Id, Name = t.Name }).ToList();
        }
        
        public IEnumerable<SimpleDTO> GetListManufacturer(string name = null)
        {
            return manufacturerRepository.GetList(new SimpleEntityAllOrByNameLikeSpecification<Manufacturer>(name))
                .OrderBy(t => t.Name)
                .Select(t => new SimpleDTO() { Id = t.Id, Name = t.Name }).ToList();
        }

    }
}
