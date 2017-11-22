namespace HelpDesk.Common.EventBus.AppEvents.Interface
{
    public interface IUserPasswordRecoveryAppEvent : IAppEvent
    {
        string Email { get; }
        string Password { get; }
    }
}
