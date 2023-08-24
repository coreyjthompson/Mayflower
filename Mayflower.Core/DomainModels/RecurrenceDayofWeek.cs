using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
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
            Value = day.ToName() ?? "None";
            Name = day.ToName() ?? "None";
        }
    }
}
