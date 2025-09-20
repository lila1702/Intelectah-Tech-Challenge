using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class FabricanteRepository : BaseRepository<Fabricante>, IFabricanteRepository
    {
        public FabricanteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Fabricante>> GetWithVeiculosAsync()
        {
            return await _dbSet.Include(f => f.Veiculos).ToListAsync();
        }

        public async Task<bool> IsNomeUniqueAsync(string nome, int? id = null)
        {
            var query = _dbSet.Where(f => f.Nome.ToLower() == nome.ToLower());

            if (id.HasValue)
            {
                query = query.Where(f => f.Id != id.Value);
            }

            return !await query.AnyAsync();
        }
    }
}
