using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projects.DAL.Entities
{
    [Table("Position")]
    public class Position
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
