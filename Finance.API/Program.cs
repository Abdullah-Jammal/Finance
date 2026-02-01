using Finance.API.Authorization;
using Finance.API.Middleware;
using Finance.Application;
using Finance.Infrastructure;
using Finance.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthorizationPolicies();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler,
    AuthorizationResultHandler>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtKey));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "Access";
        options.DefaultChallengeScheme = "Access";
    })

    .AddJwtBearer("Access", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var type = ctx.Principal?
                    .FindFirst("token_type")?.Value;

                if (type != "access")
                    ctx.Fail("Invalid access token");

                return Task.CompletedTask;
            }
        };
    })

    .AddJwtBearer("Temp", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var type = ctx.Principal?
                    .FindFirst("token_type")?.Value;

                if (type != "temp")
                    ctx.Fail("Invalid temp token");

                return Task.CompletedTask;
            }
        };
    })

    .AddJwtBearer("Refresh", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                var type = ctx.Principal?
                    .FindFirst("token_type")?.Value;

                if (type != "refresh")
                    ctx.Fail("Invalid refresh token");

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    var roleManager =
        scope.ServiceProvider.GetRequiredService<
            RoleManager<IdentityRole<Guid>>>();
    var userManager =
        scope.ServiceProvider.GetRequiredService<
            UserManager<Finance.Infrastructure.Identity.ApplicationUser>>();

    await AuthSeed.SeedAsync(db, roleManager, userManager);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
