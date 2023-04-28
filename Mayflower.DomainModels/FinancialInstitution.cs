using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class FinancialInstitution
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)"), Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(50)")]
        public string? NickName { get; set; }
    }

}
