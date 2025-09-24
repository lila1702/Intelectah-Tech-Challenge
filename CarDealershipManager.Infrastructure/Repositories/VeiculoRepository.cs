using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Interfaces.Repositories;
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

        public override async Task<Veiculo> GetByIdAsync(int id)
        {
            var veiculo = await _dbSet.Include(v => v.Fabricante).FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
            {
                throw new InvalidOperationException("Veiculo não encontrado");
            }

            return veiculo;
        }

        public override async Task<IEnumerable<Veiculo>> GetAllActiveAsync()
        {
            return await _dbSet.Include(v => v.Fabricante).ToListAsync();
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
