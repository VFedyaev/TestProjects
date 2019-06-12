using Microsoft.AspNet.Identity.EntityFramework;
using Projects.DAL.Entities;
using System.Data.Entity;

namespace Projects.DAL.EF
{
    public class ProjectContext : IdentityDbContext<ApplicationUser>
    {
        DbSet<ExecutorCompany> ExecutorCompanies { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        public ProjectContext() : base("DefaultConnection") { }

        public ProjectContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<ProjectContext>(null);
        }
    }
}
