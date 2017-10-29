using HelpDesk.DataService.Interface;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HelpDesk.Common;
using HelpDesk.DataService.DTO;
using HelpDesk.WorkerWebApp.Filters;
using System.Collections.Generic;
using HelpDesk.WorkerWebApp.Resources;
using System.Threading;
using HelpDesk.WorkerWebApp.Models;
using HelpDesk.Entity;

namespace HelpDesk.WorkerWebApp.Controllers
{
    //Layout of the angular application.
    [Authorize]
    
    public class AngularTemplateController : Controller
    {
        private readonly IWorkerUserService userService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeObjectService employeeObjectService;
        private readonly ISettingsService settingsService;

        public AngularTemplateController(IWorkerUserService userService,
            IEmployeeService employeeService,
            IEmployeeObjectService employeeObjectService,
            ISettingsService settingsService)
        {
            this.userService = userService;
            this.employeeService = employeeService;
            this.employeeObjectService = employeeObjectService;
            this.settingsService = settingsService;
        }

        

        public WorkerUserDTO CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return null;

                if (Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] == null)
                {
                    WorkerUserDTO currentUser = userService.GetDTO(User.Identity.GetUserId<long>());
                    Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = currentUser;
                }

                return (WorkerUserDTO)Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY];
            }
        }

        #region layout
        
        [HttpGet]
        [Culture]
        public ActionResult Index()
        {
            if (CurrentUser.UserType.Id == 1)
            {
                ViewBag.Menu = new Dictionary<string, string>()
                {
                    { "requestHistory", Resource.Menu_RequestHistory }
                };
            }
            else
            {
                ViewBag.Menu = new Dictionary<string, string>()
                {
                    { "request", Resource.Menu_Request },
                    { "requestHistory", Resource.Menu_RequestHistory }
                };
            }
            

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
            return PartialView("~/App/Main/views/request/request.cshtml");
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

        [HttpGet]
        public ActionResult RequestEvent(long statusRequestId)
        {
            if (statusRequestId == (long)RawStatusRequestEnum.ExtendedDeadLine)
                return PartialView("~/App/Main/views/requestHistory/eventDeadLine.cshtml");

            return PartialView("~/App/Main/views/requestHistory/event.cshtml");
        }
        #endregion RequestHistory

        #region Employee
        [HttpGet]
        public ActionResult Employee()
        {
            return PartialView("~/App/Main/views/employee/employee.cshtml");
        }
        #endregion Employee

        #region EmployeeObject
        [HttpGet]
        public ActionResult EmployeeObjectAddIS()
        {
            return PartialView("~/App/Main/views/request/addIS.cshtml");
        }

        [HttpGet]
        public ActionResult EmployeeObjectAddTO()
        {
            return PartialView("~/App/Main/views/request/addTO.cshtml");
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

        public ActionResult EmployeeTree()
        {
            ViewBag.CurrentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            return PartialView("~/App/Main/views/employee/list.cshtml");
        }
        #endregion справочники
    }
}