using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Invoices.EF;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Common;
using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Invoices.Services;
using Invoices.Interfaces;
using Newtonsoft.Json;
using Invoices.TrackingPlugin;
using Invoices.Data.Repositories;
using Invoices.DAL.Repositories;

namespace Invoices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserWorkRepository, UserWorkRepository>();
            services.AddTransient<IWorkItemRepository, WorkItemRepository>();
            services.AddDbContext<InvoiceContext>(options => options
            .UseSqlServer(Configuration.GetConnectionString("InvoiceContext")));

            services.AddTransient<ITrackingSourceRepository>(x =>
            {
                var connection = new VssConnection(
                new Uri(Configuration.GetSection("WebConfig").GetSection("orgUrl").Value),
                new VssBasicCredential(string.Empty, Configuration.GetSection("WebConfig").GetSection("personalAccessToken").Value));
                return new AzureTrackingSourceRepository(connection.GetClient<WorkItemTrackingHttpClient>());
            });

            services.AddTransient(x =>new LoadService
            (x.GetRequiredService<ITrackingSourceRepository>(),
             x.GetRequiredService<IUserRepository>(),
             x.GetRequiredService<IWorkItemRepository>()));
            services.AddTransient<IUserWorkService, UserWorkService>();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddMvc(option => option.EnableEndpointRouting = false)
                 .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
