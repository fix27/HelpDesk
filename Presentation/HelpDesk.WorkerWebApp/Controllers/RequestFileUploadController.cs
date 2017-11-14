using HelpDesk.DataService.Interface;
using HelpDesk.Web.Common.Controllers;
using System.Net.Http;
using System.Web.Http;


namespace HelpDesk.WorkerWebApp.Controllers
{
    [RoutePrefix("api/RequestFileUpload")]
    [Authorize]
    public class RequestFileUploadController : BaseFileUploadController
    {
        public RequestFileUploadController(IRequestFileService requestFileService)
            :base(requestFileService)
        {
            baseUrl = "RequestFileUpload";
        }

        [Route("GetThumbnail")]
        [HttpGet]
        public override HttpResponseMessage GetThumbnail(long id)
        {
            return base.GetThumbnail(id);
        }

        [Route("Get")]
        [HttpGet]
        public override HttpResponseMessage Get(long id)
        {
            return base.Get(id);
        }

        [Route("Upload")]
        [HttpPost]
        public override IHttpActionResult Upload()
        {
            return base.Upload();
        }

        [Route("Upload")]
        [HttpGet]
        public override IHttpActionResult GetFileList(long forignKeyId = 0)
        {
            return base.GetFileList(forignKeyId);
        }

        [Route("Delete")]
        [HttpGet]
        public override IHttpActionResult DeleteFile(long id)
        {
            return base.DeleteFile(id);
        }

    }
}