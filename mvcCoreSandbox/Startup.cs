using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using mvcCoreSandbox.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

namespace mvcCoreSandbox
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;

            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            #region jnk
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders()
            //    ;


            //
            //------------------------------------
            //var keysFolder = Path.Combine(_environment.ContentRootPath, "Keys");
            //var keysFolder = Path.Combine(_environment.WebRootPath, "Keys");
            var keysFolder = @"D:\home\site\wwwroot\Keys";

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
                .SetApplicationName("cipherRex");

            services.AddAuthentication("Identity.Application")
                .AddCookie("Identity.Application", options =>
                {
                    options.Cookie.Name = "cipherRex";
                    options.Cookie.Path = "/";
                });

            //------------------------------------

        //    services.ConfigureExternalCookie
        //(
        //config =>
        //{
        //    config.Cookie.Name = "cipherRex";
        //    config.Cookie.Path = "/";
        //    config.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Unspecified;

        //}
        //);

            services.AddControllersWithViews();
            services.AddRazorPages();


            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config =>
            //{
            //    config.Cookie.Name = "cipherRex";

            //});

            //    services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
            //.AddCookie("cipherRex",
            //config =>
            //{
            //    config.Cookie.Name = "cipherRex";
            //    config.Cookie.Path = "/";
            //});

        }

        /*
        InvalidOperationException: 
        No authenticationScheme was specified, 
        and there was no DefaultChallengeScheme found. 
        The default schemes can be set using either AddAuthentication(string defaultScheme) 
        or AddAuthentication(Action<AuthenticationOptions> configureOptions).
        Microsoft.AspNetCore.Authentication.AuthenticationService.ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        */

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
