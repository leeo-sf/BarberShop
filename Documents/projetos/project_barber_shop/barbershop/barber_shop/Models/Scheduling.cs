using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    [Table("SCHEDULING")]
    public class Scheduling
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Column("client_id")]
        public int ClientId { get; set; }
        public User Client { get; set; }
        [Column("barber_id")]
        public int BarberId { get; set; }
        public User Barber { get; set; }
        [Required(ErrorMessage = "{0} obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data do agendamento")]
        public DateTime Date { get; set; }
        [Column("schedulingtimes_id")]
        public int SchedulingTimesId { get; set; }
        [Display(Name = "Horário do agendamento")]
        public SchedulingTime SchedulingTimes { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public bool Concluded { get; set; }
    }
}
