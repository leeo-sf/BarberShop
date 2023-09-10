using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class Administrator : Person
    {
        [Column("profile_id")]
        [JsonIgnore]
        public int ProfileId { get; set; }

        [Display(Name = "Perfil")]
        public Profile Profile { get; set; }
    }
}
