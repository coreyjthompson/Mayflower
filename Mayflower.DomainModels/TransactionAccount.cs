using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class TransactionAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Column("TransactionAccountThemeId"), ForeignKey("_transactionAccountTheme"), Required]
        public TransactionAccountStyle Type { get; set; }

        public string? NickName { get; set; }

        public string? AccountVendor { get; set; }

        #region Navigation
        public virtual TransactionAccountTheme _transactionAccountTheme { get; set; } = new TransactionAccountTheme();
        #endregion
    }

}
