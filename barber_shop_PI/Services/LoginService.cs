using barber_shop_PI.Data;
using barber_shop_PI.Models;
using Microsoft.EntityFrameworkCore;

namespace barber_shop_PI.Services
{
    public class LoginService
    {
        private readonly Contexto _contexto;

        public LoginService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> ValidaLogin(Usuario obj)
        {
            bool email = await _contexto.Usuario.AnyAsync(x => x.Email == obj.Email);
            bool senha = await _contexto.Usuario.AnyAsync(x => x.Senha == obj.Senha);

            if (email && senha)
            {
                return await _contexto.Usuario.Include(x => x.Categoria).FirstOrDefaultAsync(x => x.Email == obj.Email);
            }

            return null;
        }
    }
}
