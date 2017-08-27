using HelpDesk.DataService.Interface;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HelpDesk.Common;
using HelpDesk.DTO;
using HelpDesk.WebApp.Filters;
using System.Collections.Generic;
using HelpDesk.WebApp.Resources;
using System.Threading;
using HelpDesk.WebApp.Models;
using HelpDesk.Entity;

namespace HelpDesk.WebApp.Controllers
{
    //Layout of the angular application.
    [Authorize]
    
    public class AngularTemplateController : Controller
    {
        private readonly IUserService userService;
        private readonly IPersonalProfileService personalProfileService;
        private readonly IRequestProfileService requestProfileService;
        private readonly ISettingsService settingsService;

        public AngularTemplateController(IUserService userService,
            IPersonalProfileService personalProfileService,
            IRequestProfileService requestProfileService,
            ISettingsService settingsService)
        {
            this.userService = userService;
            this.personalProfileService = personalProfileService;
            this.requestProfileService = requestProfileService;
            this.settingsService = settingsService;
        }

        

        public UserDTO CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] == null)
                {
                    UserDTO currentUser = userService.GetDTO(User.Identity.GetUserId<long>());
                    Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = currentUser;
                }

                return (UserDTO)Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY];
            }
        }

        #region layout
        
        [HttpGet]
        [Culture]
        public ActionResult Index()
        {
            ViewBag.Menu = new Dictionary<string, string>()
            {
                { "request", Resource.Menu_Request },
                { "requestHistory", Resource.Menu_RequestHistory },
                { "requestProfile", Resource.Menu_RequestProfile },
                { "personalProfile", Resource.Menu_PersonalProfile }
            };

            Settings settings = settingsService.Get();

            return View("~/App/Main/views/layout/layout.cshtml",
                new AppLayoutModel()
                {
                     ManualUrl = settings.ManualUrl,
                     Message = settings.Message,
                     ServiceLevelAgreementUrl = settings.ServiceLevelAgreementUrl,
                     TechSupportPhones = settings.TechSupportPhones
                }); 
        }

        [HttpGet]
        [Culture]
        public ActionResult Header()
        {
            return PartialView("~/App/Main/views/layout/header.cshtml"); 
        }

        #endregion layout



        #region Request
        [HttpGet]
        public ActionResult NewRequest()
        {
            long userId = User.Identity.GetUserId<long>();
            
            return PartialView("~/App/Main/views/request/request.cshtml", new RequestModel()
            {
                AllowableForSendRequest = requestProfileService.AllowableForSendRequest(userId),
                PersonalProfileComplete = personalProfileService.IsComplete(userId)
            });
        }
        #endregion Request

        #region RequestHistory
        [HttpGet]
        public ActionResult RequestHistory()
        {
            return PartialView("~/App/Main/views/requestHistory/list.cshtml");
        }

        [HttpGet]
        public ActionResult RequestEvents()
        {
            return PartialView("~/App/Main/views/requestHistory/events.cshtml");
        }
        #endregion RequestHistory

        #region PersonalProfile
        [HttpGet]
        public ActionResult PersonalProfile()
        {
            return PartialView("~/App/Main/views/personalProfile/profile.cshtml");
        }
        #endregion PersonalProfile

        #region RequestProfile
        [HttpGet]
        public ActionResult RequestProfile()
        {
            return PartialView("~/App/Main/views/requestProfile/list.cshtml");
        }

        [HttpGet]
        public ActionResult RequestProfileAddIS()
        {
            return PartialView("~/App/Main/views/requestProfile/addIS.cshtml");
        }

        [HttpGet]
        public ActionResult RequestProfileAddTO()
        {
            return PartialView("~/App/Main/views/requestProfile/addTO.cshtml");
        }
        #endregion RequestProfile

        #region справочники
        public ActionResult Organizations()
        {
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return PartialView("~/App/Main/views/organization/list.cshtml");
        }

        public ActionResult PersonalObjects()
        {
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return PartialView("~/App/Main/views/object/list.cshtml");
        }
        #endregion справочники
    }
}