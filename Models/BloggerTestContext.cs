using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bloggers.Models;

public partial class BloggerTestContext : DbContext
{
    public BloggerTestContext()
    {
    }

    public BloggerTestContext(DbContextOptions<BloggerTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blogger> Bloggers { get; set; } = null!;

    public virtual DbSet<Friend> Friends { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-9M1FS39R\\SQLSERVER272;Database=blogger_test;User Id=sa;Password=Angel123=;Trusted_Connection=SSPI;MultipleActiveResultSets=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blogger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blogger__3213E83F35170DED");

            entity.ToTable("Blogger");

            entity.Property(e => e.Id)
                .HasColumnOrder(0)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnOrder(2)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnOrder(1)
                .HasColumnName("name");
            entity.Property(e => e.Picture)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnOrder(4)
                .HasColumnName("picture");
            entity.Property(e => e.Website)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnOrder(3)
                .HasColumnName("website");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friend__3213E83F01F0E0E6");

            entity.ToTable("Friend");

            entity.Property(e => e.Id)
                .HasColumnOrder(0)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BloggerId).HasColumnOrder(1);
            entity.Property(e => e.FriendId).HasColumnOrder(2);

            entity.HasOne(d => d.Blogger).WithMany(p => p.FriendBloggers)
                .HasForeignKey(d => d.BloggerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Blogger");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations)
                .HasForeignKey(d => d.FriendId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friend");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
