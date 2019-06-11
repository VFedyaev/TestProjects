using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projects.DAL.Entities
{
    [Table("Project")]
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateStart { get; set; }
        public Nullable<DateTime> DateEnd { get; set; }
        public int Priority { get; set; }
        public string Comment { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
