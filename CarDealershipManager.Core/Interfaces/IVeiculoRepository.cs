using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IVeiculoRepository : IBaseRepository<Veiculo>
    {
        Task<IEnumerable<Veiculo>> GetByFabricanteIdAsync(int fabricanteId);
        Task<IEnumerable<Veiculo>> GetByTipoAsync(TipoVeiculo tipoVeiculo);
        Task<IEnumerable<Veiculo>> SearchByModeloAsync(string modelo);
    }
}
