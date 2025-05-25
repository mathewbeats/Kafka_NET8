using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Entities; // Importar ValueConverters

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Lendit.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.Property(e => e.Images)
                    .HasConversion(ValueConverters.ListStringToJsonConverter);

                entity.Property(e => e.Attributes)
                    .HasConversion(ValueConverters.DictionaryStringToJsonConverter);
            });
        }
    }
}