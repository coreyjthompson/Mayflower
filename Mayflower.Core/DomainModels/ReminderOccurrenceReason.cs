using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class ReminderOccurrenceReason
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ReminderOccurrenceCause Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "Unknown";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "Unknown";

        public ReminderOccurrenceReason() { }

        public ReminderOccurrenceReason(ReminderOccurrenceCause style)
        {
            Id = style;
            Value = style.ToName() ?? "Unknown";
            Name = style.ToDescription() ?? "Unknown";
        }
    }

    public enum ReminderOccurrenceCause : int
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Skip")]
        Skip = 1,
        [Description("Completion")]
        Completion = 2,
        [Description("Edit")]
        Edit = 3
    }
}