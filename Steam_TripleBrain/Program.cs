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
        options => options.
            UseSqlServer(connectionString)
        );
}
catch (Exception ex)
{
    throw new Exception("Can't connect to MySql Server");
}
//builder.Services.AddDbContext<AppDbContext>(opt =>
//    opt.UseSqlServer(builder.Configuration.
//    GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Встановлюємо схему аутентифікації за замовчуванням на JWT Bearer
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  // Встановлюємо схему виклику виклику аутентифікації за замовчуванням на JWT Bearer
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                                              // Вказуємо, що потрібно перевіряти видавця токена
        ValidateAudience = true,                                            // Вказуємо, що потрібно перевіряти аудиторію токена
        ValidateLifetime = true,                                            // Вказуємо, що потрібно перевіряти час життя токена
        ValidateIssuerSigningKey = true,                                    // Вказуємо, що потрібно перевіряти підпис токена
        ValidIssuer = builder.Configuration["Jwt:Issuer"],                  // Вказуємо допустимого видавця токена, який зберігається в конфігурації
        ValidAudience = builder.Configuration["Jwt:Audience"],              // Вказуємо допустиму аудиторію токена, яка зберігається в конфігурації
        IssuerSigningKey = new SymmetricSecurityKey(           
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))       // Вказуємо секретний ключ для перевірки підпису токена, який зберігається в конфігурації
    };
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Налаштовуємо контекст бази даних для використання SQL Server з рядком підключення з конфігурації

// Додаємо репозиторії та сервіси
// Identity (register UserManager, RoleManager, SignInManager, etc.)
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddScoped<ITokenLogRepository, TokenLogRepository>();  
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAuthService, AuthService>();                
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

// Ensure required roles exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    var roles = new[] { "User" };
    foreach (var role in roles)
    {
        var exists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
        if (!exists)
        {
            var createResult = roleManager.CreateAsync(new AppRole { Name = role }).GetAwaiter().GetResult();
            if (!createResult.Succeeded)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError("Failed to create role {role}: {errors}", role, string.Join(',', createResult.Errors.Select(e => e.Description)));
            }
        }
    }
}

app.Run();
