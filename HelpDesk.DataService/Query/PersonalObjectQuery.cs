using HelpDesk.Common;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Filters;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System;
using HelpDesk.DataService.Interface;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: объекты заявок пользователя (профиль заявителя)
    /// </summary>
    public class PersonalObjectQuery : IQuery<PersonalProfileObjectDTO, PersonalProfileObject>
    {
        private readonly long userId;
        private readonly IConstantStatusRequestService constantService;
        private readonly OrderInfo orderInfo;
        private readonly PageInfo pageInfo;
        private readonly PersonalObjectFilter filter;


        public PersonalObjectQuery(long userId, IConstantStatusRequestService constantService, PersonalObjectFilter filter, OrderInfo orderInfo, PageInfo pageInfo)
            :this(userId, constantService, filter)
        {
            this.orderInfo = orderInfo;
            this.pageInfo = pageInfo;
        }

        public PersonalObjectQuery(long userId, IConstantStatusRequestService constantService, PersonalObjectFilter filter)
            : this(userId, constantService)
        {
            this.filter = filter;
        }

        public PersonalObjectQuery(long userId, IConstantStatusRequestService constantService)
        {
            this.userId = userId;
            this.constantService = constantService;
        }

        public IEnumerable<PersonalProfileObjectDTO> Run(IQueryable<PersonalProfileObject> personalProfileObjects)
        {
            
            var qb = (from uo in personalProfileObjects
                      where uo.PersonalProfile.Id == userId && uo.Object.Archive == false
                      select new PersonalProfileObjectDTO()
                      {
                          ObjectId = uo.Object.Id,
                          Id = uo.Id,
                          SoftName = uo.Object.SoftName,
                          ObjectType = uo.Object.ObjectType,
                          Soft = uo.Object.ObjectType.Soft,
                          HardType = uo.Object.HardType,
                          Model = uo.Object.Model,
                          ObjectTypeName = uo.Object.ObjectType.Name
                      });

            var q = qb;
            if (filter!= null && !String.IsNullOrWhiteSpace(filter.ObjectName))
                q = qb.Where(t => t.SoftName.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.HardType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.Model.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.Model.Manufacturer.Name.ToUpper().Contains(filter.ObjectName.ToUpper()) ||
                t.ObjectType.Name.ToUpper().Contains(filter.ObjectName.ToUpper()));
                    

            if (filter != null && filter.Wares !=null && filter.Wares.Any())
                q = q.Where(t => filter.Wares.Contains(t.Soft));

            if (orderInfo != null)
            {
                switch (orderInfo.PropertyName)
                {
                    case "WorkTypes":
                        if (orderInfo.Asc)
                            q = q.OrderBy(t => t.Soft)
                                .ThenBy(t => t.ObjectType.Name);
                        else
                            q = q.OrderByDescending(t => t.Soft)
                                .ThenByDescending(t => t.ObjectType.Name);
                        break;
                    case "ObjectName":
                        if (orderInfo.Asc)
                            q = q.OrderBy(t => t.SoftName)
                                .ThenBy(t => t.Model.Manufacturer.Name)
                                .ThenBy(t => t.Model.Name)
                                .ThenBy(t => t.ObjectType.Name)
                                .ThenBy(t => t.Soft);
                        else
                            q = q.OrderByDescending(t => t.SoftName)
                                .ThenByDescending(t => t.Model.Manufacturer.Name)
                                .ThenByDescending(t => t.Model.Name)
                                .ThenByDescending(t => t.ObjectType.Name)
                                .ThenByDescending(t => t.Soft);
                        break;
                    
                }
            }
                

            if (pageInfo != null)
            {
                pageInfo.TotalCount = qb.Count();
                pageInfo.Count = q.Count();
            }

            if (pageInfo != null && pageInfo.PageSize > 0)
                q = q.Skip(pageInfo.PageSize * pageInfo.CurrentPage).Take(pageInfo.PageSize);

            return q.ToList();
        }
    }
}
