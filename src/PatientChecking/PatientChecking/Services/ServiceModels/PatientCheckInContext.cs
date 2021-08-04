using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PatientChecking.Services.ServiceModels
{
    public partial class PatientCheckInContext : DbContext
    {
        public PatientCheckInContext()
        {
        }

        public PatientCheckInContext(DbContextOptions<PatientCheckInContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer("Data Source=5CD1199VDQ;Initial Catalog=PatientCheckIn;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("Address");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Address__Contact__398D8EEE");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.MedicalConcerns).HasMaxLength(100);

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appointme__Patie__33D4B598");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Contact__Patient__36B12243");
            });

            modelBuilder.Entity<EmergencyContact>(entity =>
            {
                entity.HasKey(e => e.EmergencyId)
                    .HasName("PK__Emergenc__7B5544337DE6E6BE");

                entity.ToTable("EmergencyContact");

                entity.Property(e => e.EmergencyId).HasColumnName("EmergencyID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Relationship)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.EmergencyContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Emergency__Conta__3C69FB99");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.BirthplaceCity).HasMaxLength(50);

                entity.Property(e => e.DoB).HasColumnType("date");

                entity.Property(e => e.EthnicRace).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.HomeTown).HasMaxLength(50);

                entity.Property(e => e.IdcardNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IDCardNo");

                entity.Property(e => e.InsuranceNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IssuedDate).HasColumnType("date");

                entity.Property(e => e.IssuedPlace).HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.Nationality)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PatientIdentifier)
                    .IsRequired()
                    .HasMaxLength(8);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
