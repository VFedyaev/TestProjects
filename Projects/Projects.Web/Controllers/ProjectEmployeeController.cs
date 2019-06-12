using Projects.BLL.Interfaces;
using System;
using System.Net;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class ProjectEmployeeController : Controller
    {
        private IProjectEmployeeService _projectEmployeeService;

        public ProjectEmployeeController(IProjectEmployeeService projectEmployeeService)
        {
            _projectEmployeeService = projectEmployeeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager, user")]
        public ActionResult UpdateEmployees(Guid? projectId)
        {
            string[] employeeIds = Request.Form.GetValues("employeeId[]") ?? new string[0];
            try
            {
                _projectEmployeeService.UpdateProjectRelations(projectId, employeeIds);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch
            {
                _projectEmployeeService.DeleteRelationsByProjectId((Guid)projectId);
            }

            return RedirectToRoute(new
            {
                controller = "Project",
                action = "Index"
            });
        }
    }
}