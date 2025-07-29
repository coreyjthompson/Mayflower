using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class Reminder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly WhenBegins { get; set; }

        public DateOnly? WhenExpires { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column("ReminderThemeId"), ForeignKey("_reminderTheme")]
        public ReminderStyle ReminderTheme { get; set; }

        public string? Description { get; set; }

        public int? TransactionToAccountId { get; set; }

        public int? TransactionFromAccountId { get; set; }

        [Column("RecurrenceThemeId"), ForeignKey("_recurrenceTheme")]
        public RecurrenceStyle RecurrenceTheme { get; set; }

        public int RecurrenceInterval { get; set; }

        public int? RecurrenceDayOfMonth { get; set; }

        [Column("RecurrenceDayOfWeekId"), ForeignKey("_recurrenceDayOfWeek")]
        public DayOfWeek? RecurrenceDayOfWeek { get; set; }

        [Column("RecurrenceOrdinalId"), ForeignKey("_recurrenceOrdinal")]
        public RecurrencePosition RecurrenceOrdinal { get; set; }

        [Column("InactiveReasonId"), ForeignKey("_inactiveReason")]
        public InactiveReminderCause? InactiveReason { get; set; }

        #region Navigation
        public virtual FinancialAccount? TransactionToAccount { get; set; }

        public virtual FinancialAccount? TransactionFromAccount { get; set; }

        public virtual IList<ReminderOccurrence>? Occurrences { get; set; }

        public virtual ReminderTheme? _reminderTheme { get; set; }

        public virtual RecurrenceTheme? _recurrenceTheme { get; set; }

        public virtual RecurrenceDayOfWeek? _recurrenceDayOfWeek { get; set; }

        public virtual RecurrenceOrdinal? _recurrenceOrdinal { get; set; }

        public virtual InactiveReminderReason? _inactiveReason { get; set; }

        #endregion
    }
}