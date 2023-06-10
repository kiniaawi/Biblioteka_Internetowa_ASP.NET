using kiniaawiBooks.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace kiniaawiBooks
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //public Microsoft.AspNetCore.Http.PathString LoginPath { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                //options.LoginPath = new PathString("Identity/Account/Login");
                
            });

            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

            //    options.LoginPath = "/Identity/Account/Login";
            //    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    options.SlidingExpiration = true;
            //});

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                
            });

            //services.AddIdentity<User, UserRole>(options =>
            //{
            //    options.Cookies.ApplicationCookie.LoginPath = new PathString("/Admin/Account/Login");
            //});


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();


            //app.UseStatusCodePages(context => {
            //    var response = context.HttpContext.Response;
            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            //        response.StatusCode == (int)HttpStatusCode.Forbidden)
            //        response.Redirect("/Identity/Account/Login");
            //    return Task.CompletedTask;
            //});
            //app.UseClaimsMiddleware();
            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Login}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});
            //add.Authrntication().AddCookie;
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    LoginPath = new PathString("/WoL/Account/Login"),
            //    AutomaticChallenge = true
            //});

            //app.UseStatusCodePages(async context =>
            //{
            //    var response = context.HttpContext.Response;

            //    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
            //            response.StatusCode == (int)HttpStatusCode.Forbidden)
            //        response.Redirect("/Identity/Account/Login");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
