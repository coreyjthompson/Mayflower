using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MEI.Core.Helpers;

namespace Mayflower.DomainModels
{
    public class RecurrenceOrdinal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public RecurrencePosition Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "None";

        [Required, Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "None";

        public RecurrenceOrdinal() { }

        public RecurrenceOrdinal(RecurrencePosition style)
        {
            Id = style;
            Value = style.ToName();
            Name = style.ToDescription();
        }

    }

    public enum RecurrencePosition : int
    {
        [Description("None")]
        None = 0,
        [Description("First")]
        First = 1,
        [Description("Second")]
        Second = 2,
        [Description("Third")]
        Third = 3,
        [Description("Fourth")]
        Fourth = 4,
        [Description("Last")]
        Last = 5
    }

}
