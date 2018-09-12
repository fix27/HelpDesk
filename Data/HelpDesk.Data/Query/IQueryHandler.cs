namespace HelpDesk.Data.Query
{
	public interface IQueryHandler
	{
		TDto Handle<TParam, TDto, TQuery>(TParam param, TQuery query)
			where TQuery : IQuery<TParam, TDto>;
	}
	
}
