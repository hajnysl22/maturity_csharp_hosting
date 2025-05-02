using PoznamkyApp.Data;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}");

app.Run();
