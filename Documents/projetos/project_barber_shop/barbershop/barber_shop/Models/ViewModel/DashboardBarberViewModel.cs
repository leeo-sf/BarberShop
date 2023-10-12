namespace barber_shop.Models.ViewModel
{
    public class DashboardBarberViewModel
    {
        public User Barber { get; set; }
        public Assessments[] Assessments { get; set; }
        public PhotoOfBarberServices[] PhotoOfBarberServices { get; set; }
    }
}
