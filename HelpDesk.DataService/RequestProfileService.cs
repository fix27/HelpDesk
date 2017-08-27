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
    /// Для работы с профилем заявителя
    /// </summary>
    [Transaction]
    public class RequestProfileService : BaseService, IRequestProfileService
    {

        private readonly IQueryRunner queryRunner;
        private readonly IBaseRepository<PersonalProfileObject> personalProfileObjectRepository;
        private readonly IBaseRepository<PersonalProfile> personalProfileRepository;
        private readonly IBaseRepository<RequestObject> objectRepository;
        private readonly IBaseRepository<HardType> hardTypeRepository;
        private readonly IBaseRepository<Manufacturer> manufacturerRepository;
        private readonly IBaseRepository<Model> modelRepository;
        private readonly IBaseRepository<ObjectType> objectTypeRepository;
        private readonly IConstantStatusRequestService constantService;
        private readonly IRepository repository;

        public RequestProfileService(
            IQueryRunner queryRunner,
            IBaseRepository<PersonalProfileObject> personalProfileObjectRepository,
            IBaseRepository<PersonalProfile> personalProfileRepository,
            IBaseRepository<RequestObject> objectRepository,
            IBaseRepository<HardType> hardTypeRepository,
            IBaseRepository<Manufacturer> manufacturerRepository,
            IBaseRepository<Model> modelRepository,
            IBaseRepository<ObjectType> objectTypeRepository,
            IConstantStatusRequestService constantService,
            IRepository repository)
        {
            this.queryRunner                        = queryRunner;
            this.personalProfileObjectRepository    = personalProfileObjectRepository;
            this.personalProfileRepository          = personalProfileRepository;
            this.objectRepository                   = objectRepository;
            this.hardTypeRepository                 = hardTypeRepository;
            this.manufacturerRepository             = manufacturerRepository;
            this.modelRepository                    = modelRepository;
            this.objectTypeRepository               = objectTypeRepository;
            this.constantService                    = constantService;
            this.repository                         = repository;
        }
        
        public IEnumerable<PersonalProfileObjectDTO> GetListPersonalObject(long userId, PersonalObjectFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
        {
            IEnumerable<PersonalProfileObjectDTO> list = queryRunner.Run(new PersonalObjectQuery(userId, constantService, filter, orderInfo, pageInfo));
            return list;
        }

        public IEnumerable<PersonalProfileObjectDTO> GetListPersonalObject(long userId, string objectName = null)
        {
            IEnumerable<PersonalProfileObjectDTO> list = queryRunner.Run(new PersonalObjectQuery(userId, constantService, new PersonalObjectFilter() { ObjectName = objectName }));
            return list;
        }

        public bool AllowableForSendRequest(long userId)
        {
            IEnumerable<PersonalProfileObjectDTO> list = queryRunner.Run(new PersonalObjectQuery(userId, constantService));
            return list!=null && list.Any();
        }

        public IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long userId, string name = null)
        {
            IEnumerable<RequestObjectISDTO> list = queryRunner.Run(new AllowableObjectISQuery(userId, constantService, name));

            IEnumerable<long> listPersonalObjectIds = queryRunner.Run(new PersonalObjectQuery(userId, constantService))
                .Select(t => t.ObjectId);

            return list.Where(t => !listPersonalObjectIds.Contains(t.Id));
        }

        public IEnumerable<SimpleDTO> GetListAllowableObjectType(long userId)
        {
            IEnumerable<SimpleDTO> list = queryRunner.Run(new AllowableObjectTypeQuery(userId, constantService));
            return list;
        }

        [Transaction]
        public void AddIS(long userId, RequestObjectISDTO dto)
        {
            PersonalProfileObject entity = personalProfileObjectRepository
                .Get(t => t.Object.Id == dto.Id && t.PersonalProfile.Id == userId);

            if(entity != null)
                setErrorMsg("ObjectName", Resource.UniquePersonalProfileObjectConstraintMsg);

            if (errorMessages.Count > 0)
                throw new DataServiceException(Resource.GeneralConstraintMsg, errorMessages);

            entity = new PersonalProfileObject()
            {
                 Object = objectRepository.Get(dto.Id),
                 PersonalProfile = personalProfileRepository.Get(userId)
            };

            personalProfileObjectRepository.Save(entity);
            repository.SaveChanges();
        }

        [Transaction]
        public void AddTO(long userId, RequestObjectTODTO dto)
        {
            PersonalProfileObject entity = personalProfileObjectRepository
                .Get(t => t.Object.Id == dto.Id && t.PersonalProfile.Id == userId);

            if (entity != null)
                setErrorMsg("ObjectName", Resource.UniquePersonalProfileObjectConstraintMsg);

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

            entity = new PersonalProfileObject()
            {
                Object = requestObject,
                PersonalProfile = personalProfileRepository.Get(userId)
            };

            personalProfileObjectRepository.Save(entity);
            repository.SaveChanges();
        }

        [Transaction]
        public void Delete(long userId, long id)
        {
            PersonalProfileObject entity = personalProfileObjectRepository.Get(id);

            if(entity == null)
                throw new DataServiceException(Resource.AnotherUserDeleteRecordConstraintMsg);

            if (entity.PersonalProfile.Id != userId)
                throw new DataServiceException(Resource.OnlyOwnerPersonalProfileObjectCanDeleteRecordConstraintMsg);

            personalProfileObjectRepository.Delete(entity);
            repository.SaveChanges();
        }
    }
}
