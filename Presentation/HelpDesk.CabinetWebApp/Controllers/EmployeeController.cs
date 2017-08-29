using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using HelpDesk.DataService.Interface;
using Microsoft.AspNet.Identity;
using HelpDesk.Entity;
using HelpDesk.DTO;
using System.Collections;
using System.Linq;
using System;
using HelpDesk.CabinetWebApp.Models;

namespace HelpDesk.CabinetWebApp.Controllers
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
        public IHttpActionResult Get()
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                EmployeeDTO profile = employeeService.Get(userId);
                result = Json(new { success = true, data = profile });
            });
        }

        

        [Route("api/{lang}/Employee/Save")]
        [HttpPost]
        public IHttpActionResult Save(EmployeeDTO entity)
        {
            return execute(delegate ()
            {
                long userId = User.Identity.GetUserId<long>();
                entity.Id = userId;
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
        [ResponseType(typeof(IEnumerable<Organization>))]
        public IHttpActionResult GetListOrganization(string name)
        {
            return execute(delegate ()
            {
                IEnumerable<Organization> list = organizationService.GetList(name);
                result = Json(new { success = true, data = list });
            });
        }

        

        [Route("api/{lang}/Employee/GetOrganizationTree")]
        [HttpGet]
        [ResponseType(typeof(IList<jstree>))]
        public IEnumerable GetOrganizationTree(long? parentId)
        {
            IEnumerable<Organization> list = organizationService.GetList(parentId);
            IEnumerable items = list.Select(o => new jstree
            {
                id = o.Id.ToString(),
                parent = o.ParentId.HasValue ? o.ParentId.Value.ToString() : "#",
                text = String.Format("{0}, {1}", o.Name, o.Address),
                children = o.HasChild
            });
            return items;
        }

    }
}