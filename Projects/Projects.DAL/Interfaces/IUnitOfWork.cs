using Projects.DAL.Entities;
using Projects.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace Projects.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }

        IRepository<ExecutorCompany> ExecutorCompanies { get; }
        IRepository<Position> Positions { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Project> Projects { get; }
        IRepository<ProjectEmployee> ProjectEmployees { get; }

        void Save();
        Task SaveAsync();
    }
}
