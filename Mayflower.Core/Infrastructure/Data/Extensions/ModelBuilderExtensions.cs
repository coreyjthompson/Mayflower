using Mayflower.Core.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Data.Extensions
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


            // Seed financial institutions
            var financialInstitution = new FinancialInstitution { Id = 1, Name = "Ally Bank", NickName = "Ally" };
            builder.Entity<FinancialInstitution>().HasData(financialInstitution);

            // Seed financial accounts
            builder.Entity<FinancialAccount>().HasData(
                new FinancialAccount { Id = 1, Number = "1032497347", FinancialAccountTheme = FinancialAccountStyle.Checking, Nickname = "Primary Checking", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 2, Number = "1054452618", FinancialAccountTheme = FinancialAccountStyle.Checking, Nickname = "Bill Pay Account", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 3, Number = "2132880598", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Primary Savings", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 4, Number = "2133086542", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Xmas Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 5, Number = "2141652087", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Me-want Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 6, Number = "2144275472", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Birthday Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 7, Number = "2144275977", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Graces Emergency Fund", FinancialInstitutionId = financialInstitution.Id },
                new FinancialAccount { Id = 8, Number = "2148132695", FinancialAccountTheme = FinancialAccountStyle.Savings, Nickname = "Landscaping Fund", FinancialInstitutionId = financialInstitution.Id }
            );


            // Seed reminders
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            builder.Entity<Reminder>().HasData(
                new Reminder
                {
                    Id = 1,
                    Amount = 645.33m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/10/2023"),
                    Description = "Lakeview Mortgage Payment",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 10,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 2,
                    Amount = 400m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/7/2023"),
                    Description = "Old National Car Payment",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 7,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 3,
                    Amount = 120m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/2/2023"),
                    Description = "Synchrony/CareCredit Vet Card",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 2,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 4,
                    Amount = 71m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/24/2023"),
                    Description = "Astound Broadband Internet",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 24,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 5,
                    Amount = 12.95m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/8/2023"),
                    Description = "Walmart +",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 8,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 6,
                    Amount = 1.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/7/2023"),
                    Description = "Google Cloud Storage",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 7,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 7,
                    Amount = 27.81m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("4/28/2023"),
                    Description = "Pretty Litter",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Weekly,
                    RecurrenceInterval = 3,
                    RecurrenceDayOfMonth = null,
                    RecurrenceOrdinal = RecurrencePosition.None,
                    RecurrenceDayOfWeek = DayOfWeek.Friday
                },
                new Reminder
                {
                    Id = 8,
                    Amount = 19.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/7/2023"),
                    Description = "Netflix",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 7,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 9,
                    Amount = 9.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/10/2023"),
                    Description = "Paramount +",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 10,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 10,
                    Amount = 60m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/15/2023"),
                    Description = "Google Fi Mobile Phone",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 15,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 11,
                    Amount = 15.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/16/2023"),
                    Description = "HBO Max",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 16,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 12,
                    Amount = 15.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/17/2023"),
                    Description = "Amazon Music",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 17,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 13,
                    Amount = 152.08m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/25/2023"),
                    Description = "EnerBank/Regions HVAC Payment",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 25,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 14,
                    Amount = 4.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/26/2023"),
                    Description = "Peacock",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 26,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 15,
                    Amount = 10.99m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/26/2023"),
                    Description = "Disney +",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 26,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 16,
                    Amount = 205.89m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/28/2023"),
                    Description = "Nelnet Student Loan",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 28,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 17,
                    Amount = 100m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/20/2023"),
                    Description = "Chandler Water",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 20,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 18,
                    Amount = 250m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("5/28/2023"),
                    Description = "Centerpoint Gas & Electric",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 28,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 19,
                    Amount = 50m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("4/30/2023"),
                    Description = "Cricket Wireless",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 30,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 20,
                    Amount = 91.50m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("4/30/2023"),
                    Description = "Root Car Insurance",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 1,
                    RecurrenceDayOfMonth = 30,
                    RecurrenceOrdinal = RecurrencePosition.None,
                },
                new Reminder
                {
                    Id = 21,
                    Amount = 95.00m,
                    TransactionFromAccountId = 2,
                    WhenBegins = DateOnly.Parse("4/30/2023"),
                    Description = "Waste Management Trash",
                    ReminderTheme = ReminderStyle.Bill,
                    RecurrenceTheme = RecurrenceStyle.Monthly,
                    RecurrenceInterval = 3,
                    RecurrenceDayOfMonth = 30,
                    RecurrenceOrdinal = RecurrencePosition.None,
                }
            );
        }
    }


}
