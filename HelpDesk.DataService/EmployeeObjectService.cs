using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using HelpDesk.DTO;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;
using HelpDesk.Common.Aspects;
using System.Collections.Generic;
using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Query;
using HelpDesk.DataService.Specification;
using System.Linq;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с профилем заявителя - сотрудника обслуживаемой организации
    /// </summary>
    [Transaction]
    public class EmployeeObjectService : BaseService, IEmployeeObjectService
    {

        private readonly IQueryRunner queryRunner;
        private readonly IBaseRepository<EmployeeObject> employeeObjectRepository;
        private readonly IBaseRepository<Employee> employeeRepository;
        private readonly IBaseRepository<RequestObject> objectRepository;
        private readonly IBaseRepository<HardType> hardTypeRepository;
        private readonly IBaseRepository<Manufacturer> manufacturerRepository;
        private readonly IBaseRepository<Model> modelRepository;
        private readonly IBaseRepository<ObjectType> objectTypeRepository;
        
        private readonly IRepository repository;

        public EmployeeObjectService(
            IQueryRunner queryRunner,
            IBaseRepository<EmployeeObject> employeeObjectRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<RequestObject> objectRepository,
            IBaseRepository<HardType> hardTypeRepository,
            IBaseRepository<Manufacturer> manufacturerRepository,
            IBaseRepository<Model> modelRepository,
            IBaseRepository<ObjectType> objectTypeRepository,
            
            IRepository repository)
        {
            this.queryRunner                 = queryRunner;
            this.employeeObjectRepository    = employeeObjectRepository;
            this.employeeRepository          = employeeRepository;
            this.objectRepository            = objectRepository;
            this.hardTypeRepository          = hardTypeRepository;
            this.manufacturerRepository      = manufacturerRepository;
            this.modelRepository             = modelRepository;
            this.objectTypeRepository        = objectTypeRepository;
            
            this.repository                  = repository;
        }
        
        public IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, EmployeeObjectFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
        {
            IEnumerable<EmployeeObjectDTO> list = queryRunner.Run(new EmployeeObjectQuery(employeeId, filter, orderInfo, ref pageInfo));
            return list;
        }

        public IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, string objectName = null)
        {
            IEnumerable<EmployeeObjectDTO> list = queryRunner.Run(new EmployeeObjectQuery(employeeId, new EmployeeObjectFilter() { ObjectName = objectName }));
            return list;
        }

        public bool AllowableForSendRequest(long employeeId)
        {
            IEnumerable<EmployeeObjectDTO> list = queryRunner.Run(new EmployeeObjectQuery(employeeId));
            return list!=null && list.Any();
        }

        public IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long employeeId, string name = null)
        {
            IEnumerable<RequestObjectISDTO> list = queryRunner.Run(new AllowableObjectISQuery(employeeId, name));

            IEnumerable<long> listEmployeeObjectIds = queryRunner.Run(new EmployeeObjectQuery(employeeId))
                .Select(t => t.ObjectId);

            return list.Where(t => !listEmployeeObjectIds.Contains(t.Id));
        }

        public IEnumerable<SimpleDTO> GetListAllowableObjectType(long employeeId)
        {
            IEnumerable<SimpleDTO> list = queryRunner.Run(new AllowableObjectTypeQuery(employeeId));
            return list;
        }

        [Transaction]
        public void AddIS(long employeeId, RequestObjectISDTO dto)
        {
            EmployeeObject entity = employeeObjectRepository
                .Get(t => t.Object.Id == dto.Id && t.Employee.Id == employeeId);

            if(entity != null)
                setErrorMsg("ObjectName", Resource.UniqueEmployeeObjectConstraintMsg);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            entity = new EmployeeObject()
            {
                 Object = objectRepository.Get(dto.Id),
                 Employee = employeeRepository.Get(employeeId)
            };

            employeeObjectRepository.Save(entity);
            repository.SaveChanges();
        }

        [Transaction]
        public void AddTO(long employeeId, RequestObjectTODTO dto)
        {
            EmployeeObject entity = employeeObjectRepository
                .Get(t => t.Object.Id == dto.Id && t.Employee.Id == employeeId);

            if (entity != null)
                setErrorMsg("ObjectName", Resource.UniqueEmployeeObjectConstraintMsg);

            if (dto.ObjectTypeId == 0)
                setErrorMsg("ObjectTypeName", Resource.RequiredConstraintMsg);

            checkStringConstraint("HardTypeName", dto.HardTypeName, true, 200, 2);
            checkStringConstraint("ManufacturerName", dto.ManufacturerName, true, 200, 2);
            checkStringConstraint("ModelName", dto.ModelName, true, 200, 2);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            //----
            HardType hardType = null;
            if (dto.HardTypeId > 0)
            {
                hardType = hardTypeRepository.Get(dto.HardTypeId);
                if (hardType.Name.ToUpper().Trim() != dto.HardTypeName.ToUpper().Trim())
                {
                    hardType = new HardType() { Name = dto.HardTypeName.ToUpper().Trim() };
                    hardTypeRepository.Save(hardType);
                }
            }
            else
            {
                HardType existsHardType = hardTypeRepository.Get(new SimpleEntityByNameLikeSpecification<HardType>(dto.HardTypeName));
                if (existsHardType != null)
                    hardType = existsHardType;
                else
                    hardType = new HardType() { Name = dto.HardTypeName.ToUpper().Trim() };
                hardTypeRepository.Save(hardType);
            }

            //----
            Manufacturer manufacturer = null;
            if (dto.ManufacturerId > 0)
            {
                manufacturer = manufacturerRepository.Get(dto.ManufacturerId);
                if (manufacturer.Name.ToUpper().Trim() != dto.ManufacturerName.ToUpper().Trim())
                {
                    manufacturer = new Manufacturer() { Name = dto.ManufacturerName.ToUpper().Trim() };
                    manufacturerRepository.Save(manufacturer);
                }
            }
            else
            {
                Manufacturer existsManufacturer = manufacturerRepository.Get(new SimpleEntityByNameLikeSpecification<Manufacturer>(dto.ManufacturerName));
                if (existsManufacturer != null)
                    manufacturer = existsManufacturer;
                else
                    manufacturer = new Manufacturer() { Name = dto.ManufacturerName.ToUpper().Trim() };
                manufacturerRepository.Save(manufacturer);
            }

            //----
            Model model = null;
            if (dto.ModelId > 0)
            {
                model = modelRepository.Get(dto.ModelId);
                if (model.Name.ToUpper().Trim() != dto.ModelName.ToUpper().Trim())
                {
                    model = new Model() { Name = dto.ModelName.ToUpper().Trim(), Manufacturer = manufacturer };
                    modelRepository.Save(model);
                }
            }
            else
            {
                Model existsModel = modelRepository.Get(new SimpleEntityByNameLikeSpecification<Model>(dto.ModelName));
                if (existsModel != null)
                    model = existsModel;
                else
                    model = new Model() { Name = dto.ModelName.ToUpper().Trim(), Manufacturer = manufacturer };
                modelRepository.Save(model);
            }


            RequestObject requestObject = new RequestObject()
            {
                HardType = hardType,
                Model = model,
                ObjectType = objectTypeRepository.Get(dto.ObjectTypeId)                
            };
            objectRepository.Save(requestObject);

            entity = new EmployeeObject()
            {
                Object = requestObject,
                Employee = employeeRepository.Get(employeeId)
            };

            employeeObjectRepository.Save(entity);
            repository.SaveChanges();
        }

        [Transaction]
        public void Delete(long employeeId, long id)
        {
            EmployeeObject entity = employeeObjectRepository.Get(id);

            if(entity == null)
                throw new DataServiceException(Resource.AnotherUserDeleteRecordConstraintMsg);

            if (entity.Employee.Id != employeeId)
                throw new DataServiceException(Resource.OnlyOwnerEmployeeObjectCanDeleteRecordConstraintMsg);

            employeeObjectRepository.Delete(entity);
            repository.SaveChanges();
        }
    }
}
