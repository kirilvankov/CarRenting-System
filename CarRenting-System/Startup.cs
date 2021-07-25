namespace CarRenting_System
{

    using CarRenting_System.Data;
    using CarRenting_System.Data.Infrastucture;
    using CarRenting_System.Services;
    using CarRenting_System.Services.Cars;
    using CarRenting_System.Services.Statistics;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<CarRentingDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<CarRentingDbContext>();

            services
                .AddControllersWithViews();
            services.AddTransient<IDealerService, DealerService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
            services.AddTransient<ICarsService, CarsService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDataBase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app
               .UseHttpsRedirection()
               .UseStaticFiles()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapDefaultControllerRoute();
                   endpoints.MapRazorPages();
               });
            
        }
    }
}
