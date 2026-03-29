using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Repositories;
using Steam_TripleBrain.Repositories.Interface;
using Steam_TripleBrain.Services;
using Steam_TripleBrain.Services.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
builder.Services.AddScoped<IUserRepository, UserRepository>();          // Реєструємо UserRepository для інтерфейсу IUserRepository
builder.Services.AddScoped<ITokenLogRepository, TokenLogRepository>();  // Реєструємо TokenLogRepository для інтерфейсу ITokenLogRepository
builder.Services.AddScoped<IAuthService, AuthService>();                // Реєструємо AuthService для інтерфейсу IAuthService
builder.Services.AddScoped<ITokenService, TokenService>();              // Реєструємо TokenService для інтерфейсу ITokenService

var app = builder.Build();

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
