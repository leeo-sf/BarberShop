using Newtonsoft.Json;

namespace barber_shop.Models
{
    public class Gender
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Description { get; set; }
    }
}
