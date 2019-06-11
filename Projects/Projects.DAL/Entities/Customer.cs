using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projects.DAL.Entities
{
    [Table("Customer")]
    public class Customer
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}
