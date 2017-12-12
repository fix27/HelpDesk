using System.Collections.Generic;
using System.Web.Http;
using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using Microsoft.AspNet.Identity;
using HelpDesk.CabinetWebApp.Models;

namespace HelpDesk.CabinetWebApp.Controllers
{

    public class UserController : BaseApiController
    {
        private readonly ICabinetUserService userService;
        
        public UserController(
            ICabinetUserService userService)
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
                CabinetUser user = userService.Get(userId);
                if (user.Employee != null)
                    user.Employee.User = null;
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
                IEnumerable<StatusRequestDTO> list = userService.GetListSubscribeStatus(userId);
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
                userService.ChangeSubscribeRequestState(userId, param.RequestState);
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