using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class PhotoOfBarberServices
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [Column("barber_id")]
        public int BarberId { get; set; }
        [Required]
        public byte[] Image { get; set; }
    }
}
