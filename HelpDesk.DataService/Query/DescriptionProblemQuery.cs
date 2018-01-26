using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: Список проблем, зафиксированных для некоторого объекта (для ПО)/типа ТО (для ТО)
    /// </summary>
    public class DescriptionProblemQuery : IQuery<IEnumerable<SimpleDTO>, DescriptionProblem, RequestObject>
    {
        private readonly string name;
        private readonly long objectId;


        public DescriptionProblemQuery(string name, long objectId)
        {
            this.name = name;
            this.objectId = objectId;
        }


        public IEnumerable<SimpleDTO> Run(IQueryable<DescriptionProblem> descriptionProblems, IQueryable<RequestObject> objects)
        {
            var o = objects.FirstOrDefault(t => t.Id == objectId);

            if (!o.ObjectType.Soft)
                return (from p in descriptionProblems
                        where p.HardType.Id == o.HardType.Id && p.Name.ToUpper().Contains(name.ToUpper())
                        select new SimpleDTO()
                        {
                            Id = p.Id,
                            Name = p.Name
                        }).ToList();


            return (from p in descriptionProblems
                    where p.RequestObject.Id == objectId && p.Name.ToUpper().Contains(name.ToUpper())
                    select new SimpleDTO()
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList();
        }
    }
}
