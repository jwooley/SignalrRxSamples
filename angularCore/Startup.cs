using AngularCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace angularCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //services.AddEndpointRouting();
            services.AddSignalR(); //.AddAzureSignalR();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseAzureSignalR(routes =>
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<ChatHub>("/hubs/chat");
            //    routes.MapHub<SensorHub>("/hubs/sensor");
            //    routes.MapHub<DragHub>("/hubs/dragdrop");
            //});

            app.UseSpaStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});
            app.UseRouting();
            app.UseEndpoints(config =>
            {
                config.MapHub<ChatHub>("/hubs/chat");
                config.MapHub<SensorHub>("/hubs/sensor");
                config.MapHub<DragHub>("/hubs/dragdrop");
                config.MapControllers();
            });

            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.EnvironmentName == "Development")
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
