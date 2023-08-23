using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mayflower.Core.DomainModels
{
    public class ReminderOccurrence
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateOnly WhenOriginallyScheduledToOccur { get; set; }

        public DateOnly? WhenRescheduledToOccur { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Amount { get; set; }

        public string? Description { get; set; }

        [Column("OccurrenceReasonId"), ForeignKey("_occurrenceReason")]
        public ReminderOccurrenceCause ReasonForOccurrence { get; set; }

        public int ReminderId { get; set; }

        public DateTimeOffset WhenCreated { get; set; }

        #region Navigation
        public virtual Reminder Reminder { get; set; } = default!;

        public virtual ReminderOccurrenceReason? _occurrenceReason { get; set; }
        #endregion
    }
}
