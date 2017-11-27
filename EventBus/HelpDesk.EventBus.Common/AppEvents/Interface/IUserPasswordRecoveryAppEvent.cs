namespace HelpDesk.EventBus.Common.AppEvents.Interface
{
    /// <summary>
    /// Событие "Восстановление пароля"
    /// </summary>
    public interface IUserPasswordRecoveryAppEvent : IAppEvent
    {
        /// <summary>
        /// Email пользователя личного кабинета или Исполнителя 
        /// </summary>
        string Email { get; }

        /// <summary>
        /// Пароль пользователя личного кабинета или Исполнителя
        /// </summary>
        string Password { get; }
    }
}
