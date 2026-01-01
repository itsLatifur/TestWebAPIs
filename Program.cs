using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPIs.Auth;
using TestWebAPIs.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    var authConfig = builder.Configuration.GetSection("Authentication:Password");
    options.Password.RequireDigit = authConfig.GetValue<bool>("RequireDigit");
    options.Password.RequireLowercase = authConfig.GetValue<bool>("RequireLowercase");
    options.Password.RequireUppercase = authConfig.GetValue<bool>("RequireUppercase");
    options.Password.RequireNonAlphanumeric = authConfig.GetValue<bool>("RequireNonAlphanumeric");
    options.Password.RequiredLength = authConfig.GetValue<int>("RequiredLength");
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Authentication:RequireConfirmedAccount");
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(
        builder.Configuration.GetValue<int>("Authentication:SessionTimeoutMinutes")
    );
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseMiddleware<SessionAuthenticationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
