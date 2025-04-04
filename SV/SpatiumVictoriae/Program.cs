using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SpatiumVictoriae.Data;
using SpatiumVictoriae.Models;
using SpatiumVictoriae.Services; 

var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string and configure EF Core with SQLite.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure ASP.NET Identity with default settings and require confirmed accounts.
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // You can customize password, lockout, and other options here.
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Register an email sender service for sending confirmation and password reset emails.
// Implement IEmailSender (e.g., EmailSender class) using a provider like SendGrid or Mailgun.
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Optionally, register services for managing the onboarding tutorial state.
builder.Services.AddScoped<ITutorialService, TutorialService>();

// Add MVC controllers with views and Razor Pages.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Ensure static assets (CSS, JavaScript, images) are served.

app.UseRouting();

// IMPORTANT: Add Authentication before Authorization.
app.UseAuthentication();
app.UseAuthorization();

// Map routes for controllers and Razor Pages. 
// .WithStaticAssets() appears to be a custom extension; if it handles serving assets, ensure it's correctly implemented.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages().WithStaticAssets();

app.Run();
