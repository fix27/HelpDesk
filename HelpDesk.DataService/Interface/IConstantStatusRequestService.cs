namespace HelpDesk.DataService.Interface
{
    public interface IConstantStatusRequestService
    {
        long[] IgnoredRawRequestStates { get; }
        long DateEndStatusRequest { get; }

        long NewStatusRequest { get; }
        long ConfirmationStatusRequest{ get; }
               
                
    }
}
