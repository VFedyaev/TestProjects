using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.DAL.Entities;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public EmployeeDTO Get(Guid id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);

            return Mapper.Map<EmployeeDTO>(employee);
        }

        public EmployeeDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            Employee employee = _unitOfWork.Employees.Get(id);
            if (employee == null)
                throw new NotFoundException();

            return Mapper.Map<EmployeeDTO>(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employee = _unitOfWork.Employees.GetAll().ToList();

            return Mapper.Map<IEnumerable<EmployeeDTO>>(employee);
        }

        public IEnumerable<EmployeeDTO> GetListOrderedByName()
        {
            List<Employee> employee = _unitOfWork.Employees.GetAll().OrderBy(n => n.FullName).ToList();

            return Mapper.Map<IEnumerable<EmployeeDTO>>(employee);
        }

        public void Add(EmployeeDTO employeeDTO)
        {
            Employee employee = Mapper.Map<Employee>(employeeDTO);
            employee.Id = Guid.NewGuid();

            _unitOfWork.Employees.Create(employee);
            _unitOfWork.Save();
        }

        public void Update(EmployeeDTO employeeDTO)
        {
            Employee employee = Mapper.Map<Employee>(employeeDTO);

            _unitOfWork.Employees.Update(employee);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Employee employee = _unitOfWork.Employees.Get(id);
            if (employee == null)
                throw new NotFoundException();

            _unitOfWork.Employees.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<EmployeeDTO> GetEmployeesBy(string type, string value)
        {
            IEnumerable<EmployeeDTO> Employees = Enumerable.Empty<EmployeeDTO>();
            switch (type)
            {
                case "model":
                    Employees = GetEmployeesByModel(value);
                    break;
            }

            return Employees;
        }
        private IEnumerable<EmployeeDTO> GetEmployeesByModel(string value)
        {
            IEnumerable<Employee> Employees = _unitOfWork
                .Employees
                .Find(c => c.FullName.ToLower().Contains(value));

            if (Employees.Count() <= 0)
                return Enumerable.Empty<EmployeeDTO>();

            return Mapper.Map<IEnumerable<EmployeeDTO>>(Employees);
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.ProjectEmployees.Find(h => h.EmployeeId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
