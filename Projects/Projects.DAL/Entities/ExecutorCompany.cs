using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projects.DAL.Entities
{
    [Table("ExecutorCompany")]
    public class ExecutorCompany
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
    }
}
