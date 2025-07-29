using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class FinancialInstitution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string? NickName { get; set; }
    }

}
