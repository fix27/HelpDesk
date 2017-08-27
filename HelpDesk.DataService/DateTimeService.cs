using HelpDesk.DataService.Interface;
using HelpDesk.DTO;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для работы с текущей датой, временем и т.п.
    /// </summary>
    public class DateTimeService : BaseDataService, IDateTimeService
    {
        public DateTime GetCurrent()
        {
            return DateTime.Now;
        }

        public IList<Month> GetListMonth()
        {
            return new List<Month>()
            {
                new Month() { Ord = 1, Name = "Январь" },
                new Month() { Ord = 2, Name = "Февраль" },
                new Month() { Ord = 3, Name = "Март" },
                new Month() { Ord = 4, Name = "Апрель" },
                new Month() { Ord = 5, Name = "Май" },
                new Month() { Ord = 6, Name = "Июнь" },
                new Month() { Ord = 7, Name = "Июль" },
                new Month() { Ord = 8, Name = "Август" },
                new Month() { Ord = 9, Name = "Сентябрь" },
                new Month() { Ord = 10, Name = "Октябрь" },
                new Month() { Ord = 11, Name = "Ноябрь" },
                new Month() { Ord = 12, Name = "Декабрь" }
            };
        }
    }
}
