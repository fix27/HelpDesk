using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using HelpDesk.DataService.Interface;
using Microsoft.AspNet.Identity;
using HelpDesk.Entity;
using HelpDesk.DataService.DTO;
using System.Collections;
using System.Linq;
using System;
using HelpDesk.WorkerWebApp.Models;

namespace HelpDesk.WorkerWebApp.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly IObjectService objectService;
        private readonly IOrganizationService organizationService;
        private readonly IEmployeeService employeeService;
        private readonly IPostService postService;
        
        public EmployeeController(
            IObjectService objectService,
            IEmployeeService employeeService,
            IPostService postService,
            IOrganizationService organizationService)
        {
            this.objectService = objectService;
            this.employeeService = employeeService;
            this.postService = postService;
            this.organizationService = organizationService;
        }
        

        [Route("api/{lang}/Employee/Get")]
        [HttpGet]
        [ResponseType(typeof(EmployeeDTO))]
        public IHttpActionResult Get(long employeeId)
        {
            return execute(delegate ()
            {
                EmployeeDTO profile = employeeService.Get(employeeId);
                result = Json(new { success = true, data = profile });
            });
        }

        

        [Route("api/{lang}/Employee/Save")]
        [HttpPost]
        public IHttpActionResult Save(EmployeeDTO entity)
        {
            return execute(delegate ()
            {
                employeeService.Save(entity);
                result = Json(new { success = true });
            });
        }

        [Route("api/{lang}/Employee/GetListPost")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Post>))]
        public IHttpActionResult GetListPost(string name)
        {
            return execute(delegate ()
            {
                IEnumerable<Post> list = postService.GetList(name);
                result = Json(new { success = true, data = list});
            });
        }

        [Route("api/{lang}/Employee/GetListOrganization")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<OrganizationDTO>))]
        public IHttpActionResult GetListOrganization(string name)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<OrganizationDTO> list = organizationService.GetListByWorkerUser(userId, name);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/Employee/GetListEmployee")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EmployeeDTO>))]
        public IHttpActionResult GetListEmployee(string name)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<EmployeeDTO> list = employeeService.GetListByWorkerUser(userId, name);
                result = Json(new { success = true, data = list });
            });
        }

        

        [Route("api/{lang}/Employee/GetOrganizationTree")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<jstree>))]
        public IEnumerable GetOrganizationTree(long? parentId)
        {
            long userId = User.Identity.GetUserId<long>();
            IEnumerable<OrganizationDTO> list = organizationService.GetListByWorkerUser(userId, parentId);
            IEnumerable items = list.Select(o => new jstree
            {
                id = o.Id.ToString(),
                parent = o.ParentId.HasValue ? o.ParentId.Value.ToString() : "#",
                text = String.Format("{0}, {1}", o.Name, o.Address),
                children = o.HasChild
            });
            return items;
        }

        [Route("api/{lang}/Employee/GetEmployeeTree")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<jstree>))]
        public IEnumerable GetEmployeeTree(string parentId)
        {
            long userId = User.Identity.GetUserId<long>();
            string orgPrefix = "A";

            long? parentIdlong = parentId != "#" ? Int64.Parse(parentId.Substring(1)) : (long?)null;
            IEnumerable<Organization> list = organizationService.GetListByWorkerUser(userId, parentIdlong);
            IEnumerable<jstree> items = list.Select(o => new jstree
            {
                id = orgPrefix + o.Id.ToString(),
                parent = o.ParentId.HasValue ? orgPrefix + o.ParentId.Value.ToString() : "#",
                text = String.Format("{0}, {1}", o.Name, o.Address),
                children = true,
                type = "organization"
            });

            if (parentIdlong.HasValue)
            {
                IEnumerable<EmployeeDTO> employees = employeeService.GetListByOrganization(parentIdlong.Value);
                IEnumerable<jstree> employeeItems = employees.Select(e => new jstree
                {
                    id = e.Id.ToString(),
                    parent = parentId,
                    text = e.ShortEmployeeInfo,
                    children = false,
                    type = "employee"
                });

                items = items.Union(employeeItems);
            }
            
            return items;
        }

    }
}