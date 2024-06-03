using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Interfaces;
using SharedLibrary.Models;
using SharedLibrary.Services;

try
{
    #region Service configuration

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRazorPages();
    builder.Services.AddMvc();

    IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireNonAlphanumeric = false;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
    })
    .AddEntityFrameworkStores<SalesOrderDBContext>()
    .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(options => {
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt32(configuration["AuthTokenExpiryInHours"]));
        options.LoginPath = "/asd";
        options.LogoutPath = "/asdasd";
        options.AccessDeniedPath = "/asdasdasd";
        options.SlidingExpiration = true;

    });

    builder.Services.AddDbContext<SalesOrderDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

    builder.Services.AddTransient<IDBSeed, DBSeed>();
    builder.Services.AddTransient<IJWTWebLoginHandler, JWTWebLoginHandler>();
    builder.Services.AddTransient<IDBService, DBService>();
    builder.Services.AddTransient<IXMLDBService, XMLDBService>();

    #endregion Service configuration

    #region App configuration

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();

    IServiceScope scope = app.Services.CreateScope();

    IDBSeed _templateDbSeed = scope.ServiceProvider.GetRequiredService<IDBSeed>();

    _templateDbSeed.SeedDefaultData();

    app.Run();

    #endregion App configuration
}
catch (Exception ex)
{
    throw;
}