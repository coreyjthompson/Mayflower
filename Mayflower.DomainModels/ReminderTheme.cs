using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Mayflower.DomainModels
{
    public class ReminderTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ReminderStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = "Unknown";
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