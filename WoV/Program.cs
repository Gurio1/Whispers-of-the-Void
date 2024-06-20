using System.Reflection;
using System.Text;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using WoV;
using WoV.Cultivation;
using WoV.Database;
using WoV.Identity;
using WoV.SignalR;
using WoV.UserActivity;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
} );

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsPolicyBuilder => corsPolicyBuilder
        .WithOrigins("http://localhost:4200","https://localhost:7273")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddFastEndpoints()
    .SwaggerDocument();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false,   
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:JwtSecret"]))  
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if ( ( context.Request.Path.Value!.StartsWith("/test/cultivate")
                     )
                     && context.Request.Query.TryGetValue("access_token", out StringValues token)
                   )
                {
                    context.Token = token;
                }
 
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                var te = context.Exception;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<WoVDbContext>(dbConfig =>
    dbConfig.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<WoVDbContext>();

builder.Services.AddMemoryCache();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<ICharacterRepository,CharacterRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddSingleton<UserActivityService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddSignalR();
builder.Services.AddHostedService<CultivateHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.MapHub<CultivationHub>("test/cultivate");
app.MapHub<UserActivityHub>("test");

app.Run();

