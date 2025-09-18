using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IVeiculoRepository : IBaseRepository<Veiculo>
    {
        Task<IEnumerable<Veiculo>> GetAllByFabricanteIdAsync(int fabricanteId);
        Task<IEnumerable<Veiculo>> GetAllByTipoAsync(TipoVeiculo tipoVeiculo);
        Task<IEnumerable<Veiculo>> SearchByModeloAsync(string modelo);
    }
}
