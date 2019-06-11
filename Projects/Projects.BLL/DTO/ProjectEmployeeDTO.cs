using System;

namespace Projects.BLL.DTO
{
    public class ProjectEmployeeDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid EmployeeId { get; set; }

        public ProjectDTO Project { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
