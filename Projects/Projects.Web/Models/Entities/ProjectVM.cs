using Projects.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Entities
{
    public class ProjectVM
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Наименование проекта")]
        [StringLength(150, ErrorMessage = "Длина строки не должна превышать 150 символов")]
        [Required(ErrorMessage = "Заполните поле!")]
        public string Name { get; set; }

        [Display(Name = "Заказчик")]
        [Required(ErrorMessage = "Необходимо выбрать заказчика!")]
        public Guid CustomerId { get; set; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Выберите дату!")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateEnd { get; set; }

        [Display(Name = "Приоритет")]
        [Required(ErrorMessage = "Необходимо выставить приоритет проекта!")]
        public int Priority { get; set; }

        [Display(Name = "Комментарий")]
        [StringLength(300, ErrorMessage = "Длина строки не должна превышать 300 символов")]
        public string Comment { get; set; }

        public CustomerDTO Customer { get; set; }

        public ICollection<ProjectEmployeeDTO> ProjectEmployees { get; set; }
    }
}