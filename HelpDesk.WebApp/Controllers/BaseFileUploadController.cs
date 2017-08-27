using HelpDesk.DataService.Common;
using HelpDesk.DataService.Interface;
using HelpDesk.DTO.FileUpload.Interface;
using HelpDesk.WebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;


namespace HelpDesk.WebApp.Controllers
{
    /// <summary>
    /// Для возможности использовать разные сервисы для работы с загружаемыми файлами
    /// </summary>
    public class BaseFileUploadController : ApiController
    {
    
        private readonly string deleteUrlTempl      = "/api/{0}/Delete?id={1}";
        private readonly string thumbnailUrlTempl   = "/api/{0}/GetThumbnail?id={1}";
        private readonly string fileUrlTempl        = "/api/{0}/Get?id={1}";
        private readonly string deleteType      = "GET";

        private readonly IFileUploadService fileUploadService;
        
        protected string baseUrl = null;
        public BaseFileUploadController(IFileUploadService fileUploadService)
        {
            baseUrl = "FileUpload";
            this.fileUploadService = fileUploadService;
        }

        #region public virtual
        public virtual HttpResponseMessage GetThumbnail(long id)
        {
            var file = fileUploadService.GetFileThumbnail(id);
            if (file == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);


            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(file.Thumbnail)
                 
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = file.Name,
                    Size = file.Size
                };
            result.Content.Headers.ContentType =  new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        
        public virtual HttpResponseMessage Get(long id)
        {
            var file = fileUploadService.GetFile(id);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(file.Body)
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = file.Name,
                    Size = file.Size
                };
            result.Content.Headers.ContentType =  new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        
        public virtual IHttpActionResult Upload()
        {
            try
            {
                var resultList = new List<ViewDataUploadFilesResult>();

                var currentContext = HttpContext.Current;
                string tempRequestKey = currentContext.Request.Params["tempRequestKey"];
                string forignKeyId = currentContext.Request.Params["forignKeyId"];
                
                uploadAndShowResults(currentContext, new Guid(tempRequestKey), 
                    !String.IsNullOrWhiteSpace(forignKeyId)? Int64.Parse(forignKeyId): (long?)null, resultList);
                JsonFiles files = new JsonFiles(resultList);

                return Json(files);
            }
            catch (DataServiceException ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, 
                    ex.DataServiceExceptionData.GeneralMessage));
            }
        }

        
        public virtual IHttpActionResult GetFileList(long forignKeyId = 0)
        {
            var list = getFileList(forignKeyId);
            return Json(list);
        }

       
        public virtual IHttpActionResult DeleteFile(long id)
        {
            fileUploadService.DeleteFile(id);
            return Ok();
        }

        #endregion public virtual

        #region private
        private JsonFiles getFileList(long forignKeyId)
        {

            var r = new List<ViewDataUploadFilesResult>();
            IEnumerable<IFileUploadInfoDTO> list = fileUploadService.GetListFileInfo(forignKeyId);
            

            foreach (IFileUploadInfoDTO file in list)
            {
                r.Add(uploadResult(file.Id, file.Name, file.Size, file.Type));
            }
            JsonFiles files = new JsonFiles(r);

            return files;
        }

        private ViewDataUploadFilesResult uploadResult(long id, String fileName, int fileSize, string fileType)
        {
            var result = new ViewDataUploadFilesResult()
            {
                name        = fileName,
                size        = fileSize,
                type        = fileType,
                url         = String.Format(fileUrlTempl, baseUrl, id),
                deleteUrl       = String.Format(deleteUrlTempl, baseUrl, id),
                thumbnailUrl    = String.Format(thumbnailUrlTempl, baseUrl, id),
                deleteType  = deleteType,
            };
            return result;
        }

        public void uploadAndShowResults(HttpContext ContentBase, Guid tempRequestKey, long? forignKeyId, IList<ViewDataUploadFilesResult> resultList)
        {
            var httpRequest = ContentBase.Request;
            

            foreach (String inputTagName in httpRequest.Files)
            {
                var headers = httpRequest.Headers;
                var file = httpRequest.Files[inputTagName];
                
                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    uploadWholeFile(ContentBase, resultList, tempRequestKey, forignKeyId);
                //else
                //    uploadPartialFile(headers["X-File-Name"], ContentBase, resultList, tempRequestKey);
            }
        }


        private void uploadWholeFile(HttpContext requestContext, IList<ViewDataUploadFilesResult> statuses, Guid tempRequestKey, long? forignKeyId)
        {
            
            var request = requestContext.Request;
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                IFileUploadDTO f = fileUploadService.GetNew();

                f.Name = file.FileName;
                f.Size = file.ContentLength;
                f.TempRequestKey = tempRequestKey;
                f.Type = file.ContentType;
                f.ForignKeyId = forignKeyId;
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    f.Body = binaryReader.ReadBytes(file.ContentLength);
                }
                f.Thumbnail = new WebImage(f.Body).Resize(80, 80).GetBytes();
                
                long id =fileUploadService.SaveFile(f);

                statuses.Add(uploadResult(id, f.Name, f.Size, f.Type));
            }
        }



        //private void uploadPartialFile(string fileName, HttpContext requestContext, IList<ViewDataUploadFilesResult> statuses, Guid tempRequestKey)
        //{
        //    var request = requestContext.Request;
        //    if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
        //    var file = request.Files[0];
        //    var inputStream = file.InputStream;
        //    String patchOnServer = Path.Combine(StorageRoot);
        //    var fullName = Path.Combine(patchOnServer, Path.GetFileName(file.FileName));
        //    var ThumbfullPath = Path.Combine(fullName, Path.GetFileName(file.FileName + "80x80.jpg"));
        //    ImageHandler handler = new ImageHandler();

        //    var ImageBit = ImageHandler.LoadImage(fullName);
        //    handler.Save(ImageBit, 80, 80, 10, ThumbfullPath);
        //    using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
        //    {
        //        var buffer = new byte[1024];

        //        var l = inputStream.Read(buffer, 0, 1024);
        //        while (l > 0)
        //        {
        //            fs.Write(buffer, 0, l);
        //            l = inputStream.Read(buffer, 0, 1024);
        //        }
        //        fs.Flush();
        //        fs.Close();
        //    }
        //    statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName));
        //}
        #endregion private
    }
}