using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repository.Models;

public partial class BloodDonationDbContext : DbContext
{
    public BloodDonationDbContext()
    {
    }

    public BloodDonationDbContext(DbContextOptions<BloodDonationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<BloodGroup> BloodGroups { get; set; }

    public virtual DbSet<BloodInventory> BloodInventories { get; set; }

    public virtual DbSet<BloodRequest> BloodRequests { get; set; }

    public virtual DbSet<Donor> Donors { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<RequestApproval> RequestApprovals { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC2D705DF92");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Donor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__Appointme__Donor__59063A47");

            entity.HasOne(d => d.Location).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Locat__59FA5E80");
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.HasKey(e => e.BloodGroupId).HasName("PK__BloodGro__4398C68F3867153F");

            entity.Property(e => e.GroupName).HasMaxLength(10);
        });

        modelBuilder.Entity<BloodInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__BloodInv__F5FDE6B3583BC122");

            entity.ToTable("BloodInventory");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodInve__Blood__45F365D3");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BloodReq__33A8517AB3B6778D");

            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestType)
                .HasMaxLength(20)
                .HasDefaultValue("Normal");
            entity.Property(e => e.ResponseMessage).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodRequ__Blood__4BAC3F29");

            entity.HasOne(d => d.Recipient).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodRequ__Recip__4AB81AF0");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donors__052E3F7848FCA1B7");

            entity.Property(e => e.DonorId).ValueGeneratedNever();

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.Donors)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donors__BloodGro__3F466844");

            entity.HasOne(d => d.DonorNavigation).WithOne(p => p.Donor)
                .HasForeignKey<Donor>(d => d.DonorId)
                .HasConstraintName("FK__Donors__DonorId__3E52440B");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA4972595E6DB");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.HasKey(e => e.RecipientId).HasName("PK__Recipien__F0A6024DF66514E7");

            entity.Property(e => e.RecipientId).ValueGeneratedNever();
            entity.Property(e => e.MedicalCondition).HasMaxLength(255);

            entity.HasOne(d => d.RecipientNavigation).WithOne(p => p.Recipient)
                .HasForeignKey<Recipient>(d => d.RecipientId)
                .HasConstraintName("FK__Recipient__Recip__4222D4EF");
        });

        modelBuilder.Entity<RequestApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__RequestA__328477F4BB5A0875");

            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatus).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.ApproverUser).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.ApproverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestAp__Appro__5070F446");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__RequestAp__Reque__4F7CD00D");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestResu__8CC3316085609C3B");

            entity.Property(e => e.ResultNote).HasMaxLength(255);

            entity.HasOne(d => d.Donor).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__TestResul__Donor__534D60F1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CDCE59778");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345FCACF4A").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
