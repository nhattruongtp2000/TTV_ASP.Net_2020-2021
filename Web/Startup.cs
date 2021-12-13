using Data.Data;
using Data.DB;
using DI.DI.Interace;
using DI.DI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs;

namespace Web
{
    public class Startup
    {
        // Enables SignalR for online user count.
        public static bool EnableSignalR { get; } = true;


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<Iden2Context>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddTransient<IProductRepository,ProductRepository>();
            services.AddTransient<IAnalystRepository, AnalystRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IContactRepository, ContactRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IVoucherRepository, VoucherRepository>();
            services.AddTransient<IBlogRepository, BlogRepository>();
            services.AddTransient<ISlideRepository, SlideRepository>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();



            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<Iden2Context>().AddDefaultTokenProviders();

            services.AddHttpContextAccessor();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt


                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại


            });

         
            services.AddLogging();
            services.AddDistributedMemoryCache();
            services.AddSession(cfg => {
                cfg.IdleTimeout = TimeSpan.FromMinutes(30);

            });

            services.AddAuthentication().AddFacebook(options => {
                options.AppId = Configuration["Facebook:AppId"];
                options.AppSecret = Configuration["Facebook:AppSecret"];
                options.CallbackPath = "/login-facebook";
            });
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = Configuration["Google:ClientId"];
                options.ClientSecret = Configuration["Google:ClientSecret"];
                options.CallbackPath = "/login-google";
            });
            



            if (EnableSignalR)
                services.AddSignalR();

            services.AddCors();

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
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthentication();

            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
              name: "news",
              pattern: "home",
              defaults: new { controller = "Home", action = "Indexx" });


                endpoints.MapControllerRoute(
               name: "news",
               pattern: "login",
               defaults: new { controller = "Accounts", action = "Index" });


                endpoints.MapControllerRoute(
               name: "news",
               pattern: "{Alias}",
               defaults: new { controller = "Products", action = "ProductDetails" });



                endpoints.MapControllerRoute(
                name: "default",
                pattern: "laptop",
                new { controller = "Products", action = "GetProductPerCategory" ,IdCategory=1});

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "phone",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory =2});

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "headphone",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 3 });

                endpoints.MapControllerRoute(
                name: "news",
                pattern: "pc",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 4 });

                endpoints.MapControllerRoute(
                name: "news",
                pattern: "mouse",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 5 });

                endpoints.MapControllerRoute(
              name: "default",
              pattern: "laptop",
              new { controller = "Products", action = "GetProductPerCategory", IdCategory = 1 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "phone",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 2 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "headphone",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 3 });

                endpoints.MapControllerRoute(
                name: "news",
                pattern: "pc",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 4 });

                endpoints.MapControllerRoute(
                name: "news",
                pattern: "mouse",
                new { controller = "Products", action = "GetProductPerCategory", IdCategory = 5 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "samsung",
                new { controller = "Products", action = "GetProductPerBrand", IdBrand = 1 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "dell",
                new { controller = "Products", action = "GetProductPerBrand", IdBrand = 2 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "apple",
                new { controller = "Products", action = "GetProductPerBrand", IdBrand = 3 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "asus",
                new { controller = "Products", action = "GetProductPerBrand", IdBrand = 4 });

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "lg",
                new { controller = "Products", action = "GetProductPerBrand", IdBrand = 5 });

                endpoints.MapControllerRoute(
               name: "news",
               pattern: "lenovo",
               new { controller = "Products", action = "GetProductPerBrand", IdBrand = 6 });







                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Indexx}/{id?}");


                endpoints.MapAreaControllerRoute(
                 name: "Admin",
                 areaName: "Admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                if (EnableSignalR)
                    endpoints.MapHub<OnlineCountHub>("/onlinecount");
            });


        }
    }
}
