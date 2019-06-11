using Projects.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface IEmployeeService : IService<EmployeeDTO>
    {
        IEnumerable<EmployeeDTO> GetListOrderedByName();
        EmployeeDTO Get(Guid? id);
        IEnumerable<EmployeeDTO> GetEmployeesBy(string type, string value);
    }
}
