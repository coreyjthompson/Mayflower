using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class InactiveReminderReason
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public InactiveReminderCause Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "Unknown";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "Unknown";

        public InactiveReminderReason() { }

        public InactiveReminderReason(InactiveReminderCause style)
        {
            Id = style;
            Value = style.ToName() ?? "Unknown";
            Name = style.ToDescription() ?? "Unknown";
        }
    }

    public enum InactiveReminderCause : int
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Completion")]
        Skipped = 1,
        [Description("Deleted")]
        Complete = 2
    }
}