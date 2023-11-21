using WakeyWakey.Models;
using WakeyWakey.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: Set a timeout for the session. Adjust as needed.
    options.Cookie.HttpOnly = true; // Makes the session cookie inaccessible to JavaScript
});

builder.Services.AddScoped<ApiService<Event>>();
builder.Services.AddScoped<ApiService<User>>(); // cia reikia pakeisti i IApiService
builder.Services.AddScoped<ApiService<Course>>();
builder.Services.AddScoped<ApiService<TimeRecorder>>();
builder.Services.AddScoped<SubjectStreamReader>();


// Add Authentication services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<SubjectStreamReader>();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Session middleware
app.UseSession();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
