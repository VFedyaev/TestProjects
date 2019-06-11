using System;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Entities
{
    public class CustomerVM
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "ФИО")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я ]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string FullName { get; set; }

        [Display(Name = "Наименование компании")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string CompanyName { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        [StringLength(256, ErrorMessage = "Длина строки не должна превышать 256 символов")]
        public string Email { get; set; }

        [Display(Name = "Факс")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        public string Fax { get; set; }
    }
}