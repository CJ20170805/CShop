using CShop.Domain.Entities;
using CShop.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CShop.Domain.ValueObjects;

namespace CShop.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}


        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Optional: rename Identity tables
            modelBuilder.Entity<AppUser>().ToTable("Users");
            modelBuilder.Entity<AppRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            // User <-> UserProfile (1:1)
            //modelBuilder.Entity<User>()
            //  .HasOne(u => u.Profile)
            //  .WithOne(p => p.User)  
            //  .HasForeignKey<UserProfile>(p => p.UserId);

            // User <-> Role (M:M)
            //modelBuilder.Entity<User>()
            //    .HasMany(u => u.Roles)
            //    .WithMany(r => r.Users)
            //    .UsingEntity(
            //        j => j.ToTable("UserRoles")
            //    );

            modelBuilder.Entity<UserProfile>()
                .HasOne<AppUser>()
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<UserAddress>()
                .HasOne<AppUser>()
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne<AppUser>()
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // Enums to string conversion
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMethod)
                .HasConversion<string>();

            // Category hierachy (Parent <-> Subcategories)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId);

            // Category <-> Product (1:M)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Product value objects configuration
            modelBuilder.Entity<Product>(entity =>
            {
                // Map Money to decimal column
                //entity.Property(p => p.Price)
                //    .HasConversion(
                //        v => v.Amount, // to database
                //        v => Money.FromUSD(v) // from database
                //    );
                //// Map Stock to int column
                //entity.Property(p => p.Stock)
                //    .HasConversion(
                //        v => v.Quantity, // to database
                //        v => new Stock(v) // from database
                //    );

                entity.OwnsOne(p => p.Price, m =>
                {
                    m.Property(p => p.Amount).HasColumnName("Price");
                    m.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
                });

                entity.OwnsOne(p => p.Stock, s =>
                {
                    s.Property(s => s.Quantity).HasColumnName("Stock");
                });


            });

            // Product <-> ProductImage (1:M)
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order <-> Order Item (1:M)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);

            // OrderItem <-> Product (M:1)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Order value objects configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.OwnsOne(o => o.TotalAmount, m =>
                {
                    m.Property(p => p.Amount).HasColumnName("TotalAmount");
                    m.Property(p => p.Currency).HasColumnName("TotalCurrency").HasMaxLength(3);
                });

                entity.OwnsOne(o => o.ShippingAddress, sa => 
                {                     
                    sa.Property(a => a.RecipientName).HasColumnName("RecipientName").HasMaxLength(20);
                    sa.Property(a => a.AddressLine1).HasColumnName("AddressLine1").HasMaxLength(100);
                    sa.Property(a => a.AddressLine2).HasColumnName("AddressLine2").HasMaxLength(100);
                    sa.Property(a => a.City).HasColumnName("City").HasMaxLength(20);
                    sa.Property(a => a.City).HasColumnName("State").HasMaxLength(20);
                    sa.Property(a => a.City).HasColumnName("PostalCode").HasMaxLength(10);
                    sa.Property(a => a.Country).HasColumnName("Country").HasMaxLength(20);
                });
            });

            // Order Item value objects configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.OwnsOne(oi => oi.UnitPrice, m =>
                {
                    m.Property(p => p.Amount).HasColumnName("UnitPrice");
                    m.Property(p => p.Currency).HasColumnName("UnitCurrency").HasMaxLength(3);
                });
            });


            // Tag <-> Product (M: M)
            modelBuilder.Entity<ProductTag>()
                .HasKey(pt => new { pt.ProductId, pt.TagId });

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(pt =>pt.TagId);
        }

    }
}
