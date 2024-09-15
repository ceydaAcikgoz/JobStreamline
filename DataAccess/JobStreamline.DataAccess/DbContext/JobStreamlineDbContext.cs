using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using JobStreamline.Entity;
using Microsoft.EntityFrameworkCore;
using JobStreamline.Entity.Enum;

namespace JobStreamline.DataAccess
{

    public class JobStreamlineDbContext : DbContext
    {
        private IConfiguration _iConfiguration;

        public JobStreamlineDbContext(IConfiguration Configuration)  {
            this._iConfiguration = Configuration;
         }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_iConfiguration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("JobStreamline.Api"));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Company>()
                .Property(c => c.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .Property(c => c.Address)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .Property(c => c.ContactEmail)
                .HasMaxLength(50);

            modelBuilder.Entity<Company>()
                .Property(c => c.CompanySize)
                .IsRequired();

            modelBuilder.Entity<Company>()
                .Property(c => c.JobPostingLimit)
                .HasDefaultValue(2);


            // Company ve Job ilişkisi: Bir Company'nin birçok Job'u olabilir
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Jobs)
                .WithOne(j => j.Company)
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);  // Şirket silinirse ilanları da silinir



            // Job validasyon ve kuralları
            modelBuilder.Entity<Job>()
                .Property(j => j.Position)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Job>()
                .Property(j => j.Description)
                .IsRequired();

            modelBuilder.Entity<Job>()
                .Property(j => j.Benefits)
                .HasMaxLength(500);

            modelBuilder.Entity<Job>()
                .Property(j => j.WorkType)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Job>()
                .Property(j => j.Salary)
                .HasMaxLength(50);

            modelBuilder.Entity<Job>()
                .Property(j => j.Currency)
                .HasConversion<string>();

            modelBuilder.Entity<Job>()
                .Property(j => j.Location)
                .HasMaxLength(100);

            modelBuilder.Entity<Job>()
                .Property(j => j.Qualifications)
                .HasMaxLength(500);

            modelBuilder.Entity<Job>()
                .Property(j => j.Status)
                .HasConversion<string>()
                .HasDefaultValue(JobStatus.Active);

            // Varsayılan değerler
            modelBuilder.Entity<Job>()
                .Property(j => j.CreatedDate)
                .HasDefaultValueSql("CURRENT_DATE");  // İlan oluşturulma tarihi varsayılan olarak bugünkü tarih

            modelBuilder.Entity<Job>()
                .Property(j => j.ExpiryDate)
                .HasDefaultValueSql("CURRENT_DATE + INTERVAL '15 day'");  // İlan yayında kalma süresi 15 gün
        }
    }
}