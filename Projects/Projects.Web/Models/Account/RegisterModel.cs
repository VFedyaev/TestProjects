﻿using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Account
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Электронная почта")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}