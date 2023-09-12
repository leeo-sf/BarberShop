namespace barber_shop.Models.ViewModel
{
    public class SchedulingFormViewModel
    {
        public Client Client { get; set; }
        public Barber[] Barber { get; set; }
        public Scheduling Scheduling { get; set; }
        public Service[] Service { get; set; }
    }
}
