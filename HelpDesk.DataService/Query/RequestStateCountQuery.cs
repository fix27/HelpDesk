using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;
using NHibernate;

namespace HelpDesk.DataService.Query
{
	public class RequestStateCountQueryParam
	{
		public Expression<Func<BaseRequest, bool>> AccessPredicate { get; set; }
	}

	/// <summary>
	/// Запрос: Статистика активных заявок по состояниям
	/// </summary>
	public class RequestStateCountQuery : IQuery<RequestStateCountQueryParam, IEnumerable<RequestStateCountDTO>>
    {
		private readonly ISession _session;

		public RequestStateCountQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<RequestStateCountDTO> Get(RequestStateCountQueryParam param)
        {
          
            var q = from r in _session.Query<Request>().Where(param.AccessPredicate)
                    group r by new { r.Status.Id, r.Status.Name, r.Status.BackColor } into g
                    select new RequestStateCountDTO
                    {
                        StatusId = g.Key.Id,
                        Count = g.Count(),
                        StatusName = g.Key.Name,
                        StatusBackColor = g.Key.BackColor
                    };

            return q.ToList();
            
        }
    }
}
