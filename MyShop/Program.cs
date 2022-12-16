using MyShop.Context;
using MyShop.Repositories;
using MyShop.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyShop.Extensions;
using System.Text;
using System.Configuration;
using MyShop.Models;


var builder = WebApplication.CreateBuilder(args);


IConfiguration configuration = builder.Configuration;

var jwtTokenConfig = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddSingleton(jwtTokenConfig);
builder.Services.AddJWTTokenServices(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<DbContext>();
builder.Services.AddScoped<IUserRegistration, UserRegistration>();

builder.Services.AddScoped<IProductRegistration, ProductRegistration>();
//builder.Services.Configure<JwtSettings>(configuration.GetSection("JWT").CurrentConfiguration.GetSection);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10000);
}
    ); ;

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:IssuerSigningKey"))),
            ValidIssuers = new string[] { configuration.GetSection("JWT").GetSection("ValidIssuer").Value},
            ValidAudiences = new string[] { configuration.GetSection("JWT").GetSection("ValidAudience").Value},
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

        };
        
        /* x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:IssuerSigningKey"))),
            ValidIssuers = new string[] { configuration.GetValue<string>("JWT:ValidIssuer") },
            ValidAudiences = new string[] { configuration.GetValue<string>("JWT:ValidAudience") },
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

        };*/


    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();

app.MapControllers();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
