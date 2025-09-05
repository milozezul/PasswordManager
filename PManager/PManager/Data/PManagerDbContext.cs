using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PManager.Models.Configs;

namespace PManager.Data
{
    public class PManagerDbContext: DbContext
    {
        private readonly SQLConfigs _config;
        public PManagerDbContext(IOptions<SQLConfigs> config)
        {
            _config = config.Value;
        }
        public PManagerDbContext(DbContextOptions<PManagerDbContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.ConnectionString);
        }

        public DbSet<Passwords> Passwords { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<PasswordValues> PasswordValues { get; set; }
        public DbSet<Values> Values { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var passwords = modelBuilder.Entity<Passwords>();
            passwords.HasKey(item => new { item.Id });
            passwords.HasOne(p => p.Category);
            passwords.Property(p => p.Name).HasMaxLength(300);

            var categories = modelBuilder.Entity<Categories>();
            categories.HasKey(c => c.Id);
            categories.Property(c => c.Name).HasMaxLength(200);

            var passwordValues = modelBuilder.Entity<PasswordValues>();
            passwordValues.HasKey(pv => new { pv.ValueId, pv.PasswordId });
            passwordValues.HasOne(pv => pv.Password).WithMany().HasForeignKey(pv => pv.PasswordId);
            passwordValues.HasOne(v => v.Value).WithMany().HasForeignKey(v => v.ValueId);
            
            var values = modelBuilder.Entity<Values>();
            values.HasKey(v => v.Id);
            values.Property(v => v.Value).HasMaxLength(1000);
        }
    }

    public class Passwords
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Categories Category { get; set; }

    }

    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class PasswordValues
    {
        public int PasswordId { get; set; }
        public int ValueId { get; set; }

        public Passwords Password { get; set; }
        public Values Value { get; set; }

    }
    public class Values
    {
        public int Id { get; set; }
        public int Subid { get; set; }
        public byte[] Value { get; set; }
    }
}
