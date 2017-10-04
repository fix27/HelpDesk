using System;

namespace HelpDesk.Entity
{
    public enum TypeWorkCalendarItem { Holiday, Work, Shortcut }

    /// <summary>
    /// Запись в рабочем календаре о выходном дне, рабочем дне, сокращенном рабочем дне
    /// </summary>
    public class WorkCalendarItem : BaseEntity
    {
        public TypeWorkCalendarItem TypeItem { get; set; }
        public DateTime Date { get; set; }
    }
}
