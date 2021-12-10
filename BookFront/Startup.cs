using Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookFront
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
            services.AddControllersWithViews();
            
            var baseAddress = Configuration.GetSection("ApiSetting").GetSection("BaseAddress").Value;
            services.AddTransient<HttpClient>(a => new HttpClient() { BaseAddress = new Uri(baseAddress) });

            var readRegistrations = typeof(Client).Assembly.GetExportedTypes()
                .Where(x => x.Namespace.StartsWith("Api"))
                .Where(x => x.GetInterfaces().Any())
                .Where(x => x.BaseType != typeof(ApiException) && x.IsClass && x.BaseType != typeof(System.Exception))
                .Select(x => new { Service = x.GetInterfaces().Single(), Implementation = x });
            foreach (var reg in readRegistrations)
            {
                services.AddTransient(reg.Service, reg.Implementation);
            }

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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Book}/{action=Index}/{id?}");
            });
        }
    }
}
