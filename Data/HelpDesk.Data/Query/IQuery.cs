using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.Data.Query
{
    public interface IQuery<TResult, TEntity1, TEntity2, TEntity3> 
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity
        where TEntity3 : BaseEntity
        
    {
        IEnumerable<TResult> Run(IQueryable<TEntity1> queryable1, 
            IQueryable<TEntity2> queryable2, 
            IQueryable<TEntity3> queryable3);
    }

    public interface IQuery<TResult, TEntity1, TEntity2>
        where TEntity1 : BaseEntity
        where TEntity2 : BaseEntity

    {
        IEnumerable<TResult> Run(IQueryable<TEntity1> queryable1,
            IQueryable<TEntity2> queryable2);
    }

    public interface IQuery<TResult, TEntity1>
        where TEntity1 : BaseEntity
        
    {
        IEnumerable<TResult> Run(IQueryable<TEntity1> queryable1);
    }
}
