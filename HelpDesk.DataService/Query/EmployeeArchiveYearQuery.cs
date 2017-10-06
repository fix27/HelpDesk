using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: годы подачи заявок пользователя в архиве
    /// </summary>
    public class EmployeeArchiveYearQuery : IQuery<IEnumerable<Year>, RequestArch>
    {
        private readonly long employeeId;

        public EmployeeArchiveYearQuery(long employeeId)
        {
            this.employeeId = employeeId;
        }
                
        public IEnumerable<Year> Run(IQueryable<RequestArch> requests)
        {
       
            var q = from e in requests
                    where e.Employee.Id == employeeId
                    group e by e.DateInsert.Year into g
                    select new Year
                    {
                         Name = g.Key.ToString(),
                         Ord = g.Key
                    };

            return q.ToList();
        }
    }
}
