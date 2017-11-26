namespace HelpDesk.Common.EventBus.AppEvents.Interface
{
    /// <summary>
    /// Событие "Регистрация в личном кабинете" 
    /// </summary>
    public interface IUserRegisterAppEvent: IAppEvent
    {
        /// <summary>
        /// Email пользователя личного кабинета
        /// </summary>
        string Email { get; }
    }
}
