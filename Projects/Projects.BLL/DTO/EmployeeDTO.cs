using System;

namespace Projects.BLL.DTO
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Guid ExecutorCompanyId { get; set; }
        public Guid PositionId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateBorn { get; set; }

        public ExecutorCompanyDTO ExecutorCompany { get; set; }
        public PositionDTO Position { get; set; }
    }
}
