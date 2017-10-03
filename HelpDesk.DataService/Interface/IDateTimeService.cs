using HelpDesk.DataService.DTO;
using System;
using System.Collections.Generic;

namespace HelpDesk.DataService.Interface
{
    public interface IDateTimeService
    {
        /// <summary>
        /// Текущая дата/время (чтобы, в частности, при тестировании не привязываться к системному времени)
        /// </summary>
        DateTime GetCurrent();

        IList<Month> GetListMonth();

        DateTime GetRequestDateEnd(DateTime currentDateTime, int countHour, int? startWorkDay, int? endWorkDay, int? startLunchBreak, int? endLunchBreak);
    }
}
