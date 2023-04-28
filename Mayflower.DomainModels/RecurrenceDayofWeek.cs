using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MEI.Core.Helpers;

namespace Mayflower.DomainModels
{
    public class RecurrenceDayOfWeek
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DayOfWeek Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "None";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "None";

        public RecurrenceDayOfWeek() { }

        public RecurrenceDayOfWeek(DayOfWeek day)
        {
            Id = day;
            Value = day.ToName();
            Name = day.ToDescription();

        }

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
