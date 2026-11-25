using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TemperaturStyring.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ThermometerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ThermometerDb")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ThermometerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Index.html";
    options.Cookie.Name = "TemperaturStyring.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseDefaultFiles();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        bool isFrontPage =
            context.Context.Request.Path.Value?
                .Equals("/FrontPage.html", StringComparison.OrdinalIgnoreCase)
                == true;

        bool isAuthenticated =
            context.Context.User.Identity?.IsAuthenticated == true;

        if (isFrontPage && !isAuthenticated)
        {
            context.Context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Context.Response.ContentLength = 0;
        }
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();