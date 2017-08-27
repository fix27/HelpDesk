namespace HelpDesk.Entity
{
    /// <summary>
    /// Личные данные пользователя
    /// </summary>
    public class PersonalProfile : BaseEntity
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

        /// <summary>
        /// Подписан на E-mail рассылку
        /// </summary>
        public bool Subscribe { get; set; }

        public User User { get; set; }

    }
}
