using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class TransactionAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)"), Required]
        public string Number { get; set; } = string.Empty;

        [Column("TransactionAccountThemeId"), ForeignKey("_transactionAccountTheme"), Required]
        public TransactionAccountStyle Type { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? NickName { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? AccountVendor { get; set; }

        #region Navigation
        public virtual TransactionAccountTheme? _transactionAccountTheme { get; set; }
        #endregion
    }

}
