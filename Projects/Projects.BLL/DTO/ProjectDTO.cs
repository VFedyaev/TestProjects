using System;

namespace Projects.BLL.DTO
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DateStart { get; set; }
        public Nullable<DateTime> DateEnd { get; set; }
        public int Priority { get; set; }
        public string Comment { get; set; }

        public CustomerDTO Customer { get; set; }
    }
}
