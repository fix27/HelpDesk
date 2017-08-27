﻿using HelpDesk.Data.Query;
using HelpDesk.DTO;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.Linq;

namespace HelpDesk.DataService.Query
{
    /// <summary>
    /// Запрос: годы подачи заявок в архиве
    /// </summary>
    public class ArchiveYearQuery : IQuery<Year, RequestArch>
    {
        private readonly long userId;

        public ArchiveYearQuery(long userId)
        {
            this.userId = userId;
        }
                
        public IEnumerable<Year> Run(IQueryable<RequestArch> requests)
        {
       
            var q = from e in requests
                    where e.Employee.Id == userId
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
