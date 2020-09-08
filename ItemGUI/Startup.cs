using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemApiProject.Services;
using ItemGUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ItemGUI
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IItemRepositoryGUI, ItemRepositoryGUI>();
            services.AddScoped<IManufacturerRepositoryGUI, ManufacturerRepositoryGUI>();
            services.AddScoped<ICategoryRepositoryGUI, CategoryRepositoryGUI>();
            services.AddScoped<IItemRepositoryGUI, ItemRepositoryGUI>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddDbContext<ItemDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            
        }
    }
}
