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
	public class AllowableObjectTypeQueryParam
	{
		public long EmployeeId { get; set; }
		public Expression<Func<OrganizationObjectTypeWorker, bool>> AccessPredicate { get; set; }
	}

	/// <summary>
	/// Запрос: доступные для выбора сотрудником-заявителем типы работ по ТО (чтобы потом определился исполнитель)
	/// </summary>
	public class AllowableObjectTypeQuery : IQuery<AllowableObjectTypeQueryParam, IEnumerable<SimpleDTO>>
    {
		private readonly ISession _session;

		public AllowableObjectTypeQuery(ISession session)
        {
			_session = session;
        }

		public IEnumerable<SimpleDTO> Get(AllowableObjectTypeQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			if (param.EmployeeId <= 0)
				throw new ArgumentException("param.EmployeeId <= 0");

			var q = from ootw in _session.Query<OrganizationObjectTypeWorker>()
					join e in _session.Query<Employee>() on ootw.Organization.Id equals e.Organization.Id
                    where e.Id == param.EmployeeId
						&& ootw.ObjectType.Soft == false
                        && ootw.ObjectType.Archive == false
                    orderby ootw.ObjectType.Name
                    select ootw;

            if(param.AccessPredicate !=null)
                q = q.Where(param.AccessPredicate);
            
            return q.Select(ootw => new SimpleDTO()
            {
                Id = ootw.ObjectType.Id,
                Name = ootw.ObjectType.Name
            }).Distinct().ToList();
        }
    }
}
