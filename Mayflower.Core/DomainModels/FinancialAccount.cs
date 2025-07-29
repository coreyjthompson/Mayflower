using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class FinancialAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Number { get; set; } = string.Empty;

        [Column("FinancialAccountThemeId"), ForeignKey("_financialAccountTheme")]
        public FinancialAccountStyle FinancialAccountTheme { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? Nickname { get; set; }

        public int FinancialInstitutionId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal LedgerBalance { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal AvailableBalance { get; set; }

        public DateTimeOffset WhenLastUpdated { get; set; }

        #region Navigation
        public virtual FinancialInstitution? FinancialInstitution { get; set; }

        public virtual FinancialAccountTheme? _financialAccountTheme { get; set; }

        public virtual IList<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();

        #endregion
    }

}
