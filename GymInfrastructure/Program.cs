using Microsoft.EntityFrameworkCore;
using GymInfrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using GymDomain.Model;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); 

        options.SlidingExpiration = false; 
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        
        options.Cookie.Expiration = null;
    });

builder.Services.AddAuthorization(); 
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GYMDbContext>(option => option.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GYMDbContext>();

    // Перевіряємо, чи є адмін
    if (!dbContext.Users.Any(u => u.Email == "admin@admin.com"))
    {
        dbContext.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@admin.com",
            Password = "admin123", // якщо не хешуєш
            Role = "Admin"
        });

        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

