using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Helpers;
using HelpDesk.Data.Query;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Query
{
	public class UserRequestDeedlineAppEventSubscribeQueryParam
	{
		public IEnumerable<long> RequestIds { get; set; }
	}
	
	/// <summary>
    /// Запрос: список рассылки для подписанных на оповещение об истечении срока пользователей Исполнителя
    /// </summary>
    public class UserRequestDeedlineAppEventSubscribeQuery : IQuery<UserRequestDeedlineAppEventSubscribeQueryParam, IEnumerable<UserDeedlineAppEventSubscribeDTO>>
    {
		private readonly ISession _session;

		public UserRequestDeedlineAppEventSubscribeQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<UserDeedlineAppEventSubscribeDTO> Get(UserRequestDeedlineAppEventSubscribeQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			IEnumerable<Request> deedlineRequests = _session.Query<Request>().Where(t => param.RequestIds.Contains(t.Id)).ToList();
            if (!deedlineRequests.Any())
                return null;

            var q = _session.Query<AccessWorkerUser>().ToList()
                .Where(t => deedlineRequests.Select(r => r.Worker.Id).Contains(t.Worker.Id))
                .GroupBy(a => a.User.Email)
                .Select(g => new GroupedAccessWorkerUser
                {
                    Email = g.Key,
                    WorkerIds = g.Select(w => w.Worker.Id)
                });

            if (!q.Any())
                return null;

            IList<UserDeedlineAppEventSubscribeDTO> list = new List<UserDeedlineAppEventSubscribeDTO>();
            foreach (var t in q)
            {
                list.Add(new UserDeedlineAppEventSubscribeDTO
                {
                    Email = t.Email,
                    Items = deedlineRequests
                        .Where(r => t.WorkerIds.Contains(r.Worker.Id))
                        .Select(r => r.GetDTO())
                });
            }

            return list;
        }

        class GroupedAccessWorkerUser
        {
            public string Email { get; set; }
            public IEnumerable<long> WorkerIds { get; set; }
        }
    }
}
