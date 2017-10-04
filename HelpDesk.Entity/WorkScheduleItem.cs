using System;

namespace HelpDesk.Entity
{
    /// <summary>
    /// Запись графика работы
    /// </summary>
    public class WorkScheduleItem : BaseEntity
    {
        public DayOfWeek DayOfWeek { get; set; }
        
        /// <summary>
        /// Час начала рабочего дня
        /// </summary>
        public int StartWorkDay { get; set; }

        /// <summary>
        /// Час окончания рабочего дня
        /// </summary>
        public int EndWorkDay { get; set; }

        /// <summary>
        /// Час начала обеденного перерыва
        /// </summary>
        public int? StartLunchBreak { get; set; }

        /// <summary>
        /// Час окончания обеденного перерыва
        /// </summary>
        public int? EndLunchBreak { get; set; }
    }
}
