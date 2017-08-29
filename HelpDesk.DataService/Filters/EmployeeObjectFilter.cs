using System.Collections.Generic;

namespace HelpDesk.DataService.Filters
{
    /// <summary>
    /// Для фильтрации в гриде "Профиль заявителя"
    /// </summary>
    public class EmployeeObjectFilter
    {
        public string ObjectName { get; set; }
        public IEnumerable<bool> Wares { get; set; }

    }
}
