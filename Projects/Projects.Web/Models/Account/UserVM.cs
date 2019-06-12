using System;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Account
{
    public class UserVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Display(Name = "Почта")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}