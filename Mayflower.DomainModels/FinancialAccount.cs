using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
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
        public string? NickName { get; set; }

        public int FinancialInstitutionId { get; set; }

        #region Navigation
        public virtual FinancialInstitution? FinancialInstitution { get; set; }

        public virtual FinancialAccountTheme? _financialAccountTheme { get; set; }
        #endregion
    }

}
