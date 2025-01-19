using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class RecurrenceTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public RecurrenceStyle Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = string.Empty;

        public RecurrenceTheme() { }

        public RecurrenceTheme(RecurrenceStyle style)
        {
            Id = style;
            Name = style.ToName();
            Description = style.ToDescription();
        }
    }

    public enum RecurrenceStyle : int
    {
        [Description("One-time payment")]
        NoRecurrence = 0,
        [Description("Daily")]
        Day = 1,
        [Description("Weekly")]
        Week = 2,
        [Description("Monthly")]
        Month = 3,
        [Description("Yearly")]
        Year = 4
    }
}
