using Projects.BLL.DTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projects.Web.Models.Entities
{
    public class ProjectEmployeeVM
    {
        public Guid Id { get; set; }

        [Display(Name = "Проекты")]
        [Required(ErrorMessage = "Необходимо выбрать проект!")]
        public Guid ProjectId { get; set; }

        [Display(Name = "Сотрудники")]
        [Required(ErrorMessage = "Необходимо выбрать сотрудника!")]
        public Guid EmployeeId { get; set; }

        public ProjectDTO Project { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}