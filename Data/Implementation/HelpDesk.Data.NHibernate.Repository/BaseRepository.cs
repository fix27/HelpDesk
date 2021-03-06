﻿using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;
using System;
using HelpDesk.Data.Specification;
using NHibernate.Linq;

namespace HelpDesk.Data.NHibernate.Repository
{
    public class BaseRepository : IRepository
    {

        protected readonly ISession session;


        public BaseRepository(ISession session)
        {
            this.session = session;
        }

        public void SaveChanges()
        {
            session.Flush();
            session.Clear();
        }
    }

    public class BaseRepository<T> : BaseRepository, IBaseRepository<T>
        where T : BaseEntity
    {

        public BaseRepository(ISession session)
            : base(session)
        {

        }

        public virtual IQueryable<T> GetList()
        {
            return session.Query<T>();
        }

        public virtual IQueryable<T> GetList(ISpecification<T> specification)
        {
            return session.Query<T>().Where(specification.IsSatisfied());
        }

        public virtual IQueryable<T> GetList(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return session.Query<T>().Where(predicate);
        }
        public virtual void Delete(T entity)
        {
            session.Delete(entity);
        }

        public virtual void Delete(long id)
        {
            session.Query<T>()
                .Where(c => c.Id == id)
                .Delete();
        }

        public virtual T Get(long id)
        {
            ICriteria c = session.CreateCriteria<T>()
                .Add(Expression.Eq("Id", id));

            T t = c.UniqueResult<T>();
            return t;
        }

        public virtual T Get(ISpecification<T> specification)
        {
            return session.Query<T>().FirstOrDefault(specification.IsSatisfied());
        }
        public virtual T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return session.Query<T>().FirstOrDefault(predicate);
        }

        public virtual int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate = null)
        {
            if(predicate == null)
                return session.Query<T>().Count();

            return session.Query<T>().Count(predicate);
        }

        public virtual void Save(T entity)
        {
            if (entity.Id > 0)
            {
                T t = session.Load<T>(entity.Id);
                session.Merge(entity);
            }
            else
                session.Save(entity);
        }

        public virtual void Insert(T entity, long id)
        {
            session.Save(entity, id);
        }

        public virtual void Update(object values, System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            session.Query<T>()
                .Where(predicate)
                .Update(c => values);
        }
        public virtual void Delete(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            session.Query<T>()
                .Where(predicate)
                .Delete();
        }
    }
}
