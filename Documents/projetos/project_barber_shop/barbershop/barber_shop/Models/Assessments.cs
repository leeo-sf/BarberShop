using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class Assessments
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} obrigatório")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        [Column("barber_id")]
        public int BarberId { get; set; }
        public User Barber { get; set; }
        [Column("client_id")]
        public int ClientId { get; set; }
        public User Client { get; set; }
    }
}
