using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.DomainModels
{
    public class Weather
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column("WeatherTypeId"), ForeignKey("WeatherTypeId")]
        public WeatherTypeId TypeId { get; set; }

        public virtual WeatherType Type { get; set; } = new WeatherType();
    }

    public enum WeatherTypeId
    {
        Cloudy = 1,
        Sunny = 2,
        Rainy = 3,
    }

    public class WeatherType
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public WeatherTypeId Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Description { get; set; } = "Add your description here";
    }

}
