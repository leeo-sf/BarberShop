using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    [Table("SCHEDULINGTIME")]
    public class SchedulingTime
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
