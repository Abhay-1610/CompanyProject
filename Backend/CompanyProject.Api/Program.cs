using CompanyProject.Api;
using CompanyProject.Api.Login.Auth;
using CompanyProject.Application;
using CompanyProject.Application.Common.Behaviors;
using CompanyProject.Infrastructure;
using CompanyProject.Infrastructure.Data;
using CompanyProject.Infrastructure.Security;
using CompanyProject.Infrastructure.SignalR;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// Core MVC / API Services
// ======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======================================================
// Database (EF Core)
// ======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ======================================================
// Identity (Users & Roles)
// ======================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ======================================================
// MediatR
// ======================================================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

// ======================================================
// FluentValidation Pipeline
// ======================================================
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

// ======================================================
// Infrastructure Layer (Repositories, Security, etc.)
// ======================================================
builder.Services.AddInfrastructure();

// ======================================================
// JWT Authentication
// ======================================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = false,
        ValidateAudience = false
    };

    // Required for SignalR JWT over query string
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// ======================================================
// FluentValidation Registration
// ======================================================
builder.Services.AddValidatorsFromAssembly(
    typeof(AssemblyReference).Assembly
);

// ======================================================
// Role Seeder
// ======================================================
builder.Services.AddScoped<RoleSeeder>();

// ======================================================
// Swagger JWT Configuration
// ======================================================
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token as: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ======================================================
// CORS (Frontend)
// ======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

//'The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. Configure the CORS policy by listing individual origins if credentials needs to be supported.'

// ======================================================
// SignalR
// ======================================================
builder.Services.AddSignalR();

var app = builder.Build();

// ======================================================
// Global Exception Handling
// ======================================================
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        // FluentValidation errors
        if (exception is FluentValidation.ValidationException ve)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(
                ve.Errors.Select(e => e.ErrorMessage)
            );
            return;
        }

        // Unauthorized / blocked user
        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(exception.Message);
            return;
        }

        // Unhandled errors
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(exception.Message);
    });
});

// ======================================================
// Role Seeding (Runs once on startup)
// ======================================================
using (var scope = app.Services.CreateScope())
{
    var roleSeeder =
        scope.ServiceProvider.GetRequiredService<RoleSeeder>();

    await roleSeeder.SeedAsync();
}

// ======================================================
// HTTP Pipeline
// ======================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendCors");

//app.UseMiddleware<AllowedPortMiddleware>();


 app.UseAuthentication();
app.UseAuthorization();

// ======================================================
// Endpoints
// ======================================================
app.MapControllers();

app.MapHub<CompanyHub>("/hubs/company"); // SignalR hub

app.Run();
