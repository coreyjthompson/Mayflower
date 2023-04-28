using Mayflower.Data.Helpers;
using Mayflower.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Data
{
    public class MayflowerContext : DbContext
    {
        public MayflowerContext(DbContextOptions<MayflowerContext> options) : base(options)
        {
        }

        public DbSet<TransactionTheme> TransactionThemes { get; set; }
        public DbSet<TransactionAccountTheme> TransactionAccountThemes { get; set; }
        public DbSet<TransactionAccount> TransactionAccounts { get; set; }
        public DbSet<ReminderTheme> ReminderThemes { get; set; }
        public DbSet<RecurrenceOrdinal> RecurrenceOrdinals { get; set; }
        public DbSet<RecurrenceTheme> RecurrenceThemes { get; set; }
        public DbSet<RecurrenceDayofWeek> RecurrenceDaysofWeek { get; set; }
        public DbSet<Reminder> Reminders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make table names singular
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if(entityType.ClrType == typeof(Reminder))
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
