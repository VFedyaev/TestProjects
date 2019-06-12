using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.User
{
    public class ChangeRoleModel
    {
        public string UserId { get; set; }
        public string OldRole { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}