using Projects.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface IExecutorCompanyService : IService<ExecutorCompanyDTO>
    {
        IEnumerable<ExecutorCompanyDTO> GetListOrderedByName();
        ExecutorCompanyDTO Get(Guid? id);
    }
}
