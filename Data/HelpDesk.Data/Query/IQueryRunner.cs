using HelpDesk.Entity;


namespace HelpDesk.Data.Query
{
    public interface IQueryRunner 
    {
        TResult Run<TResult, TEntity1, TEntity2, TEntity3>(IQuery<TResult, TEntity1, TEntity2, TEntity3> query) 
            where TEntity1 : BaseEntity
            where TEntity2 : BaseEntity
            where TEntity3 : BaseEntity;

        TResult Run<TResult, TEntity1, TEntity2>(IQuery<TResult, TEntity1, TEntity2> query)
            where TEntity1 : BaseEntity
            where TEntity2 : BaseEntity;

        TResult Run<TResult, TEntity1>(IQuery<TResult, TEntity1> query)
            where TEntity1 : BaseEntity;
    }
}
