using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IVendaRepository : IBaseRepository<Venda>
    {
        Task<string> GenerateUniqueProtocolAsync();
        Task<IEnumerable<Venda>> GetVendasByPeriodAsync(DateTime dataInicio, DateTime dataFim);
        Task<Venda> GetVendaByProtocolAsync(string protocolo);
        Task<IEnumerable<Venda>> GetVendasByClienteIdAsync(int clienteId);
        Task<IEnumerable<Venda>> GetVendasByConcessionariaIdAsync(int concessionariaId);
    }
}
