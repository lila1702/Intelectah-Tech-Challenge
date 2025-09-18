namespace CarDealershipManager.Core.Interfaces.Services
{
    public interface IFabricanteService
    {
        Task<FabricanteDTO> GetByIdAsync(int id);
        Task<IEnumerable<FabricanteDTO>> GetAllAsync();
        Task<FabricanteDTO> CreateAsync(FabricanteCreateDTO fabricanteDTO);
        Task<FabricanteDTO> UpdateAsync(int id, FabricanteUpdateDTO fabricanteDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<FabricanteDTO>> GetWithVeiculosAsync();
    }
}
