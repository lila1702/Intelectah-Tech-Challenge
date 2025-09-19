using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Core.Interfaces.Services
{
    public interface IVeiculosService
    {
        Task<VeiculoDTO> GetByIdAsync(int id);
        Task<IEnumerable<VeiculoDTO>> GetAllAsync();
        Task<VeiculoDTO> CreateAsync(VeiculoCreateDTO veiculoDTO);
        Task<VeiculoDTO> UpdateAsync(int id, VeiculoUpdateDTO veiculoDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<VeiculoDTO>> GetByFabricanteIdAsync(int fabricanteId);
        Task<IEnumerable<VeiculoDTO>> GetByTipoAsync(TipoVeiculo tipoVeiculo);
        Task<IEnumerable<VeiculoDTO>> SearchByModeloAsync(string modelo);
    }
}
