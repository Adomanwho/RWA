using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BL.DALModels;

public partial class RwaLibraryContext : DbContext
{
    public RwaLibraryContext()
    {
    }

    public RwaLibraryContext(DbContextOptions<RwaLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationBook> LocationBooks { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC071FD3D91D");

            entity.ToTable("Book");

            entity.Property(e => e.AuthorFirstName).HasMaxLength(100);
            entity.Property(e => e.AuthorLastName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Book_Genre");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC077097F37D");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC073D626A04");

            entity.ToTable("Location");

            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<LocationBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC077895A2D5");

            entity.ToTable("LocationBook");

            entity.Property(e => e.Name).HasMaxLength(400);

            entity.HasOne(d => d.Book).WithMany(p => p.LocationBooks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_LocationBook_Book");

            entity.HasOne(d => d.Location).WithMany(p => p.LocationBooks)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_LocationBook_Location");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07A43B10C5");

            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
            entity.Property(e => e.LogMessage).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC0779F2ACE0");

            entity.ToTable("Reservation");

            entity.Property(e => e.Name).HasMaxLength(350);

            entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_Reservation_Book");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Reservation_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07948D3CC1");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(10);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0792FCA2BF");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(200);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public void WriteLog(int level, string name, string message, string? error = null)
    {
        Logs.Add(new Log
        {
            Name = name,
            Timestamp = DateTime.UtcNow,
            LogLevel = level,
            LogMessage = message,
            ErrorMessage = error
        });
        SaveChanges();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
