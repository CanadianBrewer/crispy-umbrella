using Autofac;
using Autofac.Integration.Mvc;
using DatabaseConfigurations.Services;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Autofac.DependencyInjection;
using Serilog.Sinks.MSSqlServer;
using SerilogWeb.Classic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DatabaseConfigurations
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register application dependencies.
            builder.RegisterType<ConfigurationCacheService>().As<IConfigurationCacheService>().SingleInstance();
            builder.RegisterType<CacheHandler>().As<ICacheHandler>();
            ConfigureSerilog(builder);
            Log.Information("Serilog configured and registered");

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void ConfigureSerilog(ContainerBuilder builder)
        {
            // We need the db password here in order to configure Serilog before we complete Application_Start.
            // We could use an environment variable from the server (that's modern) or we instantiate the cache.
            // But to instantiate the cache, we need the container, but we need to register Serilog via the builder
            // before we get the container by calling builder.Build(). It's a chicken and egg problem.
            // I'd suggest the env var approach.

            string server = "ROCINANTE";
            string database = "Sandbox";
            string userId = "logger";
            string password = "P@ssw0rd^";

            string logDB = $@"Server={server}; Database={database};User Id={userId}; Password={password};Encrypt=False;";
            var sinkOpts = new MSSqlServerSinkOptions();
            sinkOpts.TableName = "serilog_logs";
            var columnOpts = new ColumnOptions();
            columnOpts.Store.Remove(StandardColumn.Properties);
            columnOpts.Store.Add(StandardColumn.LogEvent);
            columnOpts.TimeStamp.NonClusteredIndex = true;

            var loggerConfig = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: logDB,
                    sinkOptions: sinkOpts,
                    columnOptions: columnOpts
                );

            builder.RegisterSerilog(loggerConfig);
            SerilogWebClassic.Configure(cfg => cfg.LogAtLevel(LogEventLevel.Information));

            // enable for debugging as needed
            // SelfLog.Enable(msg => Debug.WriteLine(msg));
        }
    }
}
