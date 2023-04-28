using Mayflower.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Data
{
    public static class ModelBuilderExtensions
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

            DomainModels.DayOfWeek[] daysOfWeek = (DomainModels.DayOfWeek[])Enum.GetValues(typeof(DomainModels.DayOfWeek));
            IList<RecurrenceDayOfWeek> recurrenceDaysOfWeek = new List<RecurrenceDayOfWeek>();

            foreach (var day in daysOfWeek)
            {
                recurrenceDaysOfWeek.Add(new RecurrenceDayOfWeek(day));
            }

            builder.Entity<RecurrenceDayOfWeek>().HasData(recurrenceDaysOfWeek.ToArray());

            // Seed transaction accounts
            builder.Entity<FinancialAccount>().HasData(
                                new FinancialAccount { Id = 1, Number = "1032497347", Type = FinancialAccountStyle.Checking, NickName = "Primary Checking", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 2, Number = "1054452618", Type = FinancialAccountStyle.Checking, NickName = "Bill Pay Account", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 3, Number = "2132880598", Type = FinancialAccountStyle.Savings, NickName = "Primary Savings", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 4, Number = "2133086542", Type = FinancialAccountStyle.Savings, NickName = "Xmas Fund", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 5, Number = "2141652087", Type = FinancialAccountStyle.Savings, NickName = "Me-want Fund", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 6, Number = "2144275472", Type = FinancialAccountStyle.Savings, NickName = "Birthday Fund", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 7, Number = "2144275977", Type = FinancialAccountStyle.Savings, NickName = "Graces Emergency Fund", AccountVendor = "Ally Bank" },
                                new FinancialAccount { Id = 8, Number = "2148132695", Type = FinancialAccountStyle.Savings, NickName = "Landscaping Fund", AccountVendor = "Ally Bank" }

            );
        }
    }


}
