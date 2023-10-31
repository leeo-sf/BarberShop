using System.ComponentModel.DataAnnotations;

namespace barber_shop.Models.ViewModel
{
    public class SchedulingFormViewModel
    {
        public Scheduling Scheduling { get; set; }
        public User[] Barbers { get ; set; }
        public Service[] Services { get; set; }
        public SchedulingTime[] SchedulingTimes { get; set; }
        [Required(ErrorMessage = "{0} obrigatório")]
        [Display(Name = "CPF do Responsável")]
        public string? CpfResponsible { get; set; }
    }
}
