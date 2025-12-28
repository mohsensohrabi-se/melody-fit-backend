using MelodyFit.API.Middleware;
using MelodyFit.Application.Common.Interfaces.Messaging;
using MelodyFit.Application.Common.Interfaces.Persistence;
using MelodyFit.Application.Common.Interfaces.Services;
using MelodyFit.Infrastructure.Messaging;
using MelodyFit.Infrastructure.Persistence;
using MelodyFit.Infrastructure.Persistence.Repositories;
using MelodyFit.Infrastructure.Services.Email;
using MelodyFit.Infrastructure.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register Db Context + PostgreSQL
builder.Services.AddDbContext<MelodyFitDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql =>
        {
            npgsql.MigrationsAssembly("MelodyFit.Infrastructure");
        });
});

// Register domain event dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Register Repositories 
builder.Services.AddScoped<IUserRepository, UserRepository>();


//Register Services
builder.Services.AddScoped<IPasswordService,PasswordService >();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, FakeEmailService>();
}
else
{
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
}

// Register MediatR 
builder.Services.AddMediatR(cfg=>
    cfg.RegisterServicesFromAssembly(MelodyFit.Application.AssemblyReference.Assembly)
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.

//Custom Middleware to catch errors
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
