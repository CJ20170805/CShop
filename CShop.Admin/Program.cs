using CShop.Admin.Components;
using CShop.Admin.Services;
using CShop.Application.Interfaces;
using CShop.Infrastructure.Data;
using CShop.Domain.Identity;
using CShop.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using NLog.Web;
using CShop.Infrastructure.Logging;
using AutoMapper;
using CShop.Application.Mapping;


var builder = WebApplication.CreateBuilder(args);


// Configure NLog
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
builder.Host.UseNLog();

builder.Services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add this line to register Razor Pages services
builder.Services.AddRazorPages();

builder.Services.AddMudServices();
builder.Services.AddSingleton<ThemeService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// **Register Identity**
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

//builder.Services.AddScoped<SignInManager<AppUser>>();
//builder.Services.AddScoped<UserManager<AppUser>>();

// Register application services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

// Map Blazor component endpoints
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add this line to map Razor Pages endpoints
app.MapRazorPages();

app.Run();