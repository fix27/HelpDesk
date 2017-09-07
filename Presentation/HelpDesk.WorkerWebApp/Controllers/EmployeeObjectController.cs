using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using HelpDesk.DataService.Interface;
using Microsoft.AspNet.Identity;
using HelpDesk.DTO;
using HelpDesk.Common;
using HelpDesk.DataService.Filters;
using HelpDesk.WorkerWebApp.Models;
using System.Collections;

namespace HelpDesk.WorkerWebApp.Controllers
{

    public class EmployeeObjectController : BaseApiController
    {
        private readonly IObjectService objectService;
        private readonly IEmployeeObjectService employeeObjectService;
                
        public EmployeeObjectController(
            IObjectService objectService,
            IEmployeeObjectService employeeObjectService)
        {
            this.objectService = objectService;
            this.employeeObjectService = employeeObjectService;
           
        }

        [Route("api/{lang}/EmployeeObject/GetNewRequestObjectIS")]
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

        [Route("api/{lang}/EmployeeObject/GetNewRequestObjectTO")]
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

        [Route("api/{lang}/EmployeeObject/GetListEmployeeObject")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EmployeeObjectDTO>))]
        public IHttpActionResult GetListEmployeeObject([FromUri]EmployeeObjectFilter filter, [FromUri]OrderInfo orderInfo, [FromUri]PageInfo pageInfo)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<EmployeeObjectDTO> list = employeeObjectService.GetListEmployeeObject(userId, filter, orderInfo, ref pageInfo);
                result = Json(new { success = true, data = list, totalCount = pageInfo.TotalCount, count = pageInfo.Count });
            });
        }

        [Route("api/{lang}/EmployeeObject/GetEmployeeObjectTree")]
        [HttpGet]
        [ResponseType(typeof(IList<jstree>))]
        public IEnumerable GetEmployeeObjectTree(long employeeId, long? parentId)
        {
            IEnumerable<EmployeeObjectDTO> list = employeeObjectService.GetListEmployeeObject(employeeId);
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
                foreach (EmployeeObjectDTO o in list)
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
                foreach (EmployeeObjectDTO o in list)
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

        [Route("api/{lang}/EmployeeObject/GetListEmployeeObjectByName")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EmployeeObjectDTO>))]
        public IHttpActionResult GetListEmployeeObjectByName(long employeeId, string objectName = null)
        {
            return execute(delegate ()
            {
                IEnumerable<EmployeeObjectDTO> list = employeeObjectService.GetListEmployeeObject(employeeId, objectName);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/EmployeeObject/GetListAllowableObjectIS")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<RequestObjectISDTO>))]
        public IHttpActionResult GetListAllowableObjectIS(long employeeId, string name = null)
        {
            return execute(delegate ()
            {
                IEnumerable<RequestObjectISDTO> list = employeeObjectService.GetListAllowableObjectIS(employeeId, name);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/EmployeeObject/GetListAllowableObjectType")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SimpleDTO>))]
        public IHttpActionResult GetListAllowableObjectType(long employeeId)
        {
            return execute(delegate ()
            {
                IEnumerable<SimpleDTO> list = employeeObjectService.GetListAllowableObjectType(employeeId);
                result = Json(new { success = true, data = list });
            });
        }
        

        [Route("api/{lang}/EmployeeObject/GetListWare")]
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

        
        [Route("api/{lang}/EmployeeObject/GetListHardType")]
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

        [Route("api/{lang}/EmployeeObject/GetListModel")]
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

        [Route("api/{lang}/EmployeeObject/GetListManufacturer")]
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
        
        [Route("api/{lang}/EmployeeObject/AddIS")]
        [HttpPost]
        public IHttpActionResult AddIS(RequestObjectISDTO dto)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                employeeObjectService.AddIS(userId, dto);
                
                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/EmployeeObject/AddTO")]
        [HttpPost]
        public IHttpActionResult AddTO(RequestObjectTODTO dto)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                employeeObjectService.AddTO(userId, dto);

                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/EmployeeObject/Delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(long id)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                employeeObjectService.Delete(userId, id);
                result = Json(new { success = true });
            });
        }
    }
}