using E_Games.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductRating> ProductRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductRating>()
                .HasKey(pr => new { pr.ProductId, pr.UserId });

            builder.Entity<ProductRating>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.Ratings)
                .HasForeignKey(p => p.ProductId);

            builder.Entity<ProductRating>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(p => p.UserId);

            builder.Entity<Product>()
                .HasIndex(p => p.Name);

            builder.Entity<Product>()
                .HasIndex(p => p.Platform);

            builder.Entity<Product>()
                .HasIndex(p => p.DateCreated);

            builder.Entity<Product>()
                .HasIndex(p => p.TotalRating);

            builder.Entity<Product>()
                .HasIndex(p => p.Price);

            builder.Entity<Product>()
                .HasIndex(p => p.Genre);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Age);

            builder.Entity<ProductRating>()
                .HasIndex(p => p.Rating);

            base.OnModelCreating(builder);
        }
    }
}