using Projects.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projects.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        Task<string> GetUserRole(string userId);
        Task ChangeUserRole(ChangeRoleDTO changeRoleDTO);
        Task DeleteUser(string userId);
    }
}
