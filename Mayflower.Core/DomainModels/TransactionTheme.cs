using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Mayflower.Core.Extensions;

namespace Mayflower.Core.DomainModels
{
    public class TransactionTheme
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TransactionStyle Id { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string Value { get; set; } = "None";

        [Column(TypeName = "varchar(25)")]
        public string Name { get; set; } = "None";

        public TransactionTheme() { }

        public TransactionTheme(TransactionStyle style)
        {
            Id = style;
            Value = style.ToName() ?? "None";
            Name = style.ToDescription() ?? "None";
        }
    }

    public enum TransactionStyle : int
    {
        [Description("None")]
        None = 0,
        [Description("Withdraw")]
        Withdraw = 1,
        [Description("Deposit")]
        Deposit = 2
    }
}