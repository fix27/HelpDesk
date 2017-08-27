using System.Collections.Generic;
using HelpDesk.Common.Helpers;
using System;
using HelpDesk.DTO;

namespace HelpDesk.DataService.Filters
{
    /// <summary>
    /// Для фильтрации в гриде "Мои заявки"
    /// </summary>
    public class RequestFilter
    {
        /// <summary>
        /// Номер заявки, или номера заявок ч/з запятую
        /// </summary>
        public string Id { get; set; }

        public IList<long> Ids { get { return Id.ToArrayValues<long>(); } }

        public string ObjectName { get; set; }
        
        /// <summary>
        /// Описание проблемы
        /// </summary>
        public string DescriptionProblem { get; set; }

        public IList<StatusRequestEnum> StatusIds { get; set; }

        public DateTime? DateInsert1 { get; set; }
        public DateTime? DateInsert2 { get; set; }
        public DateTime? DateEndPlan1 { get; set; }
        public DateTime? DateEndPlan2 { get; set; }


        /// <summary>
        /// Год для поиска в архиве
        /// </summary>
        public int ArchiveYear { get; set; }

        /// <summary>
        /// Месяц для поиска в архиве
        /// </summary>
        public int ArchiveMonth { get; set; }

        /// <summary>
        /// Год для поиска в архиве, среди которых осуществляется выбор
        /// </summary>
        public IList<Year> ArchiveYears { get; set; }

        /// <summary>
        /// Месяц для поиска в архиве, среди которых осуществляется выбор
        /// </summary>
        public IList<Month> ArchiveMonths { get; set; }

        /// <summary>
        /// Поиск в архиве
        /// </summary>
        public bool Archive { get; set; }

    }
}
