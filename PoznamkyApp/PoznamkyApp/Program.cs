using DotNetEnv;
using PoznamkyApp.Data;

public class Program
{
    public static void Main(string[] args)
    {
        // Load .env file before building the app
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<MongoDbContext>();

        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSession();

        var app = builder.Build();

        app.UseRouting();
        app.UseSession();
        app.UseStaticFiles();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Register}/{id?}");

        app.Run();
    }
}
