using Ninject.Modules;
using Projects.BLL.Interfaces;
using Projects.BLL.Services;

namespace Projects.Web.Util
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IExecutorCompanyService>().To<ExecutorCompanyService>();
            Bind<IPositionService>().To<PositionService>();
            Bind<IEmployeeService>().To<EmployeeService>();
            Bind<ICustomerService>().To<CustomerService>();
            Bind<IProjectService>().To<ProjectService>();
            Bind<IProjectEmployeeService>().To<ProjectEmployeeService>();

            Bind<ISearchService>().To<SearchService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IUserService>().To<UserService>();
        }
    }
}