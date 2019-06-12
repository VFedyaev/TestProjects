using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Account
{
    public class ChangeEmailModel
    {
        [Required]
        [Display(Name = "Почта")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string Email { get; set; }
    }
}