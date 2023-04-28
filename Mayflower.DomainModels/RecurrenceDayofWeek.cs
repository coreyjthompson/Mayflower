using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mayflower.DomainModels
{
    public class RecurrenceDayofWeek
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DayOfWeek Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = "None";
    }

    public enum DayOfWeek : int
    {
        [Description("None")]
        None = 0,
        [Description("Sunday")]
        Sunday = 1,
        [Description("Monday")]
        Monday = 2,
        [Description("Tuesday")]
        Tuesday = 4,
        [Description("Wednesday")]
        Wednesday = 8,
        [Description("Thursday")]
        Thursday = 16,
        [Description("Friday")]
        Friday = 32,
        [Description("Saturday")]
        Saturday = 64
    }

}
