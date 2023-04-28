using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class Reminder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }

        [ForeignKey("TransactionAccount"), Required]
        public int TransactionAccountId { get; set; }

        [Required]
        public DateOnly WhenToStart { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column("ReminderThemeId"), ForeignKey("_reminderTheme"), Required]
        public ReminderStyle Theme { get; set; }

        public string? Description { get; set; }

        public string? PayTo { get; set; }

        public string? PayFrom { get; set; }

        [Column("RecurrenceThemeId"), ForeignKey("_recurrenceTheme"), Required]
        public RecurrenceStyle RecurrenceTheme { get; set; }

        #region Navigation
        public virtual TransactionAccount TransactionAccount { get; set; } = new TransactionAccount();

        public virtual ReminderTheme _reminderTheme { get; set; } = new ReminderTheme();

        public virtual RecurrenceTheme _recurrenceTheme { get; set; } = new RecurrenceTheme();
        #endregion
    }
}