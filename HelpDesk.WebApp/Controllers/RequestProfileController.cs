using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using HelpDesk.DataService.Interface;
using Microsoft.AspNet.Identity;
using HelpDesk.DTO;
using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.WebApp.Models;
using System.Collections;

namespace HelpDesk.WebApp.Controllers
{

    public class RequestProfileController : BaseApiController
    {
        private readonly IObjectService objectService;
        private readonly IRequestProfileService requestProfileService;
                
        public RequestProfileController(
            IObjectService objectService,
            IRequestProfileService requestProfileService)
        {
            this.objectService = objectService;
            this.requestProfileService = requestProfileService;
           
        }

        [Route("api/{lang}/RequestProfile/GetNewRequestObjectIS")]
        [HttpGet]
        [ResponseType(typeof(RequestObjectISDTO))]
        public IHttpActionResult GetNewRequestObjectIS()
        {
            return execute(delegate ()
            {
                RequestObjectISDTO obj = new RequestObjectISDTO();
                result = Json(new { success = true, data = obj });
            });
        }

        [Route("api/{lang}/RequestProfile/GetNewRequestObjectTO")]
        [HttpGet]
        [ResponseType(typeof(RequestObjectTODTO))]
        public IHttpActionResult GetNewRequestObjectTO()
        {
            return execute(delegate ()
            {
                RequestObjectTODTO obj = new RequestObjectTODTO();
                result = Json(new { success = true, data = obj });
            });
        }

        [Route("api/{lang}/RequestProfile/GetListPersonalObject")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<PersonalProfileObjectDTO>))]
        public IHttpActionResult GetListPersonalObject([FromUri]PersonalObjectFilter filter, [FromUri]OrderInfo orderInfo, [FromUri]PageInfo pageInfo)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<PersonalProfileObjectDTO> list = requestProfileService.GetListPersonalObject(userId, filter, orderInfo, pageInfo);
                result = Json(new { success = true, data = list, totalCount = pageInfo.TotalCount, count = pageInfo.Count });
            });
        }

        [Route("api/{lang}/RequestProfile/GetPersonalObjectTree")]
        [HttpGet]
        [ResponseType(typeof(IList<jstree>))]
        public IEnumerable GetPersonalObjectTree(long? parentId)
        {
            long userId = User.Identity.GetUserId<long>();
            IEnumerable<PersonalProfileObjectDTO> list = requestProfileService.GetListPersonalObject(userId);
            List<jstree> items = new List<jstree>();

            if (!parentId.HasValue)
            {
                items.Add(new jstree
                {
                    id = "-1",
                    parent = "#",
                    text = "ПО",
                    children = true
                });
                items.Add(new jstree
                {
                    id = "-2",
                    parent = "#",
                    text = "Оборудование",
                    children = true
                });

                return items;
            }

            if (parentId == -1)
            {
                foreach (PersonalProfileObjectDTO o in list)
                {
                    if (o.Soft)
                        items.Add(new jstree
                        {
                            id = o.ObjectId.ToString(),
                            parent = "-1",
                            text = o.ObjectName,
                            children = false
                        });
                }

                return items;
            }

            if (parentId == -2)
            {
                foreach (PersonalProfileObjectDTO o in list)
                {
                    if (!o.Soft)
                        items.Add(new jstree
                        {
                            id = o.ObjectId.ToString(),
                            parent = "-2",
                            text = o.ObjectName,
                            children = false
                        });
                }

                return items;
            }

            return items;
        }

        [Route("api/{lang}/RequestProfile/GetListPersonalObjectByName")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<PersonalProfileObjectDTO>))]
        public IHttpActionResult GetListPersonalObjectByName(string objectName = null)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<PersonalProfileObjectDTO> list = requestProfileService.GetListPersonalObject(userId, objectName);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/RequestProfile/GetListAllowableObjectIS")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<RequestObjectISDTO>))]
        public IHttpActionResult GetListAllowableObjectIS(string name = null)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<RequestObjectISDTO> list = requestProfileService.GetListAllowableObjectIS(userId, name);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/RequestProfile/GetListAllowableObjectType")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SimpleDTO>))]
        public IHttpActionResult GetListAllowableObjectType()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<SimpleDTO> list = requestProfileService.GetListAllowableObjectType(userId);
                result = Json(new { success = true, data = list });
            });
        }
        

        [Route("api/{lang}/RequestProfile/GetListWare")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<WareDTO>))]
        public IHttpActionResult GetListWare()
        {
            return execute(delegate ()
            {
                IEnumerable<WareDTO> list = objectService.GetListWare();
                result = Json(new { success = true, data = list });
            });
        }

        
        [Route("api/{lang}/RequestProfile/GetListHardType")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SimpleDTO>))]
        public IHttpActionResult GetListHardType(string name = null)
        {
            return execute(delegate ()
            {
                IEnumerable<SimpleDTO> list = objectService.GetListHardType(name);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/RequestProfile/GetListModel")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SimpleDTO>))]
        public IHttpActionResult GetListModel(long manufacturerId, string name = null)
        {
            return execute(delegate ()
            {
                IEnumerable<SimpleDTO> list = objectService.GetListModel(manufacturerId, name);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/RequestProfile/GetListManufacturer")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SimpleDTO>))]
        public IHttpActionResult GetListManufacturer(string name = null)
        {
            return execute(delegate ()
            {
                IEnumerable<SimpleDTO> list = objectService.GetListManufacturer(name);
                result = Json(new { success = true, data = list });
            });
        }
        
        [Route("api/{lang}/RequestProfile/AddIS")]
        [HttpPost]
        public IHttpActionResult AddIS(RequestObjectISDTO dto)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                requestProfileService.AddIS(userId, dto);
                
                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/RequestProfile/AddTO")]
        [HttpPost]
        public IHttpActionResult AddTO(RequestObjectTODTO dto)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                requestProfileService.AddTO(userId, dto);

                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/RequestProfile/Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                requestProfileService.Delete(userId, id);
                result = Json(new { success = true });
            });
        }
    }
}