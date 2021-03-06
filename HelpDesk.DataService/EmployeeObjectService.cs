﻿using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
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
using System.Linq.Expressions;
using System;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с профилем заявителя - сотрудника обслуживаемой организации
    /// </summary>
    [Transaction]
    [Cache]
    public class EmployeeObjectService : BaseService, IEmployeeObjectService
    {

        private readonly IQueryHandler queryHandler;
        private readonly IBaseRepository<EmployeeObject> employeeObjectRepository;
        private readonly IBaseRepository<Employee> employeeRepository;
        private readonly IBaseRepository<RequestObject> objectRepository;
        private readonly IBaseRepository<HardType> hardTypeRepository;
        private readonly IBaseRepository<Manufacturer> manufacturerRepository;
        private readonly IBaseRepository<Model> modelRepository;
        private readonly IBaseRepository<ObjectType> objectTypeRepository;
        private readonly IBaseRepository<WorkerUser> workerUserRepository;
        private readonly IBaseRepository<AccessWorkerUser> accessWorkerUserRepository;
        private readonly IAccessWorkerUserExpressionService accessWorkerUserExpressionService;

		private readonly IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>> _employeeObjectQuery;
		private readonly IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>>  _allowableObjectISQuery;
		private readonly IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>> _allowableObjectTypeQuery;

		private readonly IRepository repository;

        public EmployeeObjectService(
            IQueryHandler queryHandler,
            IBaseRepository<EmployeeObject> employeeObjectRepository,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<RequestObject> objectRepository,
            IBaseRepository<HardType> hardTypeRepository,
            IBaseRepository<Manufacturer> manufacturerRepository,
            IBaseRepository<Model> modelRepository,
            IBaseRepository<ObjectType> objectTypeRepository,
            IBaseRepository<WorkerUser> workerUserRepository,
            IBaseRepository<AccessWorkerUser> accessWorkerUserRepository,
            IAccessWorkerUserExpressionService accessWorkerUserExpressionService,

			IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>> employeeObjectQuery,
			IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>> allowableObjectISQuery,
			IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>> allowableObjectTypeQuery,
			
			IRepository repository)
        {
            this.queryHandler                 = queryHandler;
            this.employeeObjectRepository    = employeeObjectRepository;
            this.employeeRepository          = employeeRepository;
            this.objectRepository            = objectRepository;
            this.hardTypeRepository          = hardTypeRepository;
            this.manufacturerRepository      = manufacturerRepository;
            this.modelRepository             = modelRepository;
            this.objectTypeRepository        = objectTypeRepository;
            this.workerUserRepository        = workerUserRepository;
            this.accessWorkerUserRepository  = accessWorkerUserRepository;
            this.accessWorkerUserExpressionService = accessWorkerUserExpressionService;

			_employeeObjectQuery = employeeObjectQuery;
			_allowableObjectISQuery = allowableObjectISQuery;
			_allowableObjectTypeQuery = allowableObjectTypeQuery;

			this.repository                  = repository;
        }

		public IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, EmployeeObjectFilter filter, OrderInfo orderInfo, ref PageInfo pageInfo)
		{
			var param = new EmployeeObjectQueryParam
			{
				EmployeeId = employeeId,
				Filter = filter,
				OrderInfo = orderInfo,
				PageInfo = pageInfo
			};
			var list = queryHandler
				.Handle<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>, IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>>
					(param, _employeeObjectQuery);
			return list; 
        }

        public IEnumerable<EmployeeObjectDTO> GetListEmployeeObject(long employeeId, string objectName = null)
        {
            var param = new EmployeeObjectQueryParam
			{
				EmployeeId = employeeId,
				Filter = new EmployeeObjectFilter() { ObjectName = objectName }
			};
			var list = queryHandler
				.Handle<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>, IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>>
					(param, _employeeObjectQuery);
			return list;
        }

        public bool AllowableForSendRequest(long employeeId)
        {
            var param = new EmployeeObjectQueryParam
			{
				EmployeeId = employeeId
			};
			var list = queryHandler
				.Handle<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>, IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>>
					(param, _employeeObjectQuery);

			return list!=null && list.Any();
        }

        #region GetListAllowable
        public IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long employeeId, string name = null)
        {

			var list = queryHandler
				.Handle<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>, IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>>>
					(new AllowableObjectISQueryParam { EmployeeId = employeeId, Name = name }, _allowableObjectISQuery);


			var listEmployeeObjectIds = queryHandler
				.Handle<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>, IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>>
					(new EmployeeObjectQueryParam
					{
						EmployeeId = employeeId
					}, _employeeObjectQuery)
					.Select(t => t.ObjectId);

			return list.Where(t => !listEmployeeObjectIds.Contains(t.Id));
        }

        public IEnumerable<SimpleDTO> GetListAllowableObjectType(long employeeId)
        {
            var list = queryHandler
				.Handle<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>, IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>>>
					(new AllowableObjectTypeQueryParam
					{
						EmployeeId = employeeId
					}, _allowableObjectTypeQuery);

			return list;
        }

        public IEnumerable<RequestObjectISDTO> GetListAllowableObjectIS(long userId, long employeeId, string name = null)
        {
            WorkerUser user = workerUserRepository.Get(userId);
            Expression<Func<RequestObject, bool>> accessPredicate = accessWorkerUserExpressionService
                .GetAccessRequestObjectPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));

            var list = queryHandler
				.Handle<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>, IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>>>
					(new AllowableObjectISQueryParam
					{
						EmployeeId = employeeId,
						Name = name,
						AccessPredicate = accessPredicate
					}, _allowableObjectISQuery);

			var listEmployeeObjectIds = queryHandler
				.Handle<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>, IQuery<EmployeeObjectQueryParam, IEnumerable<EmployeeObjectDTO>>>
					(new EmployeeObjectQueryParam
					{
						EmployeeId = employeeId
					}, _employeeObjectQuery)
					.Select(t => t.ObjectId);

			return list.Where(t => !listEmployeeObjectIds.Contains(t.Id));
        }

        public IEnumerable<SimpleDTO> GetListAllowableObjectType(long userId, long employeeId)
        {
            WorkerUser user = workerUserRepository.Get(userId);
            Expression<Func<OrganizationObjectTypeWorker, bool>> accessPredicate = accessWorkerUserExpressionService
                .GetAccessOrganizationObjectTypeWorkerPredicate(accessWorkerUserRepository.GetList(a => a.User.Id == userId));

			var list = queryHandler
				.Handle<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>, IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>>>
					(new AllowableObjectTypeQueryParam
					{
						EmployeeId = employeeId,
						 AccessPredicate = accessPredicate
					}, _allowableObjectTypeQuery);

			return list;
		}
        #endregion GetListAllowable

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

            dto.Id = entity.Object.Id;
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

                if (!model.Name.ToUpper().Contains(hardType.Name.ToUpper()))
                    model.Name = String.Format("{0} {1}", hardType.Name.ToUpper(), model.Name);

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

            dto.Id = entity.Object.Id;
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
