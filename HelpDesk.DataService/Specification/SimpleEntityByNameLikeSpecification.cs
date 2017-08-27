using HelpDesk.Data.Specification;
using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Specification
{
    public class SimpleEntityByNameLikeSpecification<T> :Specification<T>
        where T : SimpleEntity

    {
        private readonly string name;

        public SimpleEntityByNameLikeSpecification(string name)
        {
            this.name = name;
        }

        public override Expression<Func<T, bool>> IsSatisfied()
        {
            if (String.IsNullOrWhiteSpace(name))
                return s => false;

            return s => s.Name.ToUpper().Contains(name.ToUpper());
        }
    }
}
