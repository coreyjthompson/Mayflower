using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class FinancialAccount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }

        [Column(TypeName = "varchar(255)"), Required]
        public string Number { get; set; } = string.Empty;

        [Column("FinancialAccountThemeId"), ForeignKey("_financialAccountTheme"), Required]
        public FinancialAccountStyle Type { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? NickName { get; set; }

        [Required]
        public int FinancialInstitutionId { get; set; }

        #region Navigation
        public virtual FinancialInstitution FinancialInstitution { get; set; } = new FinancialInstitution();

        public virtual FinancialAccountTheme? _financialAccountTheme { get; set; }
        #endregion
    }

}
