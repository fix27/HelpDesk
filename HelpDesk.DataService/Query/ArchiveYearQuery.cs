using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Query
{
	public class ArchiveYearQueryParam
	{
		public Expression<Func<BaseRequest, bool>> AccessPredicate { get; set; }
	}

	/// <summary>
	/// Запрос: годы подачи заявок в архиве
	/// </summary>
	public class ArchiveYearQuery : IQuery<ArchiveYearQueryParam, IEnumerable<Year>>
    {
		private readonly ISession _session;

		public ArchiveYearQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<Year> Get(ArchiveYearQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			if (param.AccessPredicate == null)
				throw new ArgumentNullException("param.AccessPredicate");

			var q = from e in _session.Query<RequestArch>().Where(param.AccessPredicate)
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
