using barber_shop.Data;
using barber_shop.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace barber_shop.Services
{
    public interface IBarberShopRepository
    {
        Task Update<T>(params T[] entities) where T : class;
        Task Delete<T>(params T[] entities) where T : class;
        Task<T[]> GetAll<T>() where T : class;
        Task<T[]> GetAll<T>(Expression<Func<T, bool>> condition) where T : class;
        Task Insert<T>(params T[] entities) where T : class;
        Task<Profile> GetProfileEmail(string email);
        Task<Gender[]> GetGenders();
        Task<bool> ValidateEmailPassword(Profile obj);
        Task<AccountCategory[]> GetAccountCategories();
        Task<Service[]> GetServices();
        Task<Service> GetService(int id);
    }

    public class BarberShopRepository : IBarberShopRepository
    {
        private readonly Context _context;

        public BarberShopRepository(Context context)
        {
            _context = context;
        }

        public async Task Update<T>(params T[] entities) where T : class
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task Delete<T>(params T[] entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T[]> GetAll<T>() where T : class
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<T[]> GetAll<T>(Expression<Func<T, bool>> condition) where T : class
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(condition)
                .ToArrayAsync();
        }

        public async Task<T> GetOne<T>(Expression<Func<T, bool>> condition) where T : class
        {
            return await _context.Set<T>()
              .AsNoTracking()
              .Where(condition)
              .FirstOrDefaultAsync();
        }

        public async Task Insert<T>(params T[] entities) where T : class
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateEmailPassword(Profile obj)
        {
            var perfil = await GetProfileEmail(obj.Email);
            if (!(perfil == null))
            {
                if (perfil.Password == obj.Password)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<Profile> GetProfileEmail(string email)
        {
            //fazendo relacionamento com a tabela de category
            return await _context.Profile.Include(x => x.Category).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Gender[]> GetGenders()
        {
            //VÊ se o nome da tabela está certo
            return await _context.Gender.FromSqlInterpolated(
            $@"SELECT * FROM db_barber_shop.Gender"
            )
                .ToArrayAsync();
        }

        public async Task<AccountCategory[]> GetAccountCategories()
        {
            return await _context.AccountCategory.FromSqlInterpolated(
                $@"SELECT * FROM db_barber_shop.AccountCategory"
            )
                .ToArrayAsync();
        }

        public async Task<Service[]> GetServices()
        {
            return await _context.Service.FromSqlInterpolated(
                $@"SELECT * FROM db_barber_shop.service"
            )
                .ToArrayAsync();
        }

        public async Task<Service> GetService(int id)
        {
            return await _context.Service.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
