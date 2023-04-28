using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class TransactionTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TransactionStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = "None";
    }

    public enum TransactionStyle : int
    {
        [Description("None")]
        None = 0,
        [Description("Withdraw")]
        Withdraw = 1,
        [Description("Deposit")]
        Deposit = 2
    }
}