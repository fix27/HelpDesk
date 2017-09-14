using HelpDesk.Entity;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Организация
    /// </summary>
    public class OrganizationDTO: Organization
    {
        /// <summary>
        /// Узел может быть выбран (удовлетворяет условию включения в иерархию)
        /// </summary>
        public bool Selectable { get; set; }
    }
}
