using Microsoft.EntityFrameworkCore;

namespace Steam_TripleBrain.Data
{
    public class DataBaseContext : DbContext //Possibly if needed you can add an IdentityDbContext for user management
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entities and relationships here
        }
    }
}
