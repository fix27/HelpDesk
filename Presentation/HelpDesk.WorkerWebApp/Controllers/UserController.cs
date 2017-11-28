using System.Collections.Generic;
using System.Web.Http;
using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using Microsoft.AspNet.Identity;
using HelpDesk.WorkerWebApp.Models;

namespace HelpDesk.WorkerWebApp.Controllers
{

    public class UserController : BaseApiController
    {
        private readonly IWorkerUserService userService;
        
        public UserController(
            IWorkerUserService userService)
        {
            this.userService = userService;
        }
        

        [Route("api/{lang}/User/Get")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                WorkerUser user = userService.Get(userId);
                result = Json(new { success = true, data = user });
            });
        }

        
        [Route("api/{lang}/User/GetListSubscribeStatus")]
        [HttpGet]
        public IHttpActionResult GetListSubscribeStatus()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<RawStatusRequestDTO> list = userService.GetListSubscribeStatus(userId);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/User/ChangeSubscribeRequestState")]
        [HttpPost]
        public IHttpActionResult ChangeSubscribeRequestState(ChangeSubscribeRequestStateParameter param)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                userService.ChangeSubscribeRequestState(userId, param.RequestStateId);
                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/User/ChangeSubscribe")]
        [HttpPost]
        public IHttpActionResult ChangeSubscribe()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                userService.ChangeSubscribe(userId);
                result = Json(new { success = true });
            });
        }

    }
}