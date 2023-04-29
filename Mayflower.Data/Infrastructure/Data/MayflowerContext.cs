using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Data
{
    public class MayflowerContext : DbContext
    {
        public MayflowerContext(DbContextOptions<MayflowerContext> options) : base(options)
        {
        }

        public DbSet<TransactionTheme> TransactionThemes { get; set; }
        public DbSet<FinancialAccountTheme> FinancialAccountThemes { get; set; }
        public DbSet<FinancialAccount> TransactionAccounts { get; set; }
        public DbSet<ReminderTheme> ReminderThemes { get; set; }
        public DbSet<RecurrenceOrdinal> RecurrenceOrdinals { get; set; }
        public DbSet<RecurrenceTheme> RecurrenceThemes { get; set; }
        public DbSet<RecurrenceDayOfWeek> RecurrenceDaysOfWeek { get; set; }
        public DbSet<Reminder> Reminders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make table names singular
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType == typeof(Reminder))
                {
                    modelBuilder.Entity<Reminder>(builder =>
                    {
                        // Date is a DateOnly property and date on database
                        builder.Property(p => p.WhenToStart).HasConversion<DateOnlyConverter, DateOnlyComparer>();
                    });

                    modelBuilder.Entity<Reminder>().ToTable(entityType.ClrType.Name);
                }
                else
                {
                    modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
                }
            }

            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}
