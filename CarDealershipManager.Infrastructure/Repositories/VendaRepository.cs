using Microsoft.EntityFrameworkCore;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class VendaRepository : BaseRepository<Venda>, IVendaRepository
    {
        public VendaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Venda> GetByIdAsync(int id)
        {
            var venda = await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                               .Include(v => v.Concessionaria)
                               .Include(v => v.Cliente)
                               .FirstOrDefaultAsync(v => v.Id == id);

            if (venda == null)
            {
                throw new InvalidOperationException("Venda não encontrada");
            }

            return venda;
        }

        public override async Task<IEnumerable<Venda>> GetAllAsync()
        {
            return await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                               .Include(v => v.Concessionaria)
                               .Include(v => v.Cliente)
                               .OrderByDescending(v => v.DataVenda)
                               .ToListAsync();
        }

        public async Task<string> GenerateUniqueProtocolAsync()
        {
            string protocolo;
            bool exists;

            do
            {
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                var random = new Random().Next(1000, 9999);
                protocolo = $"{timestamp}{random}";

                exists = await _dbSet.AnyAsync(v => v.ProtocoloVenda == protocolo);
            } while (exists);

            return protocolo;
        }

        public async Task<Venda> GetVendaByProtocolAsync(string protocolo)
        {
            var venda = await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                         .Include(v => v.Concessionaria)
                         .Include(v => v.Cliente)
                         .FirstOrDefaultAsync(v => v.ProtocoloVenda == protocolo);

            if (venda == null)
            {
                throw new InvalidOperationException("Venda não encontrada");
            }

            return venda;
        }

        public async Task<IEnumerable<Venda>> GetVendasByPeriodAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                               .Include(v => v.Concessionaria)
                               .Include(v => v.Cliente)
                               .Where(v => v.DataVenda >= dataInicio && v.DataVenda <= dataFim)
                               .OrderByDescending(v => v.DataVenda)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Venda>> GetVendasByClienteIdAsync(int clienteId)
        {
            return await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                               .Include(v => v.Concessionaria)
                               .Include(v => v.Cliente)
                               .Where(v => v.ClienteId == clienteId)
                               .OrderByDescending(v => v.DataVenda)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Venda>> GetVendasByConcessionariaIdAsync(int concessionariaId)
        {
            return await _dbSet.Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                               .Include(v => v.Concessionaria)
                               .Include(v => v.Cliente)
                               .Where(v => v.ConcessionariaId == concessionariaId)
                               .OrderByDescending(v => v.DataVenda)
                               .ToListAsync();
        }
    }
}
