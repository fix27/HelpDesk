using HelpDesk.DataService.Interface;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HelpDesk.Common;
using HelpDesk.DataService.DTO;
using HelpDesk.CabinetWebApp.Filters;
using System.Collections.Generic;
using HelpDesk.CabinetWebApp.Resources;
using System.Threading;
using HelpDesk.CabinetWebApp.Models;
using HelpDesk.Entity;

namespace HelpDesk.CabinetWebApp.Controllers
{
    //Layout of the angular application.
    [Authorize]
    
    public class AngularTemplateController : Controller
    {
        private readonly ICabinetUserService userService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeObjectService employeeObjectService;
        private readonly ISettingsService settingsService;

        public AngularTemplateController(ICabinetUserService userService,
            IEmployeeService employeeService,
            IEmployeeObjectService employeeObjectService,
            ISettingsService settingsService)
        {
            this.userService = userService;
            this.employeeService = employeeService;
            this.employeeObjectService = employeeObjectService;
            this.settingsService = settingsService;
        }

        

        public CabinetUserDTO CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] == null)
                {
                    CabinetUserDTO currentUser = userService.GetDTO(User.Identity.GetUserId<long>());
                    Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = currentUser;
                }

                return (CabinetUserDTO)Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY];
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
                { "employeeObject", Resource.Menu_EmployeeObject },
                { "employee", Resource.Menu_Employee },
                { "subscribe", Resource.Menu_Subscribe }
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
                AllowableForSendRequest = employeeObjectService.AllowableForSendRequest(userId),
                EmployeeComplete = employeeService.IsComplete(userId)
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

        #region Subscribe
        [HttpGet]
        public ActionResult Subscribe()
        {
            return PartialView("~/App/Main/views/subscribe/subscribe.cshtml");
        }

        #endregion Subscribe

        #region Employee
        [HttpGet]
        public ActionResult Employee()
        {
            return PartialView("~/App/Main/views/employee/profile.cshtml");
        }
        #endregion Employee

        #region EmployeeObject
        [HttpGet]
        public ActionResult EmployeeObjects()
        {
            return PartialView("~/App/Main/views/employeeObject/list.cshtml");
        }

        [HttpGet]
        public ActionResult EmployeeObjectAddIS()
        {
            return PartialView("~/App/Main/views/employeeObject/addIS.cshtml");
        }

        [HttpGet]
        public ActionResult EmployeeObjectAddTO()
        {
            return PartialView("~/App/Main/views/employeeObject/addTO.cshtml");
        }
        #endregion EmployeeObject

        #region справочники
        public ActionResult OrganizationTree()
        {
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return PartialView("~/App/Main/views/organization/list.cshtml");
        }

        public ActionResult EmployeeObjectTree()
        {
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return PartialView("~/App/Main/views/object/list.cshtml");
        }
        #endregion справочники
    }
}