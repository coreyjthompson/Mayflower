using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class TransactionAccountTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TransactionAccountStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Description { get; set; } = "None";
    }

    public enum TransactionAccountStyle : int
    {
        None = 0,
        Savings = 1,
        Checking = 2,
        Marketing = 3
    }
}
