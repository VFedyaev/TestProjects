using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projects.DAL.EF;
using Projects.DAL.Entities;
using Projects.DAL.Identity;
using Projects.DAL.Interfaces;
using System;
using System.Threading.Tasks;

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

        PasswordValidator passwordValidator = new PasswordValidator
        {
            RequiredLength = 8,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true
        };

        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        public UnitOfWork(string connectionString)
        {
            _projectContext = new ProjectContext(connectionString);

            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_projectContext));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_projectContext));

            UserManager.PasswordValidator = passwordValidator;
            UserManager.UserValidator = new AppUserValidator(UserManager);
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

        public async Task SaveAsync()
        {
            await _projectContext.SaveChangesAsync();
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
