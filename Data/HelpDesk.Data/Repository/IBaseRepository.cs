using HelpDesk.Entity;
using System.Linq;
using System.Linq.Expressions;
using System;
using HelpDesk.Data.Specification;

namespace HelpDesk.Data.Repository
{
    /// <summary>
    /// Общий для всех репозиториев интерфейс
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetList();

        IQueryable<T> GetList(ISpecification<T> specification);
        IQueryable<T> GetList(Expression<Func<T, bool>> predicate);

        void Delete(T entity);
        void Delete(long id);

        T Get(long id);

        T Get(ISpecification<T> specification);

        T Get(Expression<Func<T, bool>> predicate);

        void Save(T entity);

        void Insert(T entity, long id);

        int Count(Expression<Func<T, bool>> predicate = null);
    }
}
