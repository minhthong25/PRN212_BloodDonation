using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9BG5A96\\USERDUYVU;uid=sa;pwd=12345;database=BloodDonationDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC22E2987B8");

            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Donor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__Appointme__Donor__3E52440B");

            entity.HasOne(d => d.Location).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__Locat__3F466844");
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.HasKey(e => e.BloodGroupId).HasName("PK__BloodGro__4398C68F4A2C6D9F");

            entity.Property(e => e.GroupName).HasMaxLength(10);
        });

        modelBuilder.Entity<BloodInventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__BloodInv__F5FDE6B3E041CB0B");

            entity.ToTable("BloodInventory");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.BloodInventories)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodInve__Blood__403A8C7D");
        });

        modelBuilder.Entity<BloodRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BloodReq__33A8517AE4DD10B4");

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
                .HasConstraintName("FK__BloodRequ__Blood__412EB0B6");

            entity.HasOne(d => d.Recipient).WithMany(p => p.BloodRequests)
                .HasForeignKey(d => d.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BloodRequ__Recip__4222D4EF");
        });

        modelBuilder.Entity<Donor>(entity =>
        {
            entity.HasKey(e => e.DonorId).HasName("PK__Donors__052E3F781FFC947E");

            entity.Property(e => e.DonorId).ValueGeneratedNever();

            entity.HasOne(d => d.BloodGroup).WithMany(p => p.Donors)
                .HasForeignKey(d => d.BloodGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donors__BloodGro__4316F928");

            entity.HasOne(d => d.DonorNavigation).WithOne(p => p.Donor)
                .HasForeignKey<Donor>(d => d.DonorId)
                .HasConstraintName("FK__Donors__DonorId__440B1D61");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA497FBCF819D");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.HasKey(e => e.RecipientId).HasName("PK__Recipien__F0A6024D73C70066");

            entity.Property(e => e.RecipientId).ValueGeneratedNever();
            entity.Property(e => e.MedicalCondition).HasMaxLength(255);

            entity.HasOne(d => d.RecipientNavigation).WithOne(p => p.Recipient)
                .HasForeignKey<Recipient>(d => d.RecipientId)
                .HasConstraintName("FK__Recipient__Recip__44FF419A");
        });

        modelBuilder.Entity<RequestApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__RequestA__328477F4985FE61F");

            entity.Property(e => e.ApprovalDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ApprovalStatus).HasMaxLength(20);
            entity.Property(e => e.Notes).HasMaxLength(255);

            entity.HasOne(d => d.ApproverUser).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.ApproverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestAp__Appro__45F365D3");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestApprovals)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__RequestAp__Reque__46E78A0C");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestResu__8CC33160F6B6CCD8");

            entity.Property(e => e.ResultNote).HasMaxLength(255);

            entity.HasOne(d => d.Donor).WithMany(p => p.TestResults)
                .HasForeignKey(d => d.DonorId)
                .HasConstraintName("FK__TestResul__Donor__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C74147165");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534CEE6906E").IsUnique();

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
