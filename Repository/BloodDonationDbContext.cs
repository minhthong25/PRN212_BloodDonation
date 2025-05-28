using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models;

namespace Repository;

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

    public virtual DbSet<RecoveryReminder> RecoveryReminders { get; set; }

    public virtual DbSet<RequestApproval> RequestApprovals { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<TestResult> TestResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC2E11B4EB4");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);

            entity.HasOne(d => d.Donor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__Appointme__Donor__59FA5E80");

            entity.HasOne(d => d.Location).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Appointme__Locat__5AEE82B9");
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.HasKey(e => e.BloodGroupId).HasName("PK__BloodGro__4398C68FD5449256");

            entity.Property(e => e.GroupName).HasMaxLength(10);
        });

        modelBuilder.Entity<BloodInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__BloodInv__F5FDE6B304E65177");

            entity.ToTable("BloodInventory");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.BloodGroupId)
                .HasConstraintName("FK__BloodInve__Blood__44FF419A");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BloodReq__33A8517A82E6B97E");

            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ResponseMessage).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.BloodGroupId)
                .HasConstraintName("FK__BloodRequ__Blood__49C3F6B7");

            entity.HasOne(d => d.Recipient).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.RecipientId)
                .HasConstraintName("FK__BloodRequ__Recip__48CFD27E");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donors__052E3F78C963B387");

            entity.Property(e => e.DonorId).ValueGeneratedNever();

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.Donors)
                .HasForeignKey(d => d.BloodGroupId)
                .HasConstraintName("FK__Donors__BloodGro__3E52440B");

            entity.HasOne(d => d.DonorNavigation).WithOne(p => p.Donor)
                .HasForeignKey<Donor>(d => d.DonorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donors__DonorId__3D5E1FD2");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA49778F5CE9F");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.HasKey(e => e.RecipientId).HasName("PK__Recipien__F0A6024D8F94E91D");

            entity.Property(e => e.RecipientId).ValueGeneratedNever();
            entity.Property(e => e.MedicalCondition).HasMaxLength(255);

            entity.HasOne(d => d.RecipientNavigation).WithOne(p => p.Recipient)
                .HasForeignKey<Recipient>(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Recipient__Recip__412EB0B6");
        });

        modelBuilder.Entity<RecoveryReminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId).HasName("PK__Recovery__01A8308740DB4CE7");

            entity.Property(e => e.Notified).HasDefaultValue(false);

            entity.HasOne(d => d.Donor).WithMany(p => p.RecoveryReminders)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__RecoveryR__Donor__5EBF139D");
        });

        modelBuilder.Entity<RequestApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__RequestA__328477F45D231FB3");

            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatus).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.Request).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__RequestAp__Reque__5070F446");

            entity.HasOne(d => d.Staff).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__RequestAp__Staff__5165187F");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AB17B6AB6FBA");

            entity.Property(e => e.StaffId).ValueGeneratedNever();

            entity.HasOne(d => d.StaffNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staffs__StaffId__4CA06362");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestResu__8CC33160A9C75E9D");

            entity.Property(e => e.ResultNote).HasMaxLength(255);

            entity.HasOne(d => d.Donor).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__TestResul__Donor__5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C4C2B9D1E");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105349CAC596D").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
