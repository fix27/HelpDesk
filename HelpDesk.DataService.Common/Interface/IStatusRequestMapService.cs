using HelpDesk.DataService.Common.DTO;
using System.Collections.Generic;


namespace HelpDesk.DataService.Common.Interface
{
    public interface IStatusRequestMapService
    {
        StatusRequestEnum GetEquivalenceByElement(long statusRequestId);

        IEnumerable<long> GetElementsByEquivalence(StatusRequestEnum statusRequest);
    }
}
