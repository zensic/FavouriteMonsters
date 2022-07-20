using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FavouriteMons.Areas.Identity.Data;
using EmailService;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// If the environment variable specifies production, use connection string stored in heroku secret
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString;
    var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

    if (env == "Production")
    {
        var connUser = Environment.GetEnvironmentVariable("USERNAME");
        var connPass = Environment.GetEnvironmentVariable("PASSWORD");
        var connHost = Environment.GetEnvironmentVariable("HOST");
        var connDb = Environment.GetEnvironmentVariable("DATABASE");

        connectionString = $"server={connHost};Uid={connUser};Pwd={connPass};Database={connDb}";
    }
    else
    {
        connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
    }

    options.UseMySql(connectionString, serverVersion);
});

// Add identity service
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

// Add mailkit service
EmailConfiguration emailConfig;

if (env == "Production")
{
    var mailFrom = Environment.GetEnvironmentVariable("MAIL_FROM");
    var mailSmtpServer = Environment.GetEnvironmentVariable("MAIL_STMPSERVER");
    var mailPort = Environment.GetEnvironmentVariable("MAIL_PORT");
    var mailUsername = Environment.GetEnvironmentVariable("MAIL_USERNAME");
    var mailPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

    emailConfig = new EmailConfiguration(mailFrom, mailSmtpServer, int.Parse(mailPort), mailUsername, mailPassword);
}
else
{
    emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();
}

builder.Services.AddSingleton(emailConfig);
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
