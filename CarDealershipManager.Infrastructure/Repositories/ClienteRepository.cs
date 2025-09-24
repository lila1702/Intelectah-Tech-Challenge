using Microsoft.EntityFrameworkCore;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using CarDealershipManager.Core.Interfaces.Repositories;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Cliente?> GetByCpfAsync(string cpf)
        {
            string cpfNormalizado = new string(cpf.Where(char.IsDigit).ToArray());
            return await _dbSet.FirstOrDefaultAsync(c => c.CPF == cpfNormalizado);
        }

        public async Task<bool> IsCpfUniqueAsync(string cpf, int? id = null)
        {
            var query = _dbSet.Where(c => c.CPF == cpf);

            if (id.HasValue)
            {
                query = query.Where(c => c.Id != id.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Cliente>> SearchByNameAsync(string nome)
        {
            return await _dbSet.Where(c => c.Nome.ToLower().Contains(nome.ToLower())).ToListAsync();
        }
    }
}
