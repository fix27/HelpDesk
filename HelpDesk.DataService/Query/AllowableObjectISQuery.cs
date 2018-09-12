using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Common.Helpers;
using System.Linq.Expressions;
using System;
using NHibernate;

namespace HelpDesk.DataService.Query
{
	public class AllowableObjectISQueryParam
	{
		public long EmployeeId { get; set; }
		public string Name { get; set; }
		public Expression<Func<RequestObject, bool>> AccessPredicate { get; set; }
	}

	/// <summary>
	/// Запрос: доступные для выбора сотрудником-заявителем ИС
	/// </summary>
	public class AllowableObjectISQuery : IQuery<AllowableObjectISQueryParam, IEnumerable<RequestObjectISDTO>>
    {
		private readonly ISession _session;
		
		public AllowableObjectISQuery(ISession session)
        {
			_session = session;
        }
		
		public IEnumerable<RequestObjectISDTO> Get(AllowableObjectISQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			if (param.EmployeeId <= 0)
				throw new ArgumentException("param.EmployeeId <= 0");


            var q = from o in _session.Query<RequestObject>()
					join ootw in _session.Query<OrganizationObjectTypeWorker>() on o.ObjectType.Id equals ootw.ObjectType.Id
                    join e in _session.Query<Employee>() on ootw.Organization.Id equals e.Organization.Id
                    where o.ObjectType.Soft == true 
                        && e.Id == param.EmployeeId
						&& o.ObjectType.Archive == false 
                        && o.Archive == false                       
                    orderby o.SoftName, o.ObjectType.Name
                    select o;

            if (param.Name != null)
                q = q.Where(o => o.SoftName != null && o.SoftName.ToUpper().Contains(param.Name.ToUpper()) ||
                        o.SoftName == null && o.ObjectType.Name.ToUpper().Contains(param.Name.ToUpper()));

            if(param.AccessPredicate != null)
                q = q.Where(param.AccessPredicate);

            return q.Select(o => new RequestObjectISDTO()
            {
                Id = o.Id,
                SoftName = o.SoftName,
                ObjectTypeName = o.ObjectType.Name
            }).Distinct().ToList();
        }
    }
}
