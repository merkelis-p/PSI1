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


builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped <ITaskApiService, TaskApiService>();
builder.Services.AddScoped<ICourseApiService, CourseApiService>();
builder.Services.AddScoped<ISubjectApiService, SubjectApiService>();
builder.Services.AddScoped<IEventApiService, EventApiService>();
builder.Services.AddScoped<SubjectStreamReader>();
builder.Services.AddScoped<SubjectStatusService>();
builder.Services.AddScoped<CourseStatusService>();


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
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use Session middleware
app.UseSession();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Define default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

