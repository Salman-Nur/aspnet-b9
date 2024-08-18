using Autofac;
using Autofac.Extensions.DependencyInjection;
using FirstDemo.Application;
using FirstDemo.Infrastructure;
using FirstDemo.Web;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using FirstDemo.Infrastructure.Extensions;
using FirstDemo.Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using FirstDemo.Infrastructure.Requirements;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
try
{
    Log.Information("Application Starting...");

    var builder = WebApplication.CreateBuilder(args);

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;

    Log.Information("Connection String:" + connectionString);

    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
		containerBuilder.RegisterModule(new ApplicationModule());
		containerBuilder.RegisterModule(new InfrastructureModule(connectionString, 
            migrationAssembly));
		containerBuilder.RegisterModule(new WebModule());
    });


    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, 
        (m) => m.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();
    builder.Services.AddCookieAuthentication();

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("SuperAdmin", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("Supervisor");
            policy.RequireRole("Admin");
        });

        options.AddPolicy("CourseUpdatePolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("UpdateCourse", "true");
        });

        options.AddPolicy("CourseViewPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("ViewCourse", "true");
        });

        options.AddPolicy("CourseViewRequirementPolicy", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.Requirements.Add(new CourseViewRequirement());
        });
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddSingleton<IAuthorizationHandler, CourseViewRequirementHandler>();
    builder.Services.Configure<Smtp>(builder.Configuration.GetSection("Smtp"));
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection()
        .UseStaticFiles()
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseSession();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    Log.CloseAndFlush();
}
