using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Interfaces;
using Projects.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Projects.BLL.Services
{
    public class SearchService : ISearchService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public SearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ModelAndViewDTO GetFilteredModelAndView(string inputTitle, string type)
        {
            ModelAndViewDTO result = new ModelAndViewDTO
            {
                Model = Enumerable.Empty<object>(),
                View = "NotFound"
            };

            string title = inputTitle.Trim();
            if (title.Length <= 0)
                return result;

            string[] words = title.ToLower().Split(' ');
            switch (type)
            {
                case "position":
                    result = GetPositionFilteredListAndView(words);
                    break;
                case "executorCompany":
                    result = GetExecutorCompanyFilteredListAndView(words);
                    break;
                case "employee":
                    result = GetEmployeeFilteredListAndView(words);
                    break;
                case "customer":
                    result = GetCustomerFilteredListAndView(words);
                    break;
                case "project":
                    result = GetProjectFilteredListAndView(words);
                    break;
            }

            return result;
        }

        private ModelAndViewDTO GetPositionFilteredListAndView(string[] words)
        {
            var positionList = _unitOfWork.Positions.GetAll().Where(e => words.All(e.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<PositionDTO>>(positionList),
                View = "Positions"
            };
        }

        private ModelAndViewDTO GetExecutorCompanyFilteredListAndView(string[] words)
        {
            var executorCompanyList = _unitOfWork.ExecutorCompanies.GetAll().Where(e => words.All(e.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<ExecutorCompanyDTO>>(executorCompanyList),
                View = "ExecutorCompanies"
            };
        }

        private ModelAndViewDTO GetEmployeeFilteredListAndView(string[] words)
        {
            var employeeList = _unitOfWork.Employees.GetAll().Where(e => words.All(e.FullName.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<EmployeeDTO>>(employeeList),
                View = "Employees"
            };
        }

        private ModelAndViewDTO GetCustomerFilteredListAndView(string[] words)
        {
            var customerList = _unitOfWork.Customers.GetAll().Where(e => words.All(e.FullName.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<CustomerDTO>>(customerList),
                View = "Customers"
            };
        }

        private ModelAndViewDTO GetProjectFilteredListAndView(string[] words)
        {
            var projectList = _unitOfWork.Projects.GetAll().Where(e => words.All(e.Name.ToLower().Contains)).ToList();

            return new ModelAndViewDTO
            {
                Model = Mapper.Map<IEnumerable<ProjectDTO>>(projectList),
                View = "Projects"
            };
        }
    }
}
