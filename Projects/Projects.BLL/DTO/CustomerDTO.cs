using System;

namespace Projects.BLL.DTO
{
    public class CustomerDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
    }
}
