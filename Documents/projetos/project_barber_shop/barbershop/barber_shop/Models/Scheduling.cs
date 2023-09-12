using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class Scheduling
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Column("client_id")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Column("barber_id")]
        public int BarberId { get; set; }
        public Barber Barber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data do agendamento")]
        public DateTime Date { get; set; }
        [Column("schedulingtimes_id")]
        public int SchedulingTimesId { get; set; }
        public SchedulingTimes SchedulingTimes { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
