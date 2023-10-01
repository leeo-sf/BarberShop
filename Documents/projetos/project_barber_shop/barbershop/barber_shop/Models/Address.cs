using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop.Models
{
    [Table("ADDRESS")]
    public class Address
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Logradouro")]
        public string PublicPlace { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Bairro")]
        public string Neighborhood { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Localidade")]
        public string Locality { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 7, ErrorMessage = "Preencha o {0} corretamente")]
        [Display(Name = "CEP")]
        public string ZipCode { get; set; }
    }
}
