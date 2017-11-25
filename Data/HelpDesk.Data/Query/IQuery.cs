using HelpDesk.Entity;
using System.Linq;

namespace HelpDesk.Data.Query
{
    public interface IQuery<TResult, TEntity1, TEntity2, TEntity3, TEntity4, TEntity5>
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity
        where TEntity3 : BaseEntity
        where TEntity4 : BaseEntity
        where TEntity5 : BaseEntity

    {
        TResult Run(IQueryable<TEntity1> queryable1,
            IQueryable<TEntity2> queryable2,
            IQueryable<TEntity3> queryable3,
            IQueryable<TEntity4> queryable4,
            IQueryable<TEntity5> queryable5);
    }
    public interface IQuery<TResult, TEntity1, TEntity2, TEntity3, TEntity4>
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity
        where TEntity3 : BaseEntity
        where TEntity4 : BaseEntity

    {
        TResult Run(IQueryable<TEntity1> queryable1,
            IQueryable<TEntity2> queryable2,
            IQueryable<TEntity3> queryable3,
            IQueryable<TEntity4> queryable4);
    }

    public interface IQuery<TResult, TEntity1, TEntity2, TEntity3> 
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity
        where TEntity3 : BaseEntity
        
    {
        TResult Run(IQueryable<TEntity1> queryable1, 
            IQueryable<TEntity2> queryable2, 
            IQueryable<TEntity3> queryable3);
    }
        
    public interface IQuery<TResult, TEntity1, TEntity2>
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity

    {
        TResult Run(IQueryable<TEntity1> queryable1,
            IQueryable<TEntity2> queryable2);
    }

    public interface IQuery<TResult, TEntity1>
        where TEntity1 : BaseEntity
        
    {
        TResult Run(IQueryable<TEntity1> queryable1);
    }
}
