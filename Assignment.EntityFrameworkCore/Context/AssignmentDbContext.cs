using Assignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Assignment.EntityFrameworkCore.Context
{
    public class AssignmentDbContext : DbContext
    {
        public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options) : base(options)
        {

        }
        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public virtual async Task CommitAsync()
        {
            _ = await base.SaveChangesAsync().ConfigureAwait(true);
        }

        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasMaxLength(20);
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.CurrencyCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(8);
                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            });
        }
    }

    public class DatacontextFactory : IDesignTimeDbContextFactory<AssignmentDbContext>
    {
        public AssignmentDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<AssignmentDbContext>();

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            optionBuilder.UseSqlServer(configuration["ConnectionStrings:AssignmentDbContext"]);

            return new AssignmentDbContext(optionBuilder.Options);
        }
    }
}