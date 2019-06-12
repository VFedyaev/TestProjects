using AutoMapper;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using Projects.BLL.Infrastructure;
using Projects.BLL.MappingProfiles;
using Projects.Web.MappingProfiles;
using Projects.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Projects.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();

            NinjectModule webModule = new WebModule();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection");
            var kernel = new StandardKernel(webModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        public class AutoMapperConfiguration
        {
            public static void Configure()
            {
                Mapper.Initialize(x =>
                {
                    x.AllowNullCollections = true;
                    x.AddProfile<BLLMappingProfile>();
                    x.AddProfile<WebMappingProfile>();
                });

                Mapper.Configuration.AssertConfigurationIsValid();
            }
        }
    }
}
