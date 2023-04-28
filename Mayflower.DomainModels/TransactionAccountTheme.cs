using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MEI.Core.Helpers;

namespace Mayflower.DomainModels
{
    public class TransactionAccountTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TransactionAccountStyle Id { get; set; }

        [Required, Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "None";

        [Required, Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "None";

        public TransactionAccountTheme() { }   

        public TransactionAccountTheme(TransactionAccountStyle style)
        {
            Id = style;
            Value = style.ToName();
            Name = style.ToDescription();
        }

    }

    public enum TransactionAccountStyle : int
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
