using HelpDesk.DataService.DTO;
using System.Collections.Generic;


namespace HelpDesk.DataService.Interface
{
    public interface IStatusRequestMapService
    {
        StatusRequestEnum GetEquivalenceByElement(long statusRequestId);

        IEnumerable<long> GetElementsByEquivalence(StatusRequestEnum statusRequest);
    }
}
