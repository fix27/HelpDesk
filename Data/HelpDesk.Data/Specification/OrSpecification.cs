using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.Data.Specification
{
    public class OrSpecification<T> : Specification<T>
        where T : BaseEntity
    {
        private readonly Specification<T> specification1;
        private readonly ISpecification<T> specification2;
        public OrSpecification(Specification<T> specification1, ISpecification<T> specification2)
        {
            this.specification1 = specification1;
            this.specification2 = specification2;
        }
        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return specification1.IsSatisfied().Or(specification2.IsSatisfied());
        }
    }
}
