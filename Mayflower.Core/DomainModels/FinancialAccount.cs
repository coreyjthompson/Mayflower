using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Nickname { get; set; } = string.Empty;

        public int FinancialInstitutionId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal LedgerBalance { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal AvailableBalance { get; set; }

        public DateTimeOffset WhenLastUpdated { get; set; }

        #region Navigation
        public virtual FinancialInstitution FinancialInstitution { get; set; } = default!;

        public virtual FinancialAccountTheme _financialAccountTheme { get; set; } = default!;

        public virtual IList<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();

        #endregion
    }

}
