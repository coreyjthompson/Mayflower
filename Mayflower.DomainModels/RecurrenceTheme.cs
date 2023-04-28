using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mayflower.DomainModels
{
    public class RecurrenceTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public RecurrenceStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = "No Recurrence";
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
