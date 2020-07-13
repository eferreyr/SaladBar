using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data.Models
{
    public partial class SaladBarContext : DbContext
    {
        public virtual DbSet<Audits> Audits { get; set; }
        public virtual DbSet<Devices> Devices { get; set; }
        public virtual DbSet<InterventionDays> InterventionDays { get; set; }
        public virtual DbSet<InterventionTrays> InterventionTrays { get; set; }
        public virtual DbSet<ResearchTeamMembers> ResearchTeamMembers { get; set; }
        public virtual DbSet<Schools> Schools { get; set; }

        public virtual DbSet<Weighings> Weighings { get; set; }
        public virtual DbSet<WeighingTrays> WeighingTrays { get; set; }
        public virtual DbSet<WeighStationTypes> WeighStationTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.;Database=SaladBar;uid=saladbar_app;password=password;");

                /*
                 * var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

                  optionsBuilder.UseSqlServer(builder["ConnectionString"]);
                */

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SchoolTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .IsRequired();

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Grades)
                    .HasColumnName("grades")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RandomSampleSize)
                    .HasColumnName("random_sample_size")
                    .IsRequired();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('system')");

                entity.Property(e => e.DtCreated)
                    .HasColumnName("dt_created")
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DtModified)
                    .HasColumnName("dt_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasMany(e => e.Schools)
                    .WithOne(e => e.SchoolType)
                    .HasForeignKey(f => f.SchoolTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
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

                entity.Property(e => e.Dirty)
                    .HasColumnName("dirty")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

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

            modelBuilder.Entity<Devices>(entity =>
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

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasColumnName("device_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Dirty)
                    .IsRequired()
                    .HasColumnName("dirty")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Y')");

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

            modelBuilder.Entity<InterventionDays>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

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

                entity.Property(e => e.SchoolId).HasColumnName("school_id");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.InterventionDays)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_interventiondays_school_id_schools_id");
            });

            modelBuilder.Entity<InterventionTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dirty)
                    .HasColumnName("dirty")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

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

            modelBuilder.Entity<ResearchTeamMembers>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('N')");

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
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

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

                entity.Property(e => e.SchoolLogo)
                    .HasColumnName("school_logo");

                entity.Property(e => e.SchoolTypeId)
                    .HasColumnName("school_type_id")
                    .IsRequired();

                entity.HasOne(d => d.SchoolType)
                    .WithMany(p => p.Schools)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_schools_schooltypeid_schooltypes_id");
            });

            modelBuilder.Entity<Weighings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dirty)
                    .HasColumnName("dirty")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

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

                entity.Property(e => e.Milk)
                    .HasColumnName("milk")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Picture).HasColumnName("picture");

                entity.Property(e => e.SaladDressing)
                    .HasColumnName("salad_dressing")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UniqueSituation)
                    .HasColumnName("unique_situation")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Empty)
                    .HasColumnName("empty")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Multiple)
                    .HasColumnName("multiple")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Seconds)
                    .HasColumnName("seconds")
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

            modelBuilder.Entity<WeighingTrays>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dirty)
                    .HasColumnName("dirty")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("Y");

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

            modelBuilder.Entity<WeighStationTypes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

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
