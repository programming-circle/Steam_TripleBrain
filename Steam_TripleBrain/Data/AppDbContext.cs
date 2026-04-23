using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid> //Possibly if needed you can add an IdentityDbContext for user management
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Table of Users
        //public DbSet<ProfilesAcc> Profiles { get; set; }

        // Table of journal tokens
        //public DbSet<TokenLogs> TokenLogs { get; set; }
        //Game
        public DbSet<Game> Games => Set<Game>();
        //DLC
        //public DbSet<DLC> DLCs => Set<DLC>();
        //User
        public DbSet<User> Users => Set<User>();
        
        
        //Order
        public DbSet<Order> Orders => Set<Order>();
        //OrderItem 
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        //Genre
        public DbSet<Genre> Genres => Set<Genre>();
        //Tag
        //public DbSet<Tag> Tags => Set<Tag>();
        //ImageUrl
        public DbSet<ImageUrl> ImageUrls => Set<ImageUrl>();
        //WishList
        public DbSet<WishList> WishLists => Set<WishList>();
        //FriendShip
        //public DbSet<FriendShip> FriendShips => Set<FriendShip>();
        //Review
        //public DbSet<Review> Reviews => Set<Review>();

        //Tokens
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities and relationships here

            // Settings for Profile
            /*
             modelBuilder.Entity<ProfilesAcc>(entity =>                            
            {
                entity.HasKey(p => p.Id);                                       // Installing Id like key
                entity.Property(p => p.Username).IsRequired().HasMaxLength(100);// Installing Username як обов'язкове поле з максимальною довжиною 100 символів
                entity.Property(p => p.PasswordHash).IsRequired();              // Installing PasswordHash як обов'язкове поле
                entity.Property(p => p.Role).IsRequired().HasMaxLength(50);     // Встановлюємо Role як обов'язкове поле з максимальною довжиною 50 символів
                entity.Property(p => p.Email).HasMaxLength(200);                // Встановлюємо Email з максимальною довжиною 200 символів (не обов'язкове поле)
                entity.Property(p => p.FullName).HasMaxLength(200);             // Встановлюємо FullName з максимальною довжиною 200 символів (не обов'язкове поле)
            });
            */

            // Settings for TokenLog
            modelBuilder.Entity<TokenLogs>(entity =>                            
            {
                entity.HasKey(t => t.Id);                                       // Встановлюємо Id як первинний ключ
                entity.Property(t => t.Username).IsRequired().HasMaxLength(100);// Встановлюємо Username як обов'язкове поле з максимальною довжиною 100 символів
                entity.Property(t => t.Token).IsRequired();                     // Встановлюємо Token як обов'язкове поле
                entity.Property(t => t.IssuedAt).IsRequired();                  // Встановлюємо IssuedAt як обов'язкове поле
                entity.Property(t => t.IsRevoked).HasDefaultValue(false);       // Встановлюємо IsRevoked з дефолтним значенням false
            });

            //Game
            // Game entity configuration
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);

                // Poster: Game -> ImageUrl (one-to-one like relationship from Game to ImageUrl via PosterId)
                entity.HasOne(g => g.Poster)
                      .WithMany()
                      .HasForeignKey("PosterId")
                      .OnDelete(DeleteBehavior.Cascade);

                // Images: Game -> ImageUrl (one-to-many)
                entity.HasMany(g => g.Images)
                      .WithOne()
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);

                // Genres: Game -> Genre (one-to-many)
                entity.HasMany(g => g.Genres)
                      .WithOne()
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);

                // Tags: Game -> Tag (one-to-many)
                /*
                 * entity.HasMany(g => g.Tags)
                      .WithOne()
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);
                */
                // DLCs: Game -> DLC (one-to-many)
                /*
                entity.HasMany(g => g.DLCs)
                      .WithOne(d => d.Game)
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Cascade);
                */
            });

            // ImageUrl entity configuration
            modelBuilder.Entity<ImageUrl>(entity =>
            {
                entity.HasKey(i => i.Id);

                // ImageUrl may belong to a Game (as part of Images collection)
                entity.HasOne<Game>()
                      .WithMany(g => g.Images)
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Genre entity configuration
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne<Game>()
                      .WithMany(g => g.Genres)
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Tag entity configuration
            /*
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne<Game>()
                      .WithMany(g => g.Tags)
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // DLC entity configuration
            modelBuilder.Entity<DLC>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.HasOne(d => d.Game)
                      .WithMany(g => g.DLCs)
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Cascade);

                // relation to User (User.DLCs)
                entity.HasOne<User>()
                      .WithMany(u => u.DLCs)
                      .HasForeignKey("UserId")
                      .OnDelete(DeleteBehavior.Restrict);
            });
            */
            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                // User icon (ImageUrl)
                entity.HasOne(u => u.Icon)
                      .WithMany()
                      .HasForeignKey("IconId")
                      .OnDelete(DeleteBehavior.Restrict);

                // Purchased games
                entity.HasMany(u => u.PurchasedGames)
                      .WithOne()
                      .HasForeignKey("UserId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WishList entity configuration
            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.HasMany(w => w.WishGames)
                      .WithOne()
                      .HasForeignKey("WishListId")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Order / OrderItem configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasMany(o => o.Items)
                      .WithOne()
                      .HasForeignKey("OrderId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
            });

            //Token
            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.HasKey(x => x.Id);

                b.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                b.HasIndex(x => x.Token).IsUnique();

                b.HasOne(x => x.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
