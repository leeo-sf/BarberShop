using System.ComponentModel.DataAnnotations;

namespace barber_shop.Models
{
    public class PromotionalMessage
    {
        [Required]
        [Display(Name = "Assunto do Email")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Título do Email")]
        public string TitleEmail { get; set; }
        [Required]
        [Display(Name = "Corpo do Email")]
        public string EmailBody { get; set; }
        public byte[]? Image { get; set; }
    }
}
