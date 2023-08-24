using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class FinancialAccountTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public FinancialAccountStyle Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "None";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "None";

        public FinancialAccountTheme() { }

        public FinancialAccountTheme(FinancialAccountStyle style)
        {
            Id = style;
            Value = style.ToName() ?? "None";
            Name = style.ToDescription() ?? "None";
        }

    }

    public enum FinancialAccountStyle : int
    {
        [Description("None")]
        None = 0,
        [Description("Savings")]
        Savings = 1,
        [Description("Checking")]
        Checking = 2,
        [Description("Marketing")]
        Marketing = 3
    }
}
