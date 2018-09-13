using System;
using System.Collections.Generic;
using System.Web.Http;
using HelpDesk.DataService.Common;
using log4net;

namespace HelpDesk.WorkerWebApp.Controllers
{
    //Содержит метод для установки ModelState
    [Authorize]
    public class BaseApiController : ApiController
    {
        private readonly ILog log = LogManager.GetLogger("HelpDesk.WorkerWebApp");
        protected string errorGeneralMessage;
        protected string warningGeneralMessage;

        protected void setModelStateErrors(DataServiceException ex)
        {

            if (ex.DataServiceExceptionData != null && !String.IsNullOrWhiteSpace(ex.DataServiceExceptionData.GeneralMessage))
            {
                ModelState.AddModelError("ErrorGeneralMessage", ex.DataServiceExceptionData.GeneralMessage);
                errorGeneralMessage = ex.DataServiceExceptionData.GeneralMessage;
            }
            
            
            if (ex.DataServiceExceptionData != null && ex.DataServiceExceptionData.Messages != null)
            {
                foreach (string name in ex.DataServiceExceptionData.Messages.Keys)
                {
                    IList<string> list = ex.DataServiceExceptionData.Messages[name];
                    foreach (string msg in list)
                    {
                        ModelState.AddModelError(name, msg);
                    }
                }
            }

            if (ex.DataServiceExceptionData != null && ex.DataServiceExceptionData.IndexMessages != null)
            {
                foreach (int index in ex.DataServiceExceptionData.IndexMessages.Keys)
                {
                    Dictionary<string, List<string>> d = ex.DataServiceExceptionData.IndexMessages[index];
                    foreach (string name in d.Keys)
                    {
                        IList<string> list = d[name];
                        foreach (string msg in list)
                        {
                            ModelState.AddModelError(String.Format("{0}[{1}]", name, index), msg);
                        }
                    }
                }
            }
        }

        protected IHttpActionResult result;
        protected IHttpActionResult execute(Action setActionResult)
        {
            try
            {
                setActionResult();
                return result;
            }
            catch (DataServiceException ex)
            {
                setModelStateErrors(ex);
                IHttpActionResult r = Json(new
                {
                    success = false,
                    errors = ModelState
                });
                return r;
            }
            catch(Exception ex)
            {
                log.Error("Application error", ex);
                return this.BadRequest("Не обработанное исключение: " + 
                    String.Format("{0} --- {1}", ex.Message, ex.StackTrace));
            }
        }
    }
}