using System;
using System.Collections.Generic;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Models;

public partial class CafeDbContext : DbContext
{
    public CafeDbContext()
    {
    }

    public CafeDbContext(DbContextOptions<CafeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        // Load environment variables from .env file
        Env.Load();

        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animals_pk");

            entity.HasIndex(e => e.CaretakerId, "IX_Animals_CaretakerId");

            entity.HasIndex(e => e.Category, "IX_Animals_Category");

            entity.HasIndex(e => e.IsActive, "IX_Animals_IsActive");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Species).HasMaxLength(100);

            entity.HasOne(d => d.Caretaker).WithMany(p => p.Animals)
                .HasForeignKey(d => d.CaretakerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animals_Users");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Events_pk");

            entity.HasIndex(e => e.CreatedBy, "IX_Events_CreatedBy");

            entity.HasIndex(e => e.IsPublic, "IX_Events_IsPublic");

            entity.HasIndex(e => e.StartDate, "IX_Events_StartDate");

            entity.Property(e => e.EndDate).HasPrecision(0);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasPrecision(0);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Events_Users");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MenuItems_pk");

            entity.HasIndex(e => e.Category, "IX_MenuItems_Category");

            entity.HasIndex(e => e.IsAvailable, "IX_MenuItems_IsAvailable");

            entity.Property(e => e.AllergensInfo).HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(6, 2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Orders_pk");

            entity.HasIndex(e => e.CreatedAt, "IX_Orders_CreatedAt");

            entity.HasIndex(e => e.ReservationId, "IX_Orders_ReservationId");

            entity.HasIndex(e => e.Status, "IX_Orders_Status");

            entity.Property(e => e.CreatedAt).HasPrecision(0);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Orders_Reservations");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.MenuItemId, "IX_OrderItems_MenuItemId");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.MenuItem).WithMany()
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderItems_MenuItems");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("OrderItems_Orders");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reservations_pk");

            entity.HasIndex(e => e.CreatedAt, "IX_Reservations_CreatedAt");

            entity.HasIndex(e => e.ReservationDate, "IX_Reservations_ReservationDate");

            entity.HasIndex(e => e.Status, "IX_Reservations_Status");

            entity.HasIndex(e => e.TableId, "IX_Reservations_TableId");

            entity.HasIndex(e => e.UserId, "IX_Reservations_UserId");

            entity.Property(e => e.CreatedAt).HasPrecision(0);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.ReservationDate).HasPrecision(0);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Table).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reservations_Tables");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reservations_Users");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tables_pk");

            entity.HasIndex(e => e.IsAvailable, "IX_Tables_IsAvailable");

            entity.HasIndex(e => e.Zone, "IX_Tables_Zone");

            entity.Property(e => e.Number).HasMaxLength(10);
            entity.Property(e => e.Zone).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pk");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Role, "IX_Users_Role");

            entity.Property(e => e.CreatedAt).HasPrecision(0);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}