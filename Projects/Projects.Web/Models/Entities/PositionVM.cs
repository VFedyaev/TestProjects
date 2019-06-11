using Projects.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Entities
{
    public class PositionVM
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Должность")]
        [StringLength(50, ErrorMessage = "Длина строки не должна превышать 50 символов")]
        [RegularExpression(@"^[a-zA-ZЁёӨөҮүҢңА-Яа-я - # +]+$", ErrorMessage = "Ввод цифр запрещен")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string Name { get; set; }

        public ICollection<EmployeeDTO> Employees { get; set; }
    }
}