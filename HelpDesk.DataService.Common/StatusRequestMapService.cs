using HelpDesk.DataService.Common.Interface;
using HelpDesk.DataService.Common.DTO;
using System.Collections.Generic;


namespace HelpDesk.DataService.Common
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
