using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop_PI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        [Column("id_categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public Usuario() { }

        public Usuario(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }
    }
}
