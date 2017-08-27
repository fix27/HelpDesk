using HelpDesk.DataService.Interface;
using HelpDesk.DTO;
using System.Collections.Generic;


namespace HelpDesk.DataService
{
    public class StatusRequestMapService: IStatusRequestMapService
    {
        public StatusRequestEnum GetEquivalenceByElement(long statusRequestId)
        {
            return StatusRequestFactorization.GetEquivalenceByElement(statusRequestId);
        }

        public IEnumerable<long> GetElementsByEquivalence(StatusRequestEnum statusRequest)
        {
            return StatusRequestFactorization.GetElementsByEquivalence(statusRequest);
        }
    }
}
