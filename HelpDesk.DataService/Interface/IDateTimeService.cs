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


    }
}
