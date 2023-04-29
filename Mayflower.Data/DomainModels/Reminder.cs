using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class Reminder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly WhenToStart { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column("ReminderThemeId"), ForeignKey("_reminderTheme")]
        public ReminderStyle ReminderTheme { get; set; }

        public string? Description { get; set; }

        public int? TransactionToAccountId { get; set; }

        public int? TransactionFromAccountId { get; set; }

        [Column("RecurrenceThemeId"), ForeignKey("_recurrenceTheme")]
        public RecurrenceStyle RecurrenceTheme { get; set; }

        #region Navigation
        public virtual FinancialAccount? TransactionToAccount { get; set; }

        public virtual FinancialAccount? TransactionFromAccount { get; set; }

        public virtual ReminderTheme? _reminderTheme { get; set; }

        public virtual RecurrenceTheme? _recurrenceTheme { get; set; }
        #endregion
    }
}