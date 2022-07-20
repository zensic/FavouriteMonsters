using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FavouriteMons.Areas.Identity.Data;
using EmailService;

var builder = WebApplication.CreateBuilder(args);

// If the environment variable specifies production, use connection string stored in heroku secret
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

// Add identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add mailkit 
var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
//builder.Services.AddSingleton(() => {
//    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//    if (env == "Development")
//    {
//        var emailConfig = builder.Configuration
//            .GetSection("EmailConfiguration")
//            .Get<EmailConfiguration>();
//    }

//    EmailConfiguration emailConfiguration = new EmailConfiguration();

//    return emailConfiguration;
//});
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Add MVC and razor pages
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
