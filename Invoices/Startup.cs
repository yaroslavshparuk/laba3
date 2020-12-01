using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Proxies;
using Microsoft.EntityFrameworkCore;
using Invoices.EF;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Common;
using System;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Invoices.TrackingPlugin;
using Invoices.Services;
using Invoices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            services.AddDbContext<InvoiceContext>(options => options.UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("InvoiceContext")));
            services.AddTransient<ITrackingService>(x =>
            {
                var connection = new VssConnection(
                new Uri(Configuration.GetSection("WebConfig").GetSection("orgUrl").Value),
                new VssBasicCredential(string.Empty, Configuration.GetSection("WebConfig").GetSection("personalAccessToken").Value));
                return new DevOpsTrackingService(connection.GetClient<WorkItemTrackingHttpClient>());
            });
            services.AddTransient(x =>new LoadService
            (x.GetRequiredService<ITrackingService>(),
             x.GetRequiredService<InvoiceContext>()));
            services.AddTransient<IInvoiceBuilder, GridInvoiceBuilder>();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddMvc(option => option.EnableEndpointRouting = false)
                 .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
