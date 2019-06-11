using Projects.DAL.EF;
using Projects.DAL.Entities;
using Projects.DAL.Interfaces;
using System;

namespace Projects.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProjectContext _projectContext;

        private Repository<ExecutorCompany> _executorCompanyRepository;
        private Repository<Position> _positionRepository;
        private Repository<Employee> _employeeRepository;
        private Repository<Customer> _customerRepository;
        private Repository<Project> _projectRepository;
        private Repository<ProjectEmployee> _projectEmployeeRepository;

        public UnitOfWork(string connectionString)
        {
            _projectContext = new ProjectContext(connectionString);
        }

        public IRepository<ExecutorCompany> ExecutorCompanies
        {
            get
            {
                if (_executorCompanyRepository == null)
                    _executorCompanyRepository = new Repository<ExecutorCompany>(_projectContext);
                return _executorCompanyRepository;
            }
        }

        public IRepository<Position> Positions
        {
            get
            {
                if (_positionRepository == null)
                    _positionRepository = new Repository<Position>(_projectContext);
                return _positionRepository;
            }
        }

        public IRepository<Employee> Employees
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository = new Repository<Employee>(_projectContext);
                return _employeeRepository;
            }
        }

        public IRepository<Customer> Customers
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new Repository<Customer>(_projectContext);
                return _customerRepository;
            }
        }

        public IRepository<Project> Projects
        {
            get
            {
                if (_projectRepository == null)
                    _projectRepository = new Repository<Project>(_projectContext);
                return _projectRepository;
            }
        }

        public IRepository<ProjectEmployee> ProjectEmployees
        {
            get
            {
                if (_projectEmployeeRepository == null)
                    _projectEmployeeRepository = new Repository<ProjectEmployee>(_projectContext);
                return _projectEmployeeRepository;
            }
        }

        public void Save()
        {
            _projectContext.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _projectContext.Dispose();
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
