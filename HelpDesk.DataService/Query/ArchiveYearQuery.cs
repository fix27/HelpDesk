using HelpDesk.Data.Query;
using HelpDesk.DataService.DTO;
using HelpDesk.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: годы подачи заявок в архиве
    /// </summary>
    public class ArchiveYearQuery : IQuery<IEnumerable<Year>, RequestArch>
    {
        private readonly Expression<Func<BaseRequest, bool>> accessPredicate;

        public ArchiveYearQuery(Expression<Func<BaseRequest, bool>> accessPredicate)
        {
            this.accessPredicate = accessPredicate;
        }
                
        public IEnumerable<Year> Run(IQueryable<RequestArch> requests)
        {
       
            var q = from e in requests.Where(accessPredicate)
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
