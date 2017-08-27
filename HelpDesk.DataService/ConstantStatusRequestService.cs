using HelpDesk.DataService.Interface;
using HelpDesk.DTO;


namespace HelpDesk.DataService
{
    public class ConstantStatusRequestService: IConstantStatusRequestService
    {
        public long[] IgnoredRawRequestStates { get  { return Constants.IgnoredRawRequestStates;  }  }
        public long DateEndStatusRequest { get { return Constants.DateEndStatusRequest; } }

        public long NewStatusRequest { get { return Constants.NewStatusRequest; } }
        public long ConfirmationStatusRequest { get { return Constants.ConfirmationStatusRequest; } }
        
    }
}
