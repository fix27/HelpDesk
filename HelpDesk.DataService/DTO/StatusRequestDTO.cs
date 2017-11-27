using HelpDesk.DataService.Common.DTO;

namespace HelpDesk.DataService.DTO
{
    /// <summary>
    /// Состояние заявки
    /// </summary>
    public class StatusRequestDTO : IChecked<StatusRequestEnum>
    {
        public StatusRequestEnum Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True - Архивное состояние, False - активное состояние. Используется при фильтрации заявок
        /// </summary>
        public bool? Archive { get; set; }

        #region IChecked
        public bool Checked { get; set; }

        public StatusRequestEnum Value { get { return Id; } }
        #endregion IChecked

        public override bool Equals(object obj)
        {
            StatusRequestDTO obj2 = obj as StatusRequestDTO;
            if (obj2 == null) return false;
            return Id == obj2.Id;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }
    }
}
