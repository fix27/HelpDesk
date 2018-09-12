using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.Helpers;
using HelpDesk.Data.Query;
using HelpDesk.DataService.Common.DTO;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.ConsumerEventService.Query
{
	public class UserRequestAppEventSubscribeQueryParam
	{
		public long RequestEventId { get; set; }
		public Func<long, StatusRequestEnum> GetEquivalenceByElement { get; set; }
		public bool Archive { get; set; }
	}
	
	/// <summary>
	/// Запрос: списки рассылки для подписанных на событие пользователей Исполнителя (первый элемент Tuple) 
	/// и пользователей личного кабинета (второй элемент Tuple)
	/// </summary>
	public class UserRequestAppEventSubscribeQuery : IQuery<UserRequestAppEventSubscribeQueryParam, 
		Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>>>
    {

		private readonly ISession _session;

		public UserRequestAppEventSubscribeQuery(ISession session)
		{
			_session = session;
		}

		                
        public Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, IEnumerable<UserRequestAppEventSubscribeDTO>> Get(UserRequestAppEventSubscribeQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			BaseRequest r = null;
            BaseRequestEvent evnt = null;

            if (param.Archive)
            {
                evnt = _session.Query<RequestEventArch>().FirstOrDefault(t => t.Id == param.RequestEventId);

                if (evnt == null)
                    return null;

                r = _session.Query<RequestArch>().First(t => t.Id == evnt.RequestId);
            }
            else
            {
                evnt = _session.Query<RequestEvent>().FirstOrDefault(t => t.Id == param.RequestEventId);
                if (evnt == null)
                    return null;

                r = _session.Query<Request>().First(t => t.Id == evnt.RequestId);
            }

            RequestDTO request = r.GetDTO();


            IEnumerable<long> userIds = _session.Query<AccessWorkerUser>()
                .Where(t => t.Worker.Id == r.Worker.Id && 
                    (t.User.UserType.TypeCode == TypeWorkerUserEnum.Worker || 
                     t.User.UserType.TypeCode == TypeWorkerUserEnum.WorkerAndDispatcher))
                .Select(t => t.User.Id).ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> ws =
				_session.Query<WorkerUserEventSubscribe>().Where(t => userIds.Contains(t.User.Id) && 
                    t.User.Id != request.User.Id && 
                    t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    Request = request,
                    Email = t.User.Email,
                    IsWorker = true
                })
                .ToList();

            IEnumerable<UserRequestAppEventSubscribeDTO> cs =
				_session.Query<CabinetUserEventSubscribe>().Where(t => t.User.Id == r.Employee.Id && 
                    t.User.Subscribe)
                .Select(t => new UserRequestAppEventSubscribeDTO
                {
                    Request = request,
                    Email = t.User.Email
                })
                .ToList();

            return new Tuple<IEnumerable<UserRequestAppEventSubscribeDTO>, 
                             IEnumerable<UserRequestAppEventSubscribeDTO>>(ws, cs);
        }
    }
}
