using Projects.BLL.DTO;
using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface ICustomerService : IService<CustomerDTO>
    {
        IEnumerable<CustomerDTO> GetListOrderedByName();
        CustomerDTO Get(Guid? id);
    }
}
