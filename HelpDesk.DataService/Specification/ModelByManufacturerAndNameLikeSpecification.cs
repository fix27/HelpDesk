using HelpDesk.Data.Specification;
using HelpDesk.Entity;
using System;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Specification
{
    public class ModelByManufacturerAndNameLikeSpecification : Specification<Model>
    {
        private readonly long manufacturerId;
        private readonly string name;

        public ModelByManufacturerAndNameLikeSpecification(long manufacturerId, string name)
        {
            this.manufacturerId = manufacturerId;
            this.name = name;
        }

        public override Expression<Func<Model, bool>> IsSatisfied()
        {
            if (String.IsNullOrWhiteSpace(name))
                return s => false;

            return s => s.Manufacturer.Id == manufacturerId && s.Name.ToUpper().Contains(name.ToUpper());
        }
    }
}
