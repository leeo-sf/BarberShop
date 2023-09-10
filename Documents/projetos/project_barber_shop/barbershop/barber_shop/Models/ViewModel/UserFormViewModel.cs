namespace barber_shop.Models.ViewModel
{
    public class UserFormViewModel
    {
        public User User { get; set; }
        public Gender[] Genders { get; set; }
        public AccountCategory[] AccountCategories { get; set; }
    }
}
