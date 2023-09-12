using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class SchedulingTimes
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
