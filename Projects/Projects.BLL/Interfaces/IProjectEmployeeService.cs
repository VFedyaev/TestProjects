using Projects.BLL.DTO;
using System;

namespace Projects.BLL.Interfaces
{
    public interface IProjectEmployeeService : IService<ProjectEmployeeDTO>
    {
        void Create(Guid projectId, Guid employeeId);
        void UpdateProjectRelations(Guid? projectId, string[] employeeIds);
        void DeleteRelationsByProjectId(Guid id);
    }
}
