using CMDB.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CMDB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false).AddRazorRuntimeCompilation();
            services.AddTransient<ITokenStore, TokenStore>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                /*endpoints.MapControllerRoute(
                    name: "ReleaseMobile",
                    pattern: "{controller}/{action}/{id?}/{MobileId}",
                    new { controller = "Identity", action = "ReleaseMobile" }
                    );
                endpoints.MapControllerRoute(
                    name: "ReleaseDevice",
                    pattern: "{controller}/{action}/{id?}/{AssetTag}",
                    new { controller = "Identity", action = "ReleaseDevice" }
                    );
                endpoints.MapControllerRoute(
                    name: "ReleaseSubscription",
                    pattern: "{controller}/{action}/{id?}/{SubscriptionId}",
                    new { controller = "Identity", action = "ReleaseSubscription" }
                    );*/
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
