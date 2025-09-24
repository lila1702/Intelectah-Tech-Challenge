using CarDealershipManager.Core.Interfaces.Repositories;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class ConcessionariaRepository : BaseRepository<Concessionaria>, IConcessionariaRepository
    {
        public ConcessionariaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Concessionaria>> GetByNomeAsync(string nome)
        {
            return await _dbSet.Where(c => c.Nome.ToLower().Contains(nome.ToLower())).ToListAsync();
        }

        public async Task<bool> IsNomeUniqueAsync(string nome, int? id = null)
        {
            var query = _dbSet.Where(c => c.Nome.ToLower() == nome.ToLower());

            if (id.HasValue)
            {
                query = query.Where(c => c.Id != id.Value);
            }

            return !await query.AnyAsync();
        }
    }
}
