namespace HelpDesk.Entity
{
    /// <summary>
    /// Пользователь личного кабинета
    /// </summary>
    public class CabinetUser : BaseEntity
    {
        /// <summary>
        /// E-mail (он же логин)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Сотрудник, связанный с пользователем личного кабинета
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Подписан на E-mail рассылку
        /// </summary>
        public bool Subscribe { get; set; }
    }
}
