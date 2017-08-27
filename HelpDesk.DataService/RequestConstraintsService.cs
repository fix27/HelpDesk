using HelpDesk.DataService.Interface;
using HelpDesk.Data.Repository;
using HelpDesk.Entity;
using HelpDesk.DataService.Resources;
using HelpDesk.DataService.Common;

namespace HelpDesk.DataService
{
    /// <summary>
    /// Для проверки различных состояний. Если проверка проходит, выбрасывается DataServiceException
    /// </summary>
    public class RequestConstraintsService : IRequestConstraintsService
    {
        
        private readonly IBaseRepository<Request> requestRepository;
        private readonly IBaseRepository<RequestArch> requestArchRepository;
        private readonly IBaseRepository<RequestEvent> requestEventRepository;
        
        
        public RequestConstraintsService(
            IBaseRepository<Request> requestRepository,
            IBaseRepository<RequestArch> requestArchRepository,
            IBaseRepository<RequestEvent> requestEventRepository
            )
        {
            this.requestRepository = requestRepository;
            this.requestArchRepository = requestArchRepository;
            this.requestEventRepository = requestEventRepository;

        }

        

        public void CheckExistsRequest(long requestId)
        {
            //запрещено изменение/удаление принятых в работу заявок
            int requestEventCout = requestEventRepository.Count(t => t.RequestId == requestId);
            if (requestEventCout > 2)
                throw new DataServiceException(Resource.RequestAcceptConstraintMsg);

            //запрещено изменение/удаление архивных заявок
            BaseRequest request = requestRepository.Get(requestId);
            if (request == null)
            {
                request = requestArchRepository.Get(requestId);
                if(request != null)
                    throw new DataServiceException(Resource.RequesArchConstraintMsg);
                else
                    throw new DataServiceException(Resource.NoDataFoundMsg);
            }
            
        }
        
    }
}
