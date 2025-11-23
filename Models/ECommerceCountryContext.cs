using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace E_CommerceStoreCountry.Models
{
    public partial class ECommerceCountryContext : DbContext
    {
        public ECommerceCountryContext()
        {
        }

        public ECommerceCountryContext(DbContextOptions<ECommerceCountryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddOrdersDetail> AddOrdersDetails { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Catoegry> Catoegries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-BSQ2EKM\\MSSQLSERVER1;Database=E-CommerceCountry;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Arabic_CI_AS");

            modelBuilder.Entity<AddOrdersDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataTime).HasColumnType("datetime");

                entity.Property(e => e.Orderid).HasColumnName("orderid");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Qty).HasColumnName("qty");

                entity.Property(e => e.Totalprice).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.AddOrdersDetails)
                    .HasForeignKey(d => d.Orderid)
                    .HasConstraintName("FK_AddOrdersDetails_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AddOrdersDetails)
                    .HasForeignKey(d => d.Productid)
                    .HasConstraintName("FK_AddOrdersDetails_Product");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Cart_Product");
            });

            modelBuilder.Entity<Catoegry>(entity =>
            {
                entity.ToTable("Catoegry");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DataTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.DateSystem).HasColumnType("date");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Cateo)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CateoId)
                    .HasConstraintName("FK_Product_Catoegry");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductImages)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductImages_Product");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
