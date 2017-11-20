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
using System.Composition.Hosting;
using System.Reflection;

namespace Bcs.Analyzer.DemoWeb
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDataProtection();
            //services.AddAuthorization();
            services.AddWebEncoders();
            services.AddDotVVM(options =>
            {
                var dynamicDataConfig = new AppDynamicDataConfiguration();
                options.AddDynamicData(dynamicDataConfig);
                options.AddDefaultTempStorages("Temp");
            });

            //var configuration = new ContainerConfiguration()
            //    .WithAssembly(typeof(Startup).GetTypeInfo().Assembly)
            //    .WithAssembly(typeof(Registry).GetTypeInfo().Assembly); ;

            //var container = configuration.CreateContainer();

            services.AddSingleton<IMapper>(ConfigureMapper().CreateMapper());

            services.AddTransient<EntitiesTab, EntitiesTab>();
            services.RegisterFactory<BiochemicalEntityDetail, BiochemicalEntityDetail>();
            services.AddTransient<EditableGrid<ComponentLinkDto>, EditableGrid<ComponentLinkDto>>();
            services.AddTransient<EditableGrid<LocationLinkDto>, EditableGrid<LocationLinkDto>>();
            services.AddTransient<EditableGrid<ClassificationDto>, EditableGrid<ClassificationDto>>();
            services.AddTransient<EditableGrid<EntityNoteDto>, EditableGrid<EntityNoteDto>>();

            services.RegisterDependenciesBL();
            services.AddSingleton<IFormBuilder, BootstrapFormGroupBuilder>();
        }

        private static MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.RegisterMapperBL();
                cfg.CreateMap<BiochemicalEntityDetailDto, BiochemicalEntityDetail>();
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
}
