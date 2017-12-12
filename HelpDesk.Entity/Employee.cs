namespace HelpDesk.Entity
{
    /// <summary>
    /// Сотрудник обслуживаемой организации. 
    /// Если запись создается из личного кабинета, то Employee.Id = CabinetUser.Id  
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public string FM { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string IM { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string OT { get; set; }

        
        /// <summary>
        /// Должность
        /// </summary>
        public virtual Post Post { get; set; }

        /// <summary>
        /// Организация
        /// </summary>
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string Cabinet { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        
        public CabinetUser User { get; set; }

    }
}
