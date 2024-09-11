using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using CottonDollStore.ViewModels;

namespace DbCottonDollStore.Models;

public partial class CottonDollStoreContext : DbContext
{
    public CottonDollStoreContext(DbContextOptions<CottonDollStoreContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Banner> Banner { get; set; }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<Product> Product { get; set; }

    public virtual DbSet<ProductSpec> ProductSpec { get; set; }

    public virtual DbSet<Rank> Rank { get; set; }

    public virtual DbSet<Store> Store { get; set; }

    public virtual DbSet<User> User { get; set; }

    //public DbSet<VMProduct> VMProduct { get; set; } //我加的

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BID).HasName("PK_Banner");
            entity.Property(e => e.BannerImg)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Information)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID).HasName("PK__Category__19093A2B841B10F7");

            entity.Property(e => e.CategoryID)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryName).HasMaxLength(10);
            entity.Property(e => e.ParentCategory)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.ParentCategoryNavigation).WithMany(p => p.InverseParentCategoryNavigation)
                .HasForeignKey(d => d.ParentCategory)
                .HasConstraintName("FK__Category__Parent__3D5E1FD2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__Order__C3905BAF352A2270");

            entity.Property(e => e.OrderID)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ConNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.Logistics).HasMaxLength(20);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Payment).HasMaxLength(20);
            entity.Property(e => e.PickupDate).HasColumnType("datetime");
            entity.Property(e => e.PreDate).HasColumnType("datetime");
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Store).WithMany(p => p.Order)
                .HasForeignKey(d => d.StoreID)
                .HasConstraintName("FK__Order__StoreID__25518C17");

            entity.HasOne(d => d.User).WithMany(p => p.Order)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Order__UserID__2645B050");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.ProSpecID, e.ProID }).HasName("PK__OrderDet__473213D7FDDD3F54");

            entity.ToTable(tb => tb.HasTrigger("checkProductOrderQty"));

            entity.Property(e => e.OrderID)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProSpecID)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuyerReview).HasMaxLength(70);
            entity.Property(e => e.SellerRespond).HasMaxLength(70);
            entity.Property(e => e.Star)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.OrderID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__2739D489");

            entity.HasOne(d => d.Pro).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.ProID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__ProID__17F790F9");

            entity.HasOne(d => d.Store).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.StoreID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Store__19DFD96B");

            entity.HasOne(d => d.Spec).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => new { d.ProSpecID, d.ProID, d.StoreID })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDetail__1DB06A4F");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProID).HasName("PK__Product__620295F09BB96820");

            entity.Property(e => e.ProID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryID)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryID_2)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CategoryID_3)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Information).HasMaxLength(500);
            entity.Property(e => e.ProImg).HasMaxLength(50);
            entity.Property(e => e.ProName).HasMaxLength(30);
            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategory)
                .HasForeignKey(d => d.CategoryID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__02084FDA");

            entity.HasOne(d => d.CategoryID_2Navigation).WithMany(p => p.ProductCategoryID_2Navigation)
                .HasForeignKey(d => d.CategoryID_2)
                .HasConstraintName("FK__Product__Categor__02FC7413");

            entity.HasOne(d => d.CategoryID_3Navigation).WithMany(p => p.ProductCategoryID_3Navigation)
                .HasForeignKey(d => d.CategoryID_3)
                .HasConstraintName("FK__Product__Categor__03F0984C");

            entity.HasOne(d => d.Store).WithMany(p => p.Product)
                .HasForeignKey(d => d.StoreID)
                .HasConstraintName("FK__Product__StoreID__01142BA1");
        });


        modelBuilder.Entity<ProductSpec>(entity =>
        {
            entity.HasKey(e => new { e.ProSpecID, e.ProID, e.StoreID }).HasName("PK__ProductS__5C04AED6164834B6");

            entity.Property(e => e.ProSpecID)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProID)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Price).HasColumnType("decimal(6, 0)");
            entity.Property(e => e.Spec).HasMaxLength(30);
            entity.Property(e => e.SpecImg).HasMaxLength(50);

            entity.HasOne(d => d.Pro).WithMany(p => p.ProductSpec)
                .HasForeignKey(d => d.ProID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSp__ProID__398D8EEE");

            entity.HasOne(d => d.Store).WithMany(p => p.ProductSpec)
                .HasForeignKey(d => d.StoreID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSp__Store__571DF1D5");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.RankID).HasName("PK__Rank__B37AFB960001A619");

            entity.Property(e => e.RankID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RankName).HasMaxLength(5);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreID).HasName("PK__Store__3B82F0E1AAA21DE9");

            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StoreInformation).HasMaxLength(500);
            entity.Property(e => e.StoreName).HasMaxLength(30);
            entity.Property(e => e.UserID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.User).WithMany(p => p.Store)
                .HasForeignKey(d => d.UserID)
                .HasConstraintName("FK__Store__UserID__4AB81AF0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID).HasName("PK__User__1788CCACCBF52B00");

            entity.Property(e => e.UserID)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Account)
                .HasMaxLength(14)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.RankID)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RegDate).HasColumnType("datetime");
            entity.Property(e => e.StoreID)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserImg).HasMaxLength(50);
            entity.Property(e => e.UserPhone).HasMaxLength(10);

            entity.HasOne(d => d.Rank).WithMany(p => p.User)
                .HasForeignKey(d => d.RankID)
                .HasConstraintName("FK__User__RankID__47DBAE45");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
