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
        public IHttpActionResult Get(long employeeId)
        {
            return execute(delegate ()
            {
                EmployeeDTO employee = employeeService.Get(employeeId);
                result = Json(new { success = true, data = employee });
            });
        }

        

        [Route("api/{lang}/Employee/Save")]
        [HttpPost]
        public IHttpActionResult Save(EmployeeDTO entity)
        {
            return execute(delegate ()
            {
                employeeService.Save(entity);
                entity = employeeService.Get(entity.Id);
                result = Json(new { employee = entity, success = true });
            });
        }

        [Route("api/{lang}/Employee/GetListPost")]
        [HttpGet]
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
        public IHttpActionResult GetListOrganization(string name)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<OrganizationDTO> list = organizationService.GetListByWorkerUser(userId, name)
                    .Where(o => o.Selectable);
                result = Json(new { success = true, data = list });
            });
        }

        [Route("api/{lang}/Employee/GetListEmployee")]
        [HttpGet]
        public IHttpActionResult GetListEmployee(string name)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                IEnumerable<EmployeeDTO> list = employeeService.GetListByWorkerUser(userId, name);
                result = Json(new { success = true, data = list });
            });
        }
        

        [Route("api/{lang}/Employee/GetExistsOrganization")]
        [HttpGet]
        public IHttpActionResult GetExistsOrganization()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                bool exists = organizationService.GetExistsByWorkerUser(userId);
                result = Json(new { success = true, data = exists });
            });
        }

        [Route("api/{lang}/Employee/GetOrganizationTree")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<jstree_node>))]
        public IEnumerable GetOrganizationTree(long? parentId)
        {
            long userId = User.Identity.GetUserId<long>();
            IEnumerable<OrganizationDTO> list = organizationService.GetListByWorkerUser(userId, parentId);
            IEnumerable items = list.Select(o => new jstree_node
            {
                id = o.Id.ToString(),
                parent = o.ParentId.HasValue ? o.ParentId.Value.ToString() : "#",
                text = String.Format("{0}, {1}", o.Name, o.Address),
                children = o.HasChild,
                selectable = o.Selectable
            });
            return items;
        }

        [Route("api/{lang}/Employee/GetEmployeeTree")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<jstree_all_loaded_node>))]
        public IEnumerable GetEmployeeTree(/*string parentId*/)
        {
            long userId = User.Identity.GetUserId<long>();
            string orgPrefix = "A";

            //long? parentIdlong = parentId != "#" ? Int64.Parse(parentId.Substring(1)) : (long?)null;
            IEnumerable<OrganizationDTO> list = organizationService.GetListByWorkerUser(userId/*, parentIdlong*/);
            IEnumerable<jstree_all_loaded_node> items = list.Select(o => new jstree_all_loaded_node
            {
                id = orgPrefix + o.Id.ToString(),
                parent = o.ParentId.HasValue ? orgPrefix + o.ParentId.Value.ToString() : "#",
                text = String.Format("{0}, {1}", o.Name, o.Address),
                //children = true,
                type = "organization",
                selectable = o.Selectable
            });

            //if (parentIdlong.HasValue)
            //{
            //    IEnumerable<EmployeeDTO> employees = employeeService.GetListByOrganization(parentIdlong.Value, userId);
            //    if (employees != null)
            //    {
            //        IEnumerable<jstree> employeeItems = employees.Select(e => new jstree
            //        {
            //            id = e.Id.ToString(),
            //            parent = parentId,
            //            text = e.ShortEmployeeInfo,
            //            children = false,
            //            type = "employee",
            //            selectable = true
            //        });

            //        items = items.Union(employeeItems);
            //    }
            //}

            if (list != null && list.Any())
            {
                IEnumerable<EmployeeDTO> employees = employeeService.GetListByOrganization(
                    list.Select(o => o.Id), userId);
                if (employees != null)
                {
                    IEnumerable<jstree_all_loaded_node> employeeItems = employees.Select(e => new jstree_all_loaded_node
                    {
                        id = e.Id.ToString(),
                        parent = orgPrefix + e.OrganizationId,
                        text = e.ShortEmployeeInfo,
                        //children = false,
                        type = "employee",
                        selectable = true
                    });

                    items = items.Union(employeeItems);
                }
            }

            

            return items;
        }

    }
}