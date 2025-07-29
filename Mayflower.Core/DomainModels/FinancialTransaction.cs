using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class FinancialTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Type { get; set; } = default!;

        [Column(TypeName = "varchar(125)")]
        public string ExternalTransactionId { get; set; } = default!;

        public DateOnly WhenPosted { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? RefNumber { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; } = default!;

        [Column(TypeName = "varchar(255)")]
        public string? Memo { get; set; }

        public int FinancialAccountId { get; set; }

        #region Navigation
        [ForeignKey("FinancialAccountId")]
        public virtual FinancialAccount FinancialAccount { get; set; } = new FinancialAccount();
        #endregion
    }
}