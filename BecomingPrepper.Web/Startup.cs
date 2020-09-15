using BecomingPrepper.Data.Entities;
using BecomingPrepper.Data.Entities.ProgressTracker;
using BecomingPrepper.Data.Interfaces;
using BecomingPrepper.Data.Repositories;
using BecomingPrepper.Web.Data;
using BecomingPrepper.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using PrepGuideEntity = BecomingPrepper.Data.Entities.PrepGuideEntity;
using PrepGuideRepository = BecomingPrepper.Data.Repositories.PrepGuideRepository;

namespace BecomingPrepper.Web
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
            var connectionString = Configuration.GetSection("MongoClient").GetSection("Connection").Value;
            var database = Configuration.GetSection("Database").GetSection("Dev").Value;
            var users = Configuration.GetSection("Collections").GetSection("UsersCollection").Value;
            var prepGuides = Configuration.GetSection("Collections").GetSection("PrepGuidesCollection").Value;
            var recommendedQuantityCollection = Configuration.GetSection("Collections").GetSection("RecommendedQuantityCollection").Value;
            var foodStorageInventoryCollection = Configuration.GetSection("Collections").GetSection("FoodStorageInventoryCollection").Value;
            var mongoDatabase = new MongoClient(connectionString).GetDatabase(database);
            var usersRepository = new UserRepository(mongoDatabase, users);
            var prepGuidesRepository = new PrepGuideRepository(mongoDatabase, prepGuides);
            var recommendedQuantitiesRepository = new RecommendedQuantityRepository(mongoDatabase, recommendedQuantityCollection);
            var foodStorageInventoryRepository = new FoodStorageInventoryRepository(mongoDatabase, foodStorageInventoryCollection);

            services.Add(new ServiceDescriptor(typeof(IMongoDatabase), mongoDatabase));
            services.Add(new ServiceDescriptor(typeof(IRepository<UserEntity>), usersRepository));
            services.Add(new ServiceDescriptor(typeof(IRepository<PrepGuideEntity>), prepGuidesRepository));
            services.Add(new ServiceDescriptor(typeof(IRepository<RecommendedQuantityAmountEntity>), recommendedQuantitiesRepository));
            services.Add(new ServiceDescriptor(typeof(IRepository<FoodStorageInventoryEntity>), foodStorageInventoryRepository));

            services.Add(new ServiceDescriptor(typeof(ICoreSettings), new CoreSettings()
            {
                MongoDatabase = mongoDatabase,
                Users = usersRepository,
                PrepGuides = prepGuidesRepository,
                RecommendedQuantities = recommendedQuantitiesRepository,
                FoodStorageInventory = foodStorageInventoryRepository
            }));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

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
