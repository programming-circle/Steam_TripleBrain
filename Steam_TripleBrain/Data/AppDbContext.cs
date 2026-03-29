using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Data
{
    public class AppDbContext : DbContext //Possibly if needed you can add an IdentityDbContext for user management
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Game
        public DbSet<Game> Games => Set<Game>();
        //DLC
        public DbSet<DLC> DLCs => Set<DLC>();
        //User
        public DbSet<User> Users => Set<User>();
        
        
        //Order
        public DbSet<Order> Orders => Set<Order>();
        //OrderItem 
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        //Genre
        public DbSet<Genre> Genres => Set<Genre>();
        //Tag
        public DbSet<Tag> Tags => Set<Tag>();
        //ImageUrl
        public DbSet<ImageUrl> ImageUrls => Set<ImageUrl>();
        //WishList
        public DbSet<WishList> WishLists => Set<WishList>();
        //FriendShip
        public DbSet<FriendShip> FriendShips => Set<FriendShip>();
        //Review
        public DbSet<Review> Reviews => Set<Review>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities and relationships here
        }
    }
}
