using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projects.DAL.Entities
{
    [Table("Employee")]
    public class Employee
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Guid ExecutorCompanyId { get; set; }
        public Guid PositionId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateBorn { get; set; }

        public virtual ExecutorCompany ExecutorCompamy { get; set; }
        public virtual Position Position { get; set; }
    }
}
