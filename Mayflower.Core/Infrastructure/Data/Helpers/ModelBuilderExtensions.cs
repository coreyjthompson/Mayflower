using Mayflower.Core.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Data.Helpers
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


            // Seed financial institutions
            var financialInstitution = new FinancialInstitution { Id = 1, Name = "Ally Bank", NickName = "Ally" };
            builder.Entity<FinancialInstitution>().HasData(financialInstitution);

            // Seed financial accounts
            builder.Entity<FinancialAccount>().HasData(
                new FinancialAccount { Id = 1, Number = "1032497347", FinancialAccountTheme = FinancialAccountStyle.Checking, NickName = "Primary Checking", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 2, Number = "1054452618", FinancialAccountTheme = FinancialAccountStyle.Checking, NickName = "Bill Pay Account", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 3, Number = "2132880598", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Primary Savings", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 4, Number = "2133086542", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Xmas Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 5, Number = "2141652087", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Me-want Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 6, Number = "2144275472", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Birthday Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 7, Number = "2144275977", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Graces Emergency Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 8, Number = "2148132695", FinancialAccountTheme = FinancialAccountStyle.Savings, NickName = "Landscaping Fund", FinancialInstitutionId = financialInstitution.Id }

            );


            // Seed reminders
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            builder.Entity<Reminder>().HasData(
                new Reminder { Id = 1, Amount = 2800.00m, TransactionToAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "iMedia paycheck deposit", ReminderTheme = ReminderStyle.Income },
                new Reminder { Id = 2, Amount = 2800.00m, TransactionToAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "iMedia paycheck deposit", ReminderTheme = ReminderStyle.Income },
                new Reminder { Id = 3, Amount = 76.25m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "water bill", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 4, Amount = 650.00m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "mortage payment", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 5, Amount = 225.00m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "vectren bill", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 6, Amount = 235.56m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "car payment", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 7, Amount = 18.98m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(7), Description = "Netflix", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 8, Amount = 50m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "Fuel", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 9, Amount = 50m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "Fuel", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 10, Amount = 100m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "Cigarettes", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 11, Amount = 100m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "Cigarettes", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 12, Amount = 120m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "Smoke", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 13, Amount = 120m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "Smoke", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 14, Amount = 90m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "Child support", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 15, Amount = 90m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(9), Description = "Child support", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 16, Amount = 90m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "Child support", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 17, Amount = 90m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(23), Description = "Child support", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 18, Amount = 2.95m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(2), Description = "Child support fee", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 19, Amount = 2.95m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(9), Description = "Child support fee", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 20, Amount = 2.95m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(16), Description = "Child support fee", ReminderTheme = ReminderStyle.Bill },
                new Reminder { Id = 21, Amount = 2.95m, TransactionFromAccountId = 1, WhenToStart = currentDate.AddDays(23), Description = "Child support fee", ReminderTheme = ReminderStyle.Bill },
                new Reminder
                {
                    Id = 22,
                    Amount = 1200.00m,
                    TransactionFromAccountId = 3,
                    TransactionToAccountId = 1,
                    WhenToStart = currentDate.AddDays(14),
                    Description = "Transfer from Primary Savings|Transfer to Primary Checking",
                    ReminderTheme = ReminderStyle.Transfer
                },
                new Reminder
                {
                    Id = 23,
                    Amount = 100.00m,
                    TransactionFromAccountId = 1,
                    TransactionToAccountId = 4,
                    WhenToStart = currentDate.AddDays(16),
                    Description = "Transfer from Primary Checking|Transfer to Xmas Fund",
                    ReminderTheme = ReminderStyle.Transfer
                },
                new Reminder
                {
                    Id = 24,
                    Amount = 1200.00m,
                    TransactionFromAccountId = 1,
                    TransactionToAccountId = 7,
                    WhenToStart = currentDate.AddDays(16),
                    Description = "Transfer from Primary Checking|Transfer to Grace's Emergency Fund",
                    ReminderTheme = ReminderStyle.Transfer
                }

            );



        }
    }


}
