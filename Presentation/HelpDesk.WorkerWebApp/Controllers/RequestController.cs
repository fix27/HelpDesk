using System.Web.Http;
using System.Web.Http.Description;
using HelpDesk.DataService.Interface;
using HelpDesk.DTO;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using HelpDesk.DataService.Filters;
using HelpDesk.Common;
using System.Linq;
using HelpDesk.Entity;
using HelpDesk.DTO.Parameters;
using HelpDesk.WorkerWebApp.Models;
using System;

namespace HelpDesk.WorkerWebApp.Controllers
{

    public class RequestController : BaseApiController
    {
        private readonly IRequestService requestService;
        private readonly IDateTimeService dateTimeService;
        public RequestController(IRequestService requestService,
            IDateTimeService dateTimeService)
        {
            this.requestService = requestService;
            this.dateTimeService = dateTimeService;
        }

        [Route("api/{lang}/Request/GetNewByObjectId")]
        [HttpGet]
        [ResponseType(typeof(RequestParameter))]
        public IHttpActionResult GetNewByObjectId(long objectId)
        {
            return execute(delegate()
            {
                RequestParameter obj = requestService.GetNewByObjectId(objectId);
                result = Json(new { success = true, data = obj });
            });
        }

        [Route("api/{lang}/Request/GetNewByRequestId")]
        [HttpGet]
        [ResponseType(typeof(RequestParameter))]
        public IHttpActionResult GetNewByRequestId(long requestId)
        {
            return execute(delegate ()
            {
                RequestParameter obj = requestService.GetNewByRequestId(requestId);
                result = Json(new { success = true, data = obj });
            });
        }

        [Route("api/{lang}/Request/Get")]
        [HttpGet]
        [ResponseType(typeof(RequestParameter))]
        public IHttpActionResult Get(long requestId = 0)
        {
            return execute(delegate ()
            {
                RequestParameter obj = requestService.Get(requestId);
                result = Json(new { success = true, data = obj });
            });
        }   

        [Route("api/{lang}/Request/GetList")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<RequestDTO>))]
        public IHttpActionResult GetList([FromUri]RequestFilter filter, [FromUri]OrderInfo orderInfo, [FromUri]PageInfo pageInfo)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<RequestDTO> list = requestService.GetList(userId, filter, orderInfo, ref pageInfo);
                result = Json(new { success = true, data = list, totalCount = pageInfo.TotalCount, count = pageInfo.Count });
            });
        }

        [Route("api/{lang}/Request/GetRequestFilter")]
        [HttpGet]
        [ResponseType(typeof(RequestFilter))]
        public IHttpActionResult GetRequestFilter()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();

                
                RequestFilter filter = new RequestFilter()
                {
                    ArchiveMonths = dateTimeService.GetListMonth(),
                    ArchiveYears = requestService.GetListArchiveYear(userId).ToList()
                };

                filter.ArchiveYears.Insert(0, new Year() { Ord = 0, Name = "Все" });
                filter.ArchiveMonths.Insert(0, new Month() { Ord = 0, Name = "Все" });

                result = Json(new { success = true, data = filter });
            });
        }

        [Route("api/{lang}/Request/GetListStatus")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<StatusRequest>))]
        public IHttpActionResult GetListStatus(bool archive)
        {
            return execute(delegate ()
            {
                IEnumerable<StatusRequest> list = requestService.GetListRawStatus(archive);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/Request/Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            return execute(delegate ()
            {
                requestService.Delete(id);
                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/Request/GetCountRequiresConfirmation")]
        [HttpGet]
        [ResponseType(typeof(int))]
        public IHttpActionResult GetCountRequiresConfirmation()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                int count = requestService.GetCountRequiresConfirmation(userId);
                result = Json(new { success = true, data = count });
            });
        }

        [Route("api/{lang}/Request/GetListRequestEvent")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<RequestEventDTO>))]
        public IHttpActionResult GetListRequestEvent(long requestId)
        {
            return execute(delegate ()
            {
                IEnumerable<RequestEventDTO> list = requestService.GetListRequestEvent(requestId);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/Request/Save")]
        [HttpPost]
        public IHttpActionResult Save(RequestParameter dto)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                dto.UserId = userId;
                long requestId = requestService.Save(dto);
                result = Json(new { success = true, requestId = requestId });
            });
        }

        [Route("api/{lang}/Request/CreateRequestEvent")]
        [HttpPost]
        public IHttpActionResult CreateRequestEvent(RequestEventParameterModel param)
        {
            RequestEventParameter dto = new RequestEventParameter
            {
                 Note = param.Note,
                 RequestId = param.RequestId,
                 StatusRequestId = param.StatusRequestId,
                 NewDeadLineDate = String.IsNullOrEmpty(param.NewDeadLineDate) ? (DateTime?)null: Convert.ToDateTime(param.NewDeadLineDate)
            };

            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                requestService.CreateRequestEvent(userId, dto);
                result = Json(new { success = true });
            });
        }
    }
}