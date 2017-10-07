using HelpDesk.DataService.DTO;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IDateTimeService
    {
        /// <summary>
        /// Текущая дата/время (чтобы, в частности, при тестировании не привязываться к системному времени),
        /// и другие вычисления, связанные с датами, в части заявок
        /// </summary>
        DateTime GetCurrent();

        IList<Month> GetListMonth();

        DateTime GetRequestDateEnd(DateTime currentDateTime, int countHour);
    }
}
