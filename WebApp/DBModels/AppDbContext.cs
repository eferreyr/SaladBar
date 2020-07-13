using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SaladBarWeb.DBModels
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Audits> Audits { get; set; }
        public virtual DbSet<AuditsStaging> AuditsStaging { get; set; }
        public virtual DbSet<Batches> Batches { get; set; }
        public virtual DbSet<DataEntryLocks> DataEntryLocks { get; set; }
        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<DevicesStaging> DevicesStaging { get; set; }
        public virtual DbSet<DuplicateImageOptions> DuplicateImageOptions { get; set; }
        public virtual DbSet<GlobalInfoItems> GlobalInfoItems { get; set; }
        public virtual DbSet<ImageMetadata> ImageMetadata { get; set; }
        public virtual DbSet<ImageTypes> ImageTypes { get; set; }
        public virtual DbSet<InterventionDays> InterventionDays { get; set; }
        public virtual DbSet<InterventionDayTrayTypes> InterventionDayTrayTypes { get; set; }
        public virtual DbSet<InterventionTrays> InterventionTrays { get; set; }
        public virtual DbSet<InterventionTraysStaging> InterventionTraysStaging { get; set; }
        public virtual DbSet<MenuItems> MenuItems { get; set; }
        public virtual DbSet<MenuItemTypes> MenuItemTypes { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }
        public virtual DbSet<RandomizedStudents> RandomizedStudents { get; set; }
        public virtual DbSet<RandomizedStudentsStaging> RandomizedStudentsStaging { get; set; }
        public virtual DbSet<RandomizedStudentTrays> RandomizedStudentTrays { get; set; }
        public virtual DbSet<RandomizedStudentTraysStaging> RandomizedStudentTraysStaging { get; set; }
        public virtual DbSet<ResearchTeamMembers> ResearchTeamMembers { get; set; }
        public virtual DbSet<Schools> Schools { get; set; }
        public virtual DbSet<SchoolTypes> SchoolTypes { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<TempAlphaTestWmtracking> TempAlphaTestWmtracking { get; set; }
        public virtual DbSet<TrayTypes> TrayTypes { get; set; }
        public virtual DbSet<WeighingMeasurementImageMetadata> WeighingMeasurementImageMetadata { get; set; }
        public virtual DbSet<WeighingMeasurementMenuItems> WeighingMeasurementMenuItems { get; set; }
        public virtual DbSet<WeighingMeasurements> WeighingMeasurements { get; set; }
        public virtual DbSet<WeighingMeasurementTracking> WeighingMeasurementTracking { get; set; }
        public virtual DbSet<WeighingMeasurementTrays> WeighingMeasurementTrays { get; set; }
        public virtual DbSet<WeighingMeasurmentGlobalInfoItems> WeighingMeasurmentGlobalInfoItems { get; set; }
        public virtual DbSet<Weighings> Weighings { get; set; }
        public virtual DbSet<WeighingsStaging> WeighingsStaging { get; set; }
        public virtual DbSet<WeighingTrays> WeighingTrays { get; set; }
        public virtual DbSet<WeighingTraysStaging> WeighingTraysStaging { get; set; }
        public virtual DbSet<WeighStationTypes> WeighStationTypes { get; set; }

        // Unable to generate entity type for table 'dbo.DataDumpRandomizedStudents_20171222'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.DataDumpRandomizedStudentTrays_20171222'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Weighings_from_laptops_20180425'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=SaladBarWeb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasIndex(e => e.UserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Audits>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("table_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValuesAfter)
                    .HasColumnName("values_after")
                    .IsUnicode(false);

                entity.Property(e => e.ValuesBefore)
                    .HasColumnName("values_before")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AuditsStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasColumnName("action")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnName("table_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ValuesAfter)
                    .HasColumnName("values_after")
                    .IsUnicode(false);

                entity.Property(e => e.ValuesBefore)
                    .HasColumnName("values_before")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Batches>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DataEntryLocks>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasColumnName("asp_net_user_id")
                    .HasMaxLength(450);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.Locked)
                    .IsRequired()
                    .HasColumnName("locked")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AspNetUser)
                    .WithMany(p => p.DataEntryLocks)
                    .HasForeignKey(d => d.AspNetUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dataentrylocks_asp_net_user_id_aspnetusers_id");

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.DataEntryLocks)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_dataentrylocks_intervention_day_id_interventiondays_id");
            });

            modelBuilder.Entity<Devices>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasColumnName("device_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Seed).HasColumnName("seed");
            });

            modelBuilder.Entity<DevicesStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasColumnName("device_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DuplicateImageOptions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlobalInfoItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImageMetadata>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImageTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InterventionDays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtIntervention)
                    .HasColumnName("dt_intervention")
                    .HasColumnType("date");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionFinished)
                    .IsRequired()
                    .HasColumnName("intervention_finished")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SampleSize).HasColumnName("sample_size");

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.InterventionDays)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_interventiondays_school_id_schools_id");
            });

            modelBuilder.Entity<InterventionDayTrayTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrayTypeId).HasColumnName("tray_type_id");

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.InterventionDayTrayTypes)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_menuitems_intervention_day_id_interventiondays_id");

                entity.HasOne(d => d.TrayType)
                    .WithMany(p => p.InterventionDayTrayTypes)
                    .HasForeignKey(d => d.TrayTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_interventiondaytraytypes_tray_type_id_traytypes_id");
            });

            modelBuilder.Entity<InterventionTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrayId).HasColumnName("tray_id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.InterventionTrays)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_interventiontrays_intervention_day_id_interventiondays_id");
            });

            modelBuilder.Entity<InterventionTraysStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrayId).HasColumnName("tray_id");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<MenuItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.MenuId).HasColumnName("menu_id");

                entity.Property(e => e.MenuItemTypeId).HasColumnName("menu_item_type_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Quantifiable)
                    .IsRequired()
                    .HasColumnName("quantifiable")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_menuitems_menu_id_menus_id");

                entity.HasOne(d => d.MenuItemType)
                    .WithMany(p => p.MenuItems)
                    .HasForeignKey(d => d.MenuItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_menuitems_menu_item_type_id_menuitemtypes_id");
            });

            modelBuilder.Entity<MenuItemTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_menus_intervention_day_id_interventiondays_id");
            });

            modelBuilder.Entity<RandomizedStudents>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Assent)
                    .HasColumnName("assent")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.RandomizedStudents)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_randomizedstudents_intervention_day_id_interventiondays_id");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.RandomizedStudents)
                    .HasPrincipalKey(p => new { p.SchoolId, p.StudentId })
                    .HasForeignKey(d => new { d.SchoolId, d.StudentId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_randomizedstudents_student_id_students_id");
            });

            modelBuilder.Entity<RandomizedStudentsStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Assent)
                    .HasColumnName("assent")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomizedStudentId).HasColumnName("randomized_student_id");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RandomizedStudentTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomizedStudentId).HasColumnName("randomized_student_id");

                entity.Property(e => e.TrayId).HasColumnName("tray_id");

                entity.HasOne(d => d.RandomizedStudent)
                    .WithMany(p => p.RandomizedStudentTrays)
                    .HasForeignKey(d => d.RandomizedStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_randomizedstudenttrays_randomizedstudents_id");
            });

            modelBuilder.Entity<RandomizedStudentTraysStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomizedStudentId).HasColumnName("randomized_student_id");

                entity.Property(e => e.TrayId).HasColumnName("tray_id");
            });

            modelBuilder.Entity<ResearchTeamMembers>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Schools>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.Colors)
                    .HasColumnName("colors")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mascot)
                    .HasColumnName("mascot")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolLogo).HasColumnName("school_logo");

                entity.Property(e => e.SchoolTypeId).HasColumnName("school_type_id");

                entity.HasOne(d => d.SchoolType)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.SchoolTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_schools_school_type_id_school_types_id");
            });

            modelBuilder.Entity<SchoolTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Grades)
                    .IsRequired()
                    .HasColumnName("grades")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomSampleSize).HasColumnName("random_sample_size");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasIndex(e => new { e.SchoolId, e.StudentId })
                    .HasName("ix_students")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ethnicity)
                    .HasColumnName("ethnicity")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Grade).HasColumnName("grade");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PaidFreeReduced)
                    .HasColumnName("paid_free_reduced")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.Property(e => e.StudentId)
                    .IsRequired()
                    .HasColumnName("student_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_students_school_id_schools_id");
            });

            modelBuilder.Entity<TempAlphaTestWmtracking>(entity =>
            {
                entity.ToTable("TempAlphaTestWMTracking");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Idx).HasColumnName("idx");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Wm)
                    .HasColumnName("wm")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrayTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WeighingMeasurementImageMetadata>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImageMetadataId).HasColumnName("image_metadata_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Selected)
                    .IsRequired()
                    .HasColumnName("selected")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.Property(e => e.WeighingMeasurementId).HasColumnName("weighing_measurement_id");

                entity.HasOne(d => d.ImageMetadata)
                    .WithMany(p => p.WeighingMeasurementImageMetadata)
                    .HasForeignKey(d => d.ImageMetadataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementimagemetadata_image_metadata_id_imagemetadata_id");

                entity.HasOne(d => d.WeighingMeasurement)
                    .WithMany(p => p.WeighingMeasurementImageMetadata)
                    .HasForeignKey(d => d.WeighingMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementimagemetadata_weighing_measurement_id_weighingmeasurements_id");
            });

            modelBuilder.Entity<WeighingMeasurementMenuItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.MenuItemId).HasColumnName("menu_item_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Selected)
                    .IsRequired()
                    .HasColumnName("selected")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.WeighingMeasurementId).HasColumnName("weighing_measurement_id");

                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.WeighingMeasurementMenuItems)
                    .HasForeignKey(d => d.MenuItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementmenuitems_menu_item_id_menuitems_id");

                entity.HasOne(d => d.WeighingMeasurement)
                    .WithMany(p => p.WeighingMeasurementMenuItems)
                    .HasForeignKey(d => d.WeighingMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementmenuitems_weighing_measurement_id_weighingmeasurements_id");
            });

            modelBuilder.Entity<WeighingMeasurements>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ImageTypeId).HasColumnName("image_type_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Notes)
                    .HasColumnName("notes")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Tiebreaker)
                    .HasColumnName("tiebreaker")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WeighStationTypeId).HasColumnName("weigh_station_type_id");

                entity.Property(e => e.WeighingId).HasColumnName("weighing_id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.ImageType)
                    .WithMany(p => p.WeighingMeasurements)
                    .HasForeignKey(d => d.ImageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurements_image_type_id_imagetypes_id");

                entity.HasOne(d => d.WeighStationType)
                    .WithMany(p => p.WeighingMeasurements)
                    .HasForeignKey(d => d.WeighStationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurements_weigh_station_type_id_weighstationtypes_id");

                entity.HasOne(d => d.Weighing)
                    .WithMany(p => p.WeighingMeasurements)
                    .HasForeignKey(d => d.WeighingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurements_weighing_id_weighings_id");
            });

            modelBuilder.Entity<WeighingMeasurementTracking>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Info)
                    .IsRequired()
                    .HasColumnName("info")
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WeighingMeasurementId).HasColumnName("weighing_measurement_id");

                entity.HasOne(d => d.WeighingMeasurement)
                    .WithMany(p => p.WeighingMeasurementTracking)
                    .HasForeignKey(d => d.WeighingMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementtracking_weighing_measurement_id_weighingmeasurements_id");
            });

            modelBuilder.Entity<WeighingMeasurementTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.InterventionDayTrayTypeId).HasColumnName("intervention_day_tray_type_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.WeighingMeasurementId).HasColumnName("weighing_measurement_id");

                entity.HasOne(d => d.InterventionDayTrayType)
                    .WithMany(p => p.WeighingMeasurementTrays)
                    .HasForeignKey(d => d.InterventionDayTrayTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementtrays_intervention_day_tray_type_id_interventiondaytraytypes_id");

                entity.HasOne(d => d.WeighingMeasurement)
                    .WithMany(p => p.WeighingMeasurementTrays)
                    .HasForeignKey(d => d.WeighingMeasurementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementtrays_weighing_measurement_id_weighingmeasurements_id");
            });

            modelBuilder.Entity<WeighingMeasurmentGlobalInfoItems>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.GlobalInfoItemId).HasColumnName("global_info_item_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomizedStudentId).HasColumnName("randomized_student_id");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .IsUnicode(false);

                entity.HasOne(d => d.GlobalInfoItem)
                    .WithMany(p => p.WeighingMeasurmentGlobalInfoItems)
                    .HasForeignKey(d => d.GlobalInfoItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementglobalinfoitems_global_info_item_id_globalinfoitems_id");

                entity.HasOne(d => d.RandomizedStudent)
                    .WithMany(p => p.WeighingMeasurmentGlobalInfoItems)
                    .HasForeignKey(d => d.RandomizedStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingmeasurementglobalinfoitems_randomized_student_id_randomizedstudents_id");
            });

            modelBuilder.Entity<Weighings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Empty)
                    .HasColumnName("empty")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.Milk)
                    .HasColumnName("milk")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Multiple)
                    .HasColumnName("multiple")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Picture).HasColumnName("picture");

                entity.Property(e => e.SaladDressing)
                    .HasColumnName("salad_dressing")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Seconds)
                    .HasColumnName("seconds")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UniqueSituation)
                    .HasColumnName("unique_situation")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WeighStationTypeId).HasColumnName("weigh_station_type_id");

                entity.HasOne(d => d.InterventionDay)
                    .WithMany(p => p.Weighings)
                    .HasForeignKey(d => d.InterventionDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighings_intervention_day_id_interventiondays_id");

                entity.HasOne(d => d.WeighStationType)
                    .WithMany(p => p.Weighings)
                    .HasForeignKey(d => d.WeighStationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighings_weigh_station_type_id_weighstationtypes_id");
            });

            modelBuilder.Entity<WeighingsStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Empty)
                    .HasColumnName("empty")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InterventionDayId).HasColumnName("intervention_day_id");

                entity.Property(e => e.Milk)
                    .HasColumnName("milk")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Multiple)
                    .HasColumnName("multiple")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Picture).HasColumnName("picture");

                entity.Property(e => e.SaladDressing)
                    .HasColumnName("salad_dressing")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Seconds)
                    .HasColumnName("seconds")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UniqueSituation)
                    .HasColumnName("unique_situation")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.WeighStationTypeId).HasColumnName("weigh_station_type_id");

                entity.Property(e => e.WeighingId).HasColumnName("weighing_id");
            });

            modelBuilder.Entity<WeighingTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrayId).HasColumnName("tray_id");

                entity.Property(e => e.WeighingId).HasColumnName("weighing_id");

                entity.HasOne(d => d.Weighing)
                    .WithMany(p => p.WeighingTrays)
                    .HasForeignKey(d => d.WeighingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_weighingtrays_weighing_id_weighings_id");
            });

            modelBuilder.Entity<WeighingTraysStaging>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BatchId).HasColumnName("batch_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasColumnName("device_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TrayId).HasColumnName("tray_id");

                entity.Property(e => e.WeighingId).HasColumnName("weighing_id");
            });

            modelBuilder.Entity<WeighStationTypes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
