using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HelpDesk.WebApp.Identity;
using HelpDesk.WebApp.Models;
using HelpDesk.DataService.Interface;
using HelpDesk.Common;
using HelpDesk.DataService.Common;
using System.Collections.Generic;
using HelpDesk.WebApp.Filters;
using HelpDesk.WebApp.Resources;
using HelpDesk.DTO;
using System.Net.Mail;
using System;
using System.Net.Configuration;
using System.Configuration;
using log4net;

namespace HelpDesk.WebApp.Controllers
{
    [Authorize]
    [Culture]
    public class AccountController : Controller
    {
        private readonly ILog log = LogManager.GetLogger("HelpDesk.WebApp");
        private readonly IUserService userService;
        private readonly IDateTimeService dateTimeService;

        public AccountController(IUserService userService, IDateTimeService dateTimeService)
        {
            this.userService = userService;
            this.dateTimeService = dateTimeService;
        }
                
        private ApplicationSignInManager signInManager;
        
        
        public ApplicationSignInManager SignInManager
        {
            get
            {
                ApplicationSignInManager m = signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                return m;
            }
            private set 
            { 
                signInManager = value; 
            }
        }
        
        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            
            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = userService.GetDTO(model.UserName);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resource.Message_LoginError);
                    return View(model);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, bool CaptchaValid)
        {
            if (!CaptchaValid)
            {
                //Captcha failed to validate
                ModelState.AddModelError("reCaptcha", Resource.InvalidReCaptcha);
            }

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                userService.Create(model.Email, model.Password);
            }
            catch (DataServiceException ex)
            {
                addErrors(ex);
                return View(model);
            }


            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = userService.GetDTO(model.Email);
                    
                    Task task = new Task(() => sendEmail(model.Email,
                        String.Format(Resource.Text_RegisterNow, Resource.AppName),
                        String.Format("{0}: {1}, {2}: {3}",
                        Resource.Label_Email, model.Email, Resource.Label_Password, model.Password)));
                    task.Start();

                    return RedirectToAction("Index", "AngularTemplate");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resource.Message_Error);
                    return View(model);
            }
            
            
        }


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session[AppConstants.CURRENT_APPLICATION_USER_SESSION_KEY] = null;
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoveryPassword()
        {
            return View(new RecoveryPassvordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RecoveryPassword(RecoveryPassvordViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO user = userService.GetDTO(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", Resource.Message_EmailNotRegitered);
                }
                else
                {
                    Task task = new Task(() => sendEmail(model.Email,
                        String.Format(Resource.Text_RecoveryPasswordSubject, Resource.AppName),
                        user.Password));
                    task.Start();

                    model.IsSend = true;
                }

            }

            return View(model);
        }


        protected void sendEmail(string email, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                smtpClient.Send(new MailMessage(smtpSection.From, email, subject, body));
            }
            catch(Exception ex)
            {
                log.Error("Application error", ex);
            }
            
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                if (signInManager != null)
                {
                    signInManager.Dispose();
                    signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region private

        private void addErrors(DataServiceException ex)
        {
            foreach (string key in ex.DataServiceExceptionData.Messages.Keys)
            {
                IList<string> messages = ex.DataServiceExceptionData.Messages[key];
                foreach(string message in messages)
                    ModelState.AddModelError(key, message);
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "AngularTemplate");
        }

        #endregion
    }
}