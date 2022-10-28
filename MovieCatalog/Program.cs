using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieCatalog.Properties;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.Services;
using MovieCatalog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<ILogoutService, LogoutService>();
builder.Services.AddSingleton<IIdentityProvider, IdentityProvider>();

builder.Services.AddAuthentication(AdvancedJwtBearerHandler.AdvancedJwtBearerScheme).AddScheme<JwtBearerOptions, AdvancedJwtBearerHandler>(AdvancedJwtBearerHandler.AdvancedJwtBearerScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JwtConfigurations.Issuer,
        ValidateIssuer = true,
        ValidAudience = JwtConfigurations.Audience,
        ValidateAudience = true,
        ValidateLifetime = true,
        IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

//DB
builder.Services.AddDbContext<MovieCatalogDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
