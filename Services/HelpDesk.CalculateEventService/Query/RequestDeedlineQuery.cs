using HelpDesk.Data.Query;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using NHibernate;

namespace HelpDesk.CalculateEventJob.Query
{
	/// <summary>
	/// Запрос: список Id заявок, у которых истекает срок исполнения
	/// </summary>
	public class RequestDeedlineQuery : IQuery<object, IEnumerable<long>>
    {
		private readonly ISession _session;

		public RequestDeedlineQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<long> Get(object param)
        {
            var q = _session.Query<Request>()
				.ToList()
                .Where(r => r.Object.ObjectType.DeadlineHour > 0 && !r.DateEndFact.HasValue &&
                    r.DateEndPlan <= DateTime.Now.AddHours(r.Object.ObjectType.DeadlineHour) && r.DateEndPlan > DateTime.Now)
                .Select(r => r.Id);
            
            return q;
        }
    }
}
