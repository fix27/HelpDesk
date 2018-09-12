using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
	public class EmployeeArchiveYearQueryParam
	{
		public long EmployeeId { get; set; }		
	}

	/// <summary>
	/// Запрос: годы подачи заявок пользователя в архиве
	/// </summary>
	public class EmployeeArchiveYearQuery : IQuery<EmployeeArchiveYearQueryParam, IEnumerable<Year>>
    {
		private readonly ISession _session;

		public EmployeeArchiveYearQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<Year> Get(EmployeeArchiveYearQueryParam param)
        {

			if (param == null)
				throw new ArgumentNullException("param");

			if (param.EmployeeId <= 0)
				throw new ArgumentException("param.EmployeeId <= 0");

			var q = from e in _session.Query<RequestArch>()
                    where e.Employee.Id == param.EmployeeId
					group e by e.DateInsert.Year into g
                    select new Year
                    {
                         Name = g.Key.ToString(),
                         Ord = g.Key
                    };

            return q.ToList();
        }
    }
}
