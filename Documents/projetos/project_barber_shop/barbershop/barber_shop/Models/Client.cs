using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop.Models
{
    public class Client : Person
    {
        [Column("profile_id")]
        [JsonIgnore]
        public int ProfileId { get; set; }

        public Profile Profile { get; set; }
    }
}
