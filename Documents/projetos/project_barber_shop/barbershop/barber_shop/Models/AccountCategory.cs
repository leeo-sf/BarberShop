using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop.Models
{
    [Table("accountcategory")]
    public class AccountCategory
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
