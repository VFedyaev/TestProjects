using Projects.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ExecutorCompany> ExecutorCompanies { get; }
        IRepository<Position> Positions { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Project> Projects { get; }
        IRepository<ProjectEmployee> ProjectEmployees { get; }

        void Save();
    }
}
