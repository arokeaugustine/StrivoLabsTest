using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StrivoLabsTest.Data.Models;

public partial class StrivoTestContext : DbContext
{
    public StrivoTestContext()
    {
    }

    public StrivoTestContext(DbContextOptions<StrivoTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceToken> ServiceTokens { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VKI582V;Database=StrivoTest;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC0799297B8F");

            entity.HasIndex(e => e.ServiceId, "UQ__Services__BD1A23BD3363651E").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("Created_At");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("Is_Active");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("Password_Hash");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
        });

        modelBuilder.Entity<ServiceToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service___3214EC070E1DD98D");

            entity.ToTable("Service_Tokens");

            entity.HasIndex(e => e.Token, "UQ__Service___1EB4F8174ADD9813").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("Created_At");
            entity.Property(e => e.ExpiresAt).HasColumnName("Expires_At");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Token).HasMaxLength(255);
            entity.Property(e => e.Uid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("uid");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTokens)
                .HasPrincipalKey(p => p.ServiceId)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_service_tokens_services");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrib__3214EC07D7B86FC2");

            entity.HasIndex(e => new { e.ServiceId, e.PhoneNumber }, "UQ_service_phone").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("Created_At");
            entity.Property(e => e.IsSubscribed).HasColumnName("Is_Subscribed");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.SubscribedAt).HasColumnName("Subscribed_At");
            entity.Property(e => e.UnsubscribedAt).HasColumnName("Unsubscribed_At");

            entity.HasOne(d => d.Service).WithMany(p => p.Subscribers)
                .HasPrincipalKey(p => p.ServiceId)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_subscribers_services");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
