using Mayflower.Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Data.Extensions
{
    public static partial class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            // Seed the enum/lookup tables
            TransactionStyle[] transactionStyles = (TransactionStyle[])Enum.GetValues(typeof(TransactionStyle));
            IList<TransactionTheme> transactionThemes = new List<TransactionTheme>();

            foreach (var style in transactionStyles)
            {
                transactionThemes.Add(new TransactionTheme(style));
            }

            builder.Entity<TransactionTheme>().HasData(transactionThemes.ToArray());

            FinancialAccountStyle[] transactionAccountStyles = (FinancialAccountStyle[])Enum.GetValues(typeof(FinancialAccountStyle));
            IList<FinancialAccountTheme> transactionAccountThemes = new List<FinancialAccountTheme>();

            foreach (var style in transactionAccountStyles)
            {
                transactionAccountThemes.Add(new FinancialAccountTheme(style));
            }

            builder.Entity<FinancialAccountTheme>().HasData(transactionAccountThemes.ToArray());

            ReminderStyle[] reminderStyles = (ReminderStyle[])Enum.GetValues(typeof(ReminderStyle));
            IList<ReminderTheme> reminderThemes = new List<ReminderTheme>();

            foreach (var style in reminderStyles)
            {
                reminderThemes.Add(new ReminderTheme(style));
            }

            builder.Entity<ReminderTheme>().HasData(reminderThemes.ToArray());

            ReminderOccurrenceCause[] occurenceCauses = (ReminderOccurrenceCause[])Enum.GetValues(typeof(ReminderOccurrenceCause));
            IList<ReminderOccurrenceReason> occurrenceReasons = new List<ReminderOccurrenceReason>();

            foreach (var style in occurenceCauses)
            {
                occurrenceReasons.Add(new ReminderOccurrenceReason(style));
            }

            builder.Entity<ReminderOccurrenceReason>().HasData(occurrenceReasons.ToArray());

            InactiveReminderCause[] inactiveCauses = (InactiveReminderCause[])Enum.GetValues(typeof(InactiveReminderCause));
            IList<InactiveReminderReason> inactiveReasons = new List<InactiveReminderReason>();

            foreach (var style in inactiveCauses)
            {
                inactiveReasons.Add(new InactiveReminderReason(style));
            }

            builder.Entity<InactiveReminderReason>().HasData(inactiveReasons.ToArray());

            RecurrencePosition[] recurrencePositions = (RecurrencePosition[])Enum.GetValues(typeof(RecurrencePosition));
            IList<RecurrenceOrdinal> recurrenceOrdinals = new List<RecurrenceOrdinal>();

            foreach (var position in recurrencePositions)
            {
                recurrenceOrdinals.Add(new RecurrenceOrdinal(position));
            }

            builder.Entity<RecurrenceOrdinal>().HasData(recurrenceOrdinals.ToArray());

            RecurrenceStyle[] recurrenceStyles = (RecurrenceStyle[])Enum.GetValues(typeof(RecurrenceStyle));
            IList<RecurrenceTheme> recurrenceThemes = new List<RecurrenceTheme>();

            foreach (var style in recurrenceStyles)
            {
                recurrenceThemes.Add(new RecurrenceTheme(style));
            }

            builder.Entity<RecurrenceTheme>().HasData(recurrenceThemes.ToArray());

            DayOfWeek[] daysOfWeek = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));
            IList<RecurrenceDayOfWeek> recurrenceDaysOfWeek = new List<RecurrenceDayOfWeek>();

            foreach (var day in daysOfWeek)
            {
                recurrenceDaysOfWeek.Add(new RecurrenceDayOfWeek(day));
            }

            builder.Entity<RecurrenceDayOfWeek>().HasData(recurrenceDaysOfWeek.ToArray());

        }
    }


}
