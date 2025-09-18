namespace CarDealershipManager.Core.Interfaces.Services
{
    public interface IVendaService
    {
        Task<VendaDTO> GetByIdAsync(int id);
        Task<IEnumerable<VendaDTO>> GetAllAsync();
        Task<VendaDTO> CreateAsync(VendaCreateDTO vendaDTO);
        Task<VendaDTO> GetByProtocolAsync(string protocolo);
        Task<IEnumerable<VendaDTO>> GetByPeriodAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<VendaDTO>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<VendaDTO>> GetByConcessionariaIdAsync(int concessionariaId);

    }
}
