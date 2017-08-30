using System;

namespace HelpDesk.DTO.Parameters
{
    /// <summary>
    /// Параметр: Новое состояние заявки
    /// </summary>
    public class RequestEventParameter
    {
        /// <summary>
        /// Id заявки
        /// </summary>
        public long RequestId { get; set; }

        /// <summary>
        /// Id нового состояния
        /// </summary>
        public long StatusRequestId { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Новая дата окончания срока выполнения работ по заявке
        /// </summary>
        public DateTime? NewDeadLineDate { get; set; }

    }
}
