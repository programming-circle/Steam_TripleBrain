using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Data
{
    public class AppDbContext : DbContext //Possibly if needed you can add an IdentityDbContext for user management
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
            // Таблиця користувачів
            public DbSet<Profiles> Profiles { get; set; }

            // Таблиця журналу токенів
            public DbSet<TokenLogs> TokenLogs { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities and relationships here

            // Налаштування для Profile
            modelBuilder.Entity<Profiles>(entity =>                            
            {
                entity.HasKey(p => p.Id);                                       // Встановлюємо Id як первинний ключ
                entity.Property(p => p.Username).IsRequired().HasMaxLength(100);// Встановлюємо Username як обов'язкове поле з максимальною довжиною 100 символів
                entity.Property(p => p.PasswordHash).IsRequired();              // Встановлюємо PasswordHash як обов'язкове поле
                entity.Property(p => p.Role).IsRequired().HasMaxLength(50);     // Встановлюємо Role як обов'язкове поле з максимальною довжиною 50 символів
                entity.Property(p => p.Email).HasMaxLength(200);                // Встановлюємо Email з максимальною довжиною 200 символів (не обов'язкове поле)
                entity.Property(p => p.FullName).HasMaxLength(200);             // Встановлюємо FullName з максимальною довжиною 200 символів (не обов'язкове поле)
            });

            // Налаштування для TokenLog
            modelBuilder.Entity<TokenLogs>(entity =>                            
            {
                entity.HasKey(t => t.Id);                                       // Встановлюємо Id як первинний ключ
                entity.Property(t => t.Username).IsRequired().HasMaxLength(100);// Встановлюємо Username як обов'язкове поле з максимальною довжиною 100 символів
                entity.Property(t => t.Token).IsRequired();                     // Встановлюємо Token як обов'язкове поле
                entity.Property(t => t.IssuedAt).IsRequired();                  // Встановлюємо IssuedAt як обов'язкове поле
                entity.Property(t => t.IsRevoked).HasDefaultValue(false);       // Встановлюємо IsRevoked з дефолтним значенням false
            });

        }
    }
}
