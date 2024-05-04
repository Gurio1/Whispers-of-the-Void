using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using WoV;
using WoV.Cultivation;
using WoV.Database;
using WoV.Identity;
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

builder.Services.AddFastEndpoints()
    .AddAuthenticationJwtBearer(options =>
        options.SigningKey = builder.Configuration["Auth:JwtSecret"])
    .AddAuthorization()
    .SwaggerDocument();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<WoVDbContext>(dbConfig =>
    dbConfig.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<WoVDbContext>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ICharacterRepository,CharacterRepository>();
builder.Services.AddScoped<IUserRepository,UserRepository>();

builder.Services.AddSingleton<UserActivityService>();

builder.Services.AddSignalR();
builder.Services.AddHostedService<CultivateHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication()
    .UseAuthorization();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.MapHub<CultivationHub>("hub-test");
app.MapHub<UserActivityHub>("test");

app.Run();

