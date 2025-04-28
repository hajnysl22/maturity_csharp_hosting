using Microsoft.EntityFrameworkCore;
using PoznamkyApp.Data;

namespace PoznamkyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Ensure you add DbContext and use the connection string from appsettings.json
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();

            var app = builder.Build();

            // Code to test the database in early stages of development
            //TestDB(app);
            // Configure the HTTP request pipeline, middleware, etc.
            app.UseRouting();
            app.MapDefaultControllerRoute();
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Register}/{id?}");

            app.Run();
        }
    }
}
