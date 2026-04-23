using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Serilog;
using Steam_TripleBrain.Data;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//using Steam_TripleBrain.Repositories;
//using Steam_TripleBrain.Repositories.Interface;
using Steam_TripleBrain.Services;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Steam_TripleBrain.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Address for FrontEnd
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd", police =>
    {
        police.WithOrigins("http://192.168.0.123:3000") // Address for FrontEnd of other device
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
            "logs/log.txt",
            rollingInterval: RollingInterval.Day)
    .CreateLogger();

////For appsetting.json(Loger)
//builder.Host.UseSerilog((context, config) =>
//{
//    config.ReadFrom.Configuration(context.Configuration);
//});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});


//DB
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null)
{
    throw new Exception("No connection string in appsettings.json");
}
try
{
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(connectionString)
    );
}
catch (Exception ex)
{
    throw new Exception("Can't connect to SQL Server");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(           
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Identity (только UserManager, без ролей)
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<ITokenService, TokenService>();



var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
