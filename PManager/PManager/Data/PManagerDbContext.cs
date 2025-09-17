using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PManager.Models.Configs;
using PManager.Models.Database;
using SharedModels.Database;

namespace PManager.Data
{
    public class PManagerDbContext: DbContext
    {
        private readonly SQLConfigs _config;
        public PManagerDbContext(DbContextOptions<PManagerDbContext> options, IOptions<SQLConfigs> config) : base(options)
        {
            _config = config.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.ConnectionString);
        }

        public DbSet<Password> Passwords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecordPasswords> RecordPasswords { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRecord> UserRecords { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var users = modelBuilder
                .Entity<User>();
            users
                .HasKey(u => u.Id);
            users
                .Property(u => u.Username)
                .HasMaxLength(200);
            users
                .HasIndex(u => u.Username)
                .IsUnique();
            users
                .Property(u => u.PasswordHash)
                .HasMaxLength(200);

            var userrecords = modelBuilder
                .Entity<UserRecord>();
            userrecords
                .HasKey(ur => new { ur.UserId, ur.RecordId });
            userrecords
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId);
            userrecords
                .HasOne(ur => ur.Record)
                .WithMany()
                .HasForeignKey(ur => ur.RecordId);

            var usercategories = modelBuilder
                .Entity<UserCategory>();
            usercategories
                .HasKey(uc => new { uc.UserId, uc.CategoryId });
            usercategories
                .HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId);
            usercategories
                .HasOne(uc => uc.Category)
                .WithMany()
                .HasForeignKey(uc => uc.CategoryId);

            var passwords = modelBuilder
                .Entity<Password>();
            passwords
                .HasKey(item => new { item.Id });                
            passwords
                .Property(p => p.Value)
                .HasMaxLength(1000);

            var categories = modelBuilder
                .Entity<Category>();
            categories
                .HasKey(c => c.Id);
            categories
                .HasIndex(c => c.Name)
                .IsUnique();
            categories
                .Property(c => c.Name)
                .HasMaxLength(200);

            var recordPasswords = modelBuilder
                .Entity<RecordPasswords>();
            recordPasswords
                .HasKey(pv => new { 
                    pv.RecordId, 
                    pv.PasswordId 
                });
            recordPasswords
                .HasOne(pv => pv.Record)
                .WithMany()
                .HasForeignKey(pv => pv.RecordId);
            recordPasswords
                .HasOne(v => v.Password)
                .WithMany()
                .HasForeignKey(v => v.PasswordId);
            
            var record = modelBuilder
                .Entity<Record>();
            record
                .HasKey(v => v.Id);
            record
                .Property(v => v.Name)
                .HasMaxLength(200);
            record
                .Property(v => v.Url)
                .HasMaxLength(300);
        }
    }
}
