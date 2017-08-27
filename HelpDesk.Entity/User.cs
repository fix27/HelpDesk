namespace HelpDesk.Entity
{
    /// <summary>
    /// Пользователь системы. Также используется в Identity
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// E-mail (он же логин)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        public PersonalProfile PersonalProfile { get; set; }

    }
}
