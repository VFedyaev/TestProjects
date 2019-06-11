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
        IRepository<ExecutorCompany> ExecutorCompanies { get; set; }
        IRepository<Position> Positions { get; set; }
        IRepository<Employee> Employees { get; set; }
        IRepository<Customer> Customers { get; set; }
        IRepository<Project> Projects { get; set; }
        IRepository<ProjectEmployee> ProjectEmployees { get; set; }

        void Save();
    }
}
