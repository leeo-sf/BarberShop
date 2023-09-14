using System.ComponentModel.DataAnnotations;

namespace barber_shop.Models.ViewModel
{
    public class SchedulingFormViewModel
    {
        public Scheduling Scheduling { get; set; }
        public User[] Barbers { get ; set; }
        public Service[] Services { get; set; }
        public SchedulingTimes[] SchedulingTimes { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "O {0} deve conter {1} caracteres")]
        public string? CpfResponsible { get; set; }
    }
}
