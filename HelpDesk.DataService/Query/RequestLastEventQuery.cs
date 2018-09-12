using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
	public class RequestLastEventQueryParam
	{
		public IEnumerable<long> RequestIds { get; set; }
		public bool WithDateEnd { get; set; }
	}

	/// <summary>
	/// Запрос: последнee событиe заявок из списка Id-заявок
	/// </summary>
	public class RequestLastEventQuery : IQuery<RequestLastEventQueryParam, IEnumerable<RequestEventDTO>>
    {
		private readonly ISession _session;

		public RequestLastEventQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<RequestEventDTO> Get(RequestLastEventQueryParam param)
        {

			if (param == null)
				throw new ArgumentNullException("param");

			//ВНИМАНИЕ!!! Запрос для ids сразу материализуется при помощи ToList(), 
			//так как иначе NH не может его нормально преобразовать в sql


			IEnumerable<long> ids = (from z in _session.Query<RequestEvent>()
                                     where param.RequestIds.Contains(z.RequestId) && (!param.WithDateEnd || z.StatusRequest.Id != (long)RawStatusRequestEnum.DateEnd)
                                     group z by z.RequestId into g
                                     select g.Max(d => d.Id)).ToList();

            var q = from e in _session.Query<RequestEvent>()
					where ids.Contains(e.Id)
                    select new RequestEventDTO
                    {
                        RequestId = e.RequestId,
                        DateEvent = e.DateEvent,
                        Note = e.Note,
                        Transfer = e.StatusRequest.Id == (long)RawStatusRequestEnum.ExtendedDeadLine,
                        OrdGroup = e.OrdGroup
                    };

            //IEnumerable<long> ids = events
            //    .Where(z => requestIds.Contains(z.RequestId) && z.StatusRequest.Id != (long)RawStatusRequestEnum.DateEnd)
            //    .GroupBy(z => z.RequestId)
            //    .Select(z => z.Max(d => d.Id)).ToList();


            //var q = events.Where(e => ids.Contains(e.Id))
            //    .Select(e => new RequestEventDTO
            //    {
            //        RequestId = e.RequestId,
            //        DateEvent = e.DateEvent,
            //        Note = e.Note,
            //        Transfer = e.StatusRequest.Id == (long)RawStatusRequestEnum.ExtendedDeadLine,
            //        OrdGroup = e.OrdGroup
            //    });


            return q.ToList();
        }
    }
}
