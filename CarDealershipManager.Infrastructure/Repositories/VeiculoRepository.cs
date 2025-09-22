using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class VeiculoRepository : BaseRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Veiculo>> GetByFabricanteIdAsync(int fabricanteId)
        {
            return await _dbSet.Where(v => v.FabricanteId == fabricanteId).ToListAsync();
        }

        public async Task<IEnumerable<Veiculo>> GetByTipoAsync(TipoVeiculo tipoVeiculo)
        {
            return await _dbSet.Where(v => v.TipoVeiculo == tipoVeiculo).ToListAsync();
        }

        public async Task<IEnumerable<Veiculo>> SearchByModeloAsync(string modelo)
        {
            return await _dbSet.Where(v => v.Modelo.ToLower().Contains(modelo.ToLower())).ToListAsync();
        }
    }
}
