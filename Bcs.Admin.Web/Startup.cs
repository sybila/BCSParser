using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using DotVVM.Framework.Hosting;
using AutoMapper;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using System;
using BcsAdmin.BL;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Configuration;
using DotVVM.Framework.Controls.DynamicData.Builders;
using Bcs.Admin.Web;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BcsAdmin.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Bcs.Admin.Web.ViewModels.Grids;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Mappers;
using BcsAdmin.BL.Repositories;
using Bcs.Admin.BL.Dto;
using BcsAdmin.BL.Services;
using BcsResolver.File;
using BcsAdmin.BL.Repositories.Api.BcsAdmin.BL.Repositories;
using BcsResolver.SemanticModel;

namespace Bcs.Analyzer.DemoWeb
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddDataProtection();
            //services.AddAuthorization();
            services.AddWebEncoders();
            services.AddDotVVM();

            var dynamicDataConfig = new AppDynamicDataConfiguration();
            services.AddDynamicData(dynamicDataConfig);

            services.AddSingleton(ConfigureMapper().CreateMapper());

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Startup).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(Registry).GetTypeInfo().Assembly);
            builder.RegisterAssemblyModules(typeof(AppDbContext).GetTypeInfo().Assembly);

            builder.RegisterType(typeof(AppDbContext)).As<DbContext>();
            builder.RegisterType(typeof(EntityFrameworkUnitOfWorkProvider)).As<IUnitOfWorkProvider>();
            builder.RegisterType(typeof(EntityFrameworkUnitOfWork)).As<IUnitOfWork>();
            builder.RegisterType(typeof(UtcDateTimeProvider)).As<IDateTimeProvider>();
            builder.RegisterType(typeof(ThreadLocalUnitOfWorkRegistry)).As<IUnitOfWorkRegistry>().InstancePerLifetimeScope();

            builder.RegisterType<BootstrapFormGroupBuilder>().As<IFormBuilder>();

            builder.RegisterGeneric(typeof(EditableLinkGrid<,>)).As(typeof(IEditableLinkGrid<,>));
            builder.RegisterGeneric(typeof(EditableGrid<,>)).As(typeof(IEditableGrid<,>));
            builder.RegisterGeneric(typeof(SuggestionsFacade<>)).As(typeof(SuggestionsFacade<>));
            builder.RegisterGeneric(typeof(AutoDtoMapper<,>)).As(typeof(IEntityDTOMapper<,>));
            builder.RegisterGeneric(typeof(ApiGenericRepository<>)).As(typeof(IRepository<,>));

            builder.RegisterAllByNamespace(typeof(Startup).Assembly, "Bcs.Admin.Web.ViewModels");
            builder.RegisterAllBySuffix(typeof(Registry).Assembly, "Query");
            builder.RegisterAllBySuffix(typeof(Registry).Assembly, "Repository");
            builder.RegisterAllBySuffix(typeof(Registry).Assembly, "Facade");

            builder.RegisterAssemblyTypes(typeof(Registry).Assembly)
             .Where(t => t.Name.EndsWith("Mapper"))
             .AsImplementedInterfaces()
             .AsSelf();

            builder.RegisterType<ApiWorkspace>().As<IBcsWorkspace>().SingleInstance();

            builder.Populate(services);

            var applicationContainer = builder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        private static MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.RegisterMapperBL();
                cfg.CreateMap<BiochemicalEntityDetailDto, BiochemicalEntityDetail>();
                cfg.CreateMap<BiochemicalReactionDetailDto, BiochemicalReactionDetail>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath)
            });
        }

        public const string AuthenticationScheme = "Cookie";
    }

    public static class BilderExtensions
    {
        public static void RegisterAllBySuffix(this ContainerBuilder builder, Assembly assembly, string suffix)
            => builder.RegisterAllWhere(assembly, t => t.Name.EndsWith(suffix));

        public static void RegisterAllByNamespace(this ContainerBuilder builder, Assembly assembly, string namespaceName)
            => builder.RegisterAllWhere(assembly, t => t.Namespace == namespaceName);

        public static void RegisterAllWhere(this ContainerBuilder builder, Assembly assembly, Func<Type, bool> filter)
        {
            var types = assembly.DefinedTypes.Where(filter).ToList();

            foreach (var implementationType in types)
            {
                var allTypes = implementationType.GetBaseTypesAndSelf().Where(t => t.Namespace != "System").ToList();

                var interfaces = allTypes.SelectMany(t => t.GetTypeInfo().ImplementedInterfaces).Distinct().ToList();

                foreach (var t in allTypes)
                {
                    builder.RegisterType(implementationType).As(t);
                }
                foreach (var i in interfaces)
                {

                    builder.RegisterType(implementationType).As(i);
                }
            }
        }

        public static IEnumerable<Type> GetBaseTypesAndSelf(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}
