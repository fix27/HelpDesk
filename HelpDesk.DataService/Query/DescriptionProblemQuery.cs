using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
	public class DescriptionProblemQueryParam
	{
		public string Name { get; set; }
		public long ObjectId { get; set; }
	}

	/// <summary>
	/// Запрос: Список проблем, зафиксированных для некоторого объекта (для ПО)/типа ТО (для ТО)
	/// </summary>
	public class DescriptionProblemQuery : IQuery<DescriptionProblemQueryParam, IEnumerable<SimpleDTO>>
    {
        private readonly ISession _session;

		public DescriptionProblemQuery(ISession session)
		{
			_session = session;
		}

		public IEnumerable<SimpleDTO> Get(DescriptionProblemQueryParam param)
        {
			if (param == null)
				throw new ArgumentNullException("param");

			if (param.ObjectId <=0)
				throw new ArgumentNullException("param.ObjectId <=0");

			if (param.Name == null)
				param.Name = "";

			var o = _session.Query<RequestObject>().FirstOrDefault(t => t.Id == param.ObjectId);

            if (!o.ObjectType.Soft)
                return (from p in _session.Query<DescriptionProblem>()
						where p.HardType.Id == o.HardType.Id && p.Name.ToUpper().Contains(param.Name.ToUpper())
                        select new SimpleDTO()
                        {
                            Id = p.Id,
                            Name = p.Name
                        }).ToList();


            return (from p in _session.Query<DescriptionProblem>()
					where p.RequestObject.Id == param.ObjectId && p.Name.ToUpper().Contains(param.Name.ToUpper())
                    select new SimpleDTO()
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();
        }
    }
}
