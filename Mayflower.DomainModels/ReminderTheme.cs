using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MEI.Core.Helpers;

namespace Mayflower.DomainModels
{
    public class ReminderTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ReminderStyle Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "Unknown";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "Unknown";

        public ReminderTheme() { }  

        public ReminderTheme(ReminderStyle style)
        {
            Id = style;
            Value = style.ToName() ?? "Unknown";
            Name = style.ToDescription() ?? "Unknown";
        }

    }

    public enum ReminderStyle : int
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Bill")]
        Bill = 1,
        [Description("Transfer")]
        Transfer = 2,
        [Description("Income")]
        Income = 3
    }
}