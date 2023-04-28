using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MEI.Core.Helpers;

namespace Mayflower.DomainModels
{
    public class RecurrenceTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public RecurrenceStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "NoRecurrence";

        [Required, Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "No Recurrence";

        public RecurrenceTheme() { }    

        public RecurrenceTheme(RecurrenceStyle style)
        {
            Id = style;
            Value = style.ToName();
            Name = style.ToDescription();
        }

    }

    public enum RecurrenceStyle : int
    {
        [Description("No Recurrence")]
        NoRecurrence = 0,
        [Description("Daily")]
        Daily = 1,
        [Description("Weekly")]
        Weekly = 2,
        [Description("Monthly")]
        Monthly = 3
    }
}
