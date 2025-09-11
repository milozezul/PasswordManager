using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PManager.Models.Configs;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

    public class Password
    {
        public int Id { get; set; }
        public byte[] Value { get; set; }

    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class RecordPasswords
    {
        public int PasswordId { get; set; }
        public int RecordId { get; set; }

        public Password Password { get; set; }
        public Record Record { get; set; }

    }
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Category Category { get; set; }
    }
}
