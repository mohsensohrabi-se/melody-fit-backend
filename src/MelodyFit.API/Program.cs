using FluentValidation;
using MediatR;
using MelodyFit.API.Middleware;
using MelodyFit.Application.Common.Behaviors;
using MelodyFit.Application.Common.Interfaces.Messaging;
using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Application.Common.Interfaces.Services;
using MelodyFit.Infrastructure.Messaging;
using MelodyFit.Infrastructure.Persistence;
using MelodyFit.Infrastructure.Persistence.Repositories;
using MelodyFit.Infrastructure.Services.Email;
using MelodyFit.Infrastructure.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// DbContext + PostgreSQL
// --------------------
builder.Services.AddDbContext<MelodyFitDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsAssembly("MelodyFit.Infrastructure"));
});

// --------------------
// MediatR + Validation
// --------------------
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(MelodyFit.Application.AssemblyReference.Assembly));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(
    MelodyFit.Application.AssemblyReference.Assembly);

// --------------------
// Domain events
// --------------------
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// --------------------
// Repositories
// --------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();

// --------------------
// Services
// --------------------
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, FakeEmailService>();
}
else
{
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
}

// --------------------
// JWT Authentication
// --------------------
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

// --------------------
// Controllers + OpenAPI
// --------------------
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // Native .NET 10 OpenAPI

// --------------------
// CORS
// --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/v1.json
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
