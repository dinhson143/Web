using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Congtys;
using Web.ServiceApi_Admin_User.Service.Contacts;
using Web.ServiceApi_Admin_User.Service.Languages;
using Web.ServiceApi_Admin_User.Service.LoaiPhieus;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Roles;
using Web.ServiceApi_Admin_User.Service.Sizes;
using Web.ServiceApi_Admin_User.Service.Sliders;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.ViewModels.System.User;

namespace Web.AdminApp
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
            services.AddHttpClient();
            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
            services.AddTransient<IUserApi, UserAPi>();
            services.AddTransient<IRoleApi, RoleApi>();
            services.AddTransient<ILanguageApi, LanguageApi>();
            services.AddTransient<IProductApi, ProductApi>();
            services.AddTransient<ICategoryApi, CategoryApi>();
            services.AddTransient<ISliderApi, SliderApi>();
            services.AddTransient<ISizeApi, SizeApi>();
            services.AddTransient<IContactApi, ContactApi>();
            services.AddTransient<ICongtyApi, CongtyApi>();
            services.AddTransient<ILoaiPhieuApi, LoaiPhieuApi>();

            //Session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // authenticatiom
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login/";
                    options.AccessDeniedPath = "/Login/Forbidden/";
                });
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

            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}