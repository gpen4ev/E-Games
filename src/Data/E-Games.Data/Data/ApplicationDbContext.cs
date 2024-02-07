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

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

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

            builder.Entity<Order>()
                .HasOne<ApplicationUser>(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            builder.Entity<OrderItem>()
                .HasOne<Order>(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<OrderItem>()
                .HasOne<Product>(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

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

            builder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(14, 4);

            builder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            base.OnModelCreating(builder);
        }
    }
}