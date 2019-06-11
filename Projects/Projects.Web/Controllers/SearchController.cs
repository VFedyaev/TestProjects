using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Interfaces;
using Projects.Web.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class SearchController : Controller
    {
        private ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public ActionResult SearchMethod(string title, string type)
        {
            ModelAndViewDTO result = _searchService.GetFilteredModelAndView(title, type);

            if (result.Model.Count() > 0)
            {
                string modelType = result.Model.First().GetType().ToString().Split('.').Last();
                switch (modelType)
                {
                    case "PositionDTO":
                        result.Model = Mapper.Map<IEnumerable<PositionVM>>(result.Model);
                        break;
                    case "ExecutorCompanyDTO":
                        result.Model = Mapper.Map<IEnumerable<ExecutorCompanyVM>>(result.Model);
                        break;
                    case "EmployeeDTO":
                        result.Model = Mapper.Map<IEnumerable<EmployeeVM>>(result.Model);
                        break;
                    case "CustomerDTO":
                        result.Model = Mapper.Map<IEnumerable<CustomerVM>>(result.Model);
                        break;
                    case "ProjectDTO":
                        result.Model = Mapper.Map<IEnumerable<ProjectVM>>(result.Model);
                        break;
                }
            }
            else
                result.View = "NotFound";

            return PartialView(result.View, result.Model);
        }

        public ActionResult NotFoundResult()
        {
            return PartialView("~/Views/Error/NotFoundError.cshtml");
        }
    }
}