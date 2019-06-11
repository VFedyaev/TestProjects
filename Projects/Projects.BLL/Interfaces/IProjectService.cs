using Projects.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface IProjectService : IService<ProjectDTO>
    {
        IEnumerable<ProjectDTO> GetListOrderedByName();
        ProjectDTO Get(Guid? id);

        IEnumerable<EmployeeDTO> GetEmployees(Guid? id);
    }
}
