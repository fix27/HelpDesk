using HelpDesk.Data.Specification;
using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Specification
{
    public class SimpleEntityAllOrByNameLikeSpecification<T> : Specification<T>
        where T : SimpleEntity
    {
        private readonly string name;

        public SimpleEntityAllOrByNameLikeSpecification(string name)
        {
            this.name = name;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            return s => name == null || s.Name.ToUpper().Contains(name.ToUpper());
        }
    }
}
