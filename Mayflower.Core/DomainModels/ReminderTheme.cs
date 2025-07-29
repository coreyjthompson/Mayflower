using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class ReminderTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ReminderStyle Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "Error";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "Error";

        public ReminderTheme() { }

        public ReminderTheme(ReminderStyle style)
        {
            Id = style;
            Value = style.ToName() ?? "Error";
            Name = style.ToDescription() ?? "Error";
        }

    }

    public enum ReminderStyle : int
    {
        [Description("Error")]
        Error = 0,
        [Description("Bill")]
        Bill = 1,
        [Description("Income")]
        Income = 2,
        [Description("Transfer")]
        Transfer = 3,
    }
}