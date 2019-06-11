using Projects.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface IPositionService : IService<PositionDTO>
    {
        IEnumerable<PositionDTO> GetListOrderedByName();
        PositionDTO Get(Guid? id);
    }
}
