using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using Web.LocalizationResources;
using Web.ServiceApi_Admin_User.Service.Categories;
using Web.ServiceApi_Admin_User.Service.Comments;
using Web.ServiceApi_Admin_User.Service.Orders;
using Web.ServiceApi_Admin_User.Service.Products;
using Web.ServiceApi_Admin_User.Service.Roles;
using Web.ServiceApi_Admin_User.Service.Sliders;
using Web.ServiceApi_Admin_User.Service.Users;
using Web.ViewModels.System.User;

namespace Web
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
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(15);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // authenticatiom
            services.AddAuthentication(options =>
           {
               //CookieAuthenticationDefaults.AuthenticationScheme;
               //options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
               options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           })
                .AddFacebook(facebookOptions =>
                {
                    IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                    facebookOptions.AppId = facebookAuthNSection["AppId"];
                    facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
                    //facebookOptions.CallbackPath = "/Account/singinFB";
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login/";
                    options.AccessDeniedPath = "/Login/Forbidden/";
                });
            services.AddHttpClient();
            // Regiter DI
            services.AddTransient<IUserApi, UserAPi>();
            services.AddTransient<IRoleApi, RoleApi>();
            services.AddTransient<ISliderApi, SliderApi>();
            services.AddTransient<IProductApi, ProductApi>();
            services.AddTransient<ICategoryApi, CategoryApi>();
            services.AddTransient<IOrderApi, OrderApi>();
            services.AddTransient<ICommentApi, CommentApi>();
            // * mutiple language
            //1. add culture
            var cultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };
            //2. AddExpressLocalization
            services.AddControllersWithViews()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
                     {
                         // When using all the culture providers, the localization process will
                         // check all available culture providers in order to detect the request culture.
                         // If the request culture is found it will stop checking and do localization accordingly.
                         // If the request culture is not found it will check the next provider by order.
                         // If no culture is detected the default culture will be used.

                         // Checking order for request culture:
                         // 1) RouteSegmentCultureProvider
                         //      e.g. http://localhost:1234/tr
                         // 2) QueryStringCultureProvider
                         //      e.g. http://localhost:1234/?culture=tr
                         // 3) CookieCultureProvider
                         //      Determines the culture information for a request via the value of a cookie.
                         // 4) AcceptedLanguageHeaderRequestCultureProvider
                         //      Determines the culture information for a request via the value of the Accept-Language header.
                         //      See the browsers language settings

                         // Uncomment and set to true to use only route culture provider
                         ops.UseAllCultureProviders = false;
                         ops.ResourcesPath = "LocalizationResources";
                         ops.RequestLocalizationOptions = o =>
                         {
                             o.SupportedCultures = cultures;
                             o.SupportedUICultures = cultures;
                             o.DefaultRequestCulture = new RequestCulture("vi");
                         };
                     });
            // 3.  app.UseRequestLocalization();
            // 4. pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
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
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Product Category En",
                    pattern: "{culture}/categories/{id}", new
                    {
                        controller = "Product",
                        action = "ProductforCategory"
                    });
                endpoints.MapControllerRoute(
                    name: "Product Category Vn",
                    pattern: "{culture}/loai-san-pham/{id}", new
                    {
                        controller = "Product",
                        action = "ProductforCategory"
                    });

                endpoints.MapControllerRoute(
                    name: "Product Detail En",
                    pattern: "{culture}/products/{id}", new
                    {
                        controller = "Product",
                        action = "ProductDetail"
                    });
                endpoints.MapControllerRoute(
                    name: "Product Detail Vn",
                    pattern: "{culture}/san-pham/{id}", new
                    {
                        controller = "Product",
                        action = "ProductDetail"
                    });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}