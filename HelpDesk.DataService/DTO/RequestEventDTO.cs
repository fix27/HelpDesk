using HelpDesk.Common.Helpers;
using HelpDesk.Entity;
using System;


namespace HelpDesk.DataService.DTO
{
    /// <summary>
    /// Cобытие заявки
    /// </summary>
    public class RequestEventDTO
    {
        public long RequestId { get; set; }
        public DateTime DateEvent { get; set; }
        public string Note { get; set; }

        /// <summary>
        /// Признак события переноса срока
        /// </summary>
        public bool Transfer { get; set; }

        /// <summary>
        /// RAW-состояние
        /// </summary>
        public StatusRequest Status { get; set; }

        /// <summary>
        /// Факторизованное состояние
        /// </summary>
        public StatusRequestEnum StatusRequest { get; set; }
        public string StatusName
        {
            get
            {
                return StatusRequest.GetDisplayName();
            }
        }

        /// <summary>
        /// Признак даты окончания
        /// </summary>
        public bool DateEnd { get; set; }

        public int OrdGroup { get; set; }

        public WorkerUser User { get; set; }

    }
}
