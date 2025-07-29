using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data.Extensions;
using Mayflower.Core.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mayflower.Core.Infrastructure.Data
{
    public class MayflowerContext : DbContext
    {
        public MayflowerContext(DbContextOptions<MayflowerContext> options) : base(options)
        {
        }

        public DbSet<FinancialTransaction> Transactions { get; set; }
        public DbSet<TransactionTheme> TransactionThemes { get; set; }
        public DbSet<FinancialAccountTheme> FinancialAccountThemes { get; set; }
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public DbSet<ReminderTheme> ReminderThemes { get; set; }
        public DbSet<RecurrenceOrdinal> RecurrenceOrdinals { get; set; }
        public DbSet<RecurrenceTheme> RecurrenceThemes { get; set; }
        public DbSet<RecurrenceDayOfWeek> RecurrenceDaysOfWeek { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<ReminderOccurrenceReason> ReminderActions { get; set; }
        public DbSet<ReminderOccurrence> ReminderOccurrences { get; set; }
        public DbSet<InactiveReminderReason> InactiveReminderReasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType == typeof(Reminder))
                {
                    BuildReminderModel(ref modelBuilder, entityType);
                }
                else if (entityType.ClrType == typeof(FinancialTransaction))
                {
                    BuildFinancialTransactionModel(ref modelBuilder, entityType);
                }
                else if (entityType.ClrType == typeof(ReminderOccurrence))
                {
                    BuildReminderOccurrenceModel(ref modelBuilder, entityType);
                }

                // Make table names singular by using class name instead of dbset
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }

            // Run the initial seed data to populate the db
            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }

        private void BuildReminderModel(ref ModelBuilder modelBuilder, IMutableEntityType entityType)
        {
            modelBuilder.Entity<Reminder>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(r => r.WhenBegins).HasConversion<DateOnlyConverter, DateOnlyComparer>();
                builder.Property(r => r.WhenExpires).HasConversion<DateOnlyConverter, DateOnlyComparer>();
            });

        }

        private void BuildFinancialTransactionModel(ref ModelBuilder modelBuilder, IMutableEntityType entityType)
        {
            modelBuilder.Entity<FinancialTransaction>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(f => f.WhenPosted).HasConversion<DateOnlyConverter, DateOnlyComparer>();
            });
        }

        private void BuildReminderOccurrenceModel(ref ModelBuilder modelBuilder, IMutableEntityType entityType)
        {
            modelBuilder.Entity<ReminderOccurrence>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(r => r.WhenOriginallyScheduledToOccur).HasConversion<DateOnlyConverter, DateOnlyComparer>();
                builder.Property(r => r.WhenRescheduledToOccur).HasConversion<DateOnlyConverter, DateOnlyComparer>();
            });

            modelBuilder.Entity<ReminderOccurrence>()
                .HasOne(r => r.Reminder) // reference
                .WithMany(r => r.Occurrences) // collection
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
