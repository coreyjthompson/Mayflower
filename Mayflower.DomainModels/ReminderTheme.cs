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
        [Description("Bill Payment")]
        Bill = 1,
        [Description("Income")]
        Income = 2,
        [Description("Transfer - Withdrawal")]
        TransferWithdrawal = 3,
        [Description("Transfer - Deposit")]
        TransferDeposit = 4,
    }
}