using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop.Models
{
    [Table("GENDER")]
    public class Gender
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
