using HelpDesk.Data.Query;

namespace HelpDesk.Data.NHibernate
{
	public class QueryHandler : IQueryHandler
	{
		public TDto Handle<TParam, TDto, TQuery>(TParam param, TQuery query)
			where TQuery : IQuery<TParam, TDto>
		{
			return query.Get(param);
		}
	}
}
