using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FavouriteMons.Areas.Identity.Data;


var builder = WebApplication.CreateBuilder(args);

// If app is 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string connectionString;
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

        if (env == "Development")
        {
            connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
        }
        else
        {
            // Use heroku config vars
            var connUser = Environment.GetEnvironmentVariable("USERNAME");
            var connPass = Environment.GetEnvironmentVariable("PASSWORD");
            var connHost = Environment.GetEnvironmentVariable("HOST");
            var connDb = Environment.GetEnvironmentVariable("DATABASE");

            connectionString = $"server={connHost};Uid={connUser};Pwd={connPass};Database={connDb}";
        }

        options.UseMySql(connectionString, serverVersion);
    });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
