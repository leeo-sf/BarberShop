using barber_shop.Data;
using barber_shop.Models;
using barber_shop.Models.Enums;
using barber_shop.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
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
        Task<User> GetUserByEmail(string email);
        Task<SchedulingTime[]> GetAllSchedulingTimes();
        Task<User[]> GetAllBarbers();
        Task<User> GetUserLoggedInByCpf(string cpf);
        Task<SchedulingTime> GetSchedulingTimeById(int id);
        Task<Scheduling> GetBarberSchedulings(Scheduling obj);
        Task<Scheduling[]> GetAllSchedulings();
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
            return await this.GetOne<Profile>(x => x.Email == email);
            //return await _context.Profile.Include(x => x.Category).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Gender[]> GetGenders()
        {
            //VÊ se o nome da tabela está certo
            return await _context.Gender.FromSqlInterpolated(
            $@"SELECT * FROM sql11646521.GENDER"
            )
                .ToArrayAsync();
        }

        public async Task<AccountCategory[]> GetAccountCategories()
        {
            return await _context.AccountCategory.FromSqlInterpolated(
                $@"SELECT * FROM sql11646521.ACCOUNTCATEGORY"
            )
                .ToArrayAsync();
        }

        public async Task<Service[]> GetServices()
        {
            return await this.GetAll<Service>();
        }

        public async Task<Service> GetService(int id)
        {
            return await this.GetOne<Service>(x => x.Id == id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var userProfile = await this.GetProfileEmail(email);
            return await _context.User
                .Include(x => x.Profile) //fazendo relacionamento com a tabela de profile
                .Include(x => x.Profile.Category) //azendo relacionamento com a tabela de accountcategory
                .FirstOrDefaultAsync(x => x.ProfileId == userProfile.Id);

            //NÃO TA FUNCIONANDO
            //return await _context.Set<User>().FromSqlInterpolated(
            //    $@"SELECT * FROM db_barber_shop.`user` as u 
            //    INNER JOIN db_barber_shop.profile as p
            //    ON u.profile_id = {email}"
            //)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync();
        }

        public async Task<SchedulingTime[]> GetAllSchedulingTimes()
        {
            return await this.GetAll<SchedulingTime>();
        }

        public async Task<User[]> GetAllBarbers()
        {
            //precisa se relacionar com a tabela de profile
            //a tabela de profile precisa se relacionar com a tabela accountcategory
            //para saber e o usuário é barber
            return await _context.User
                .Include(x => x.Profile)
                .Include(x => x.Profile.Category)
                .Where(x => x.Profile.Category.Description == nameof(EnumAccountCategory.BARBER))
                .ToArrayAsync();
        }

        public async Task<User> GetUserLoggedInByCpf(string cpf)
        {
            //método que pegar o usuário logado pelo cpf
            return await this.GetOne<User>(x => x.Cpf == cpf);
        }

        public async Task<SchedulingTime> GetSchedulingTimeById(int id)
        {
            return await this.GetOne<SchedulingTime>(x => x.Id == id);
        }

        public async Task<Scheduling> GetBarberSchedulings(Scheduling obj)
        {
            //método que auxilia na descoberta para saber se o berbeiro possuí agendamentos naquele dia e horário
            return await _context.Scheduling
                .Include(x => x.SchedulingTimes) //fazendo include com a tabela de horários
                .Where(
                x => x.BarberId == obj.BarberId &&
                x.Date == obj.Date &&
                x.SchedulingTimesId == obj.SchedulingTimesId
                ) //quando a data for igual a data selecionada pelo usuário e o id do horário for igual ao id selecionado pelo usuário
                .FirstOrDefaultAsync();
        }

        public async Task<Scheduling[]> GetAllSchedulings()
        {
            //fazer relacionamento entre tabelas
            return await _context.Scheduling
                .Include(x => x.Client)
                .Include(x => x.Barber)
                .Include(x => x.SchedulingTimes)
                .Include(x => x.Service)
                .ToArrayAsync();
        }
    }
}
