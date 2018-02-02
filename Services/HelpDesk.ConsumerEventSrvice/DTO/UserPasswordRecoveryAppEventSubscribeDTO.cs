namespace HelpDesk.ConsumerEventService.DTO
{
    public class UserPasswordRecoveryAppEventSubscribeDTO: BaseUserEventSubscribeDTO
    {
        public string Password { get; set; }
    }
}
