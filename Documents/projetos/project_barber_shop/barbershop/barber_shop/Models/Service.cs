using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace barber_shop.Models
{
    public class Service
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Required]
        [Range(1.00, 999.99, ErrorMessage = "O {0} deve ser entre {2} e {1}")]
        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Value { get; set; }
        [Required]
        [Display(Name = "Imagem")]
        public byte[] Image { get; set; }
    }
}
