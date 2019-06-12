using Microsoft.AspNet.Identity;
using Projects.DAL.Entities;

namespace Projects.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }
    }
}
