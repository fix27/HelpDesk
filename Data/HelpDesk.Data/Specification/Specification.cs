using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.Data.Specification
{
    public abstract class Specification<T> : ISpecification<T>
        where T : BaseEntity
    {
        public abstract Expression<Func<T, bool>> IsSatisfied();
        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }
    }
        
}
