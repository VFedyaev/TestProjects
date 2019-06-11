using Projects.BLL.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Entities
{
    public class EmployeeVM
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "ФИО")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я ]+$", ErrorMessage = "Ввод цифр запрещен")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string FullName { get; set; }

        [Display(Name = "Исполняющая компания")]
        [Required(ErrorMessage = "Необходимо выбрать исполняющую компанию!")]
        public Guid ExecutorCompanyId { get; set; }

        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Необходимо выбрать должность!")]
        public Guid PositionId { get; set; }

        [Display(Name = "Телефон")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный E-mail")]
        [StringLength(256, ErrorMessage = "Длина строки не должна превышать 256 символов")]
        public string Email { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Выберите дату!")]
        public DateTime DateBorn { get; set; }

        public ExecutorCompanyDTO ExecutorCompany { get; set; }
        public PositionDTO Position { get; set; }
    }
}