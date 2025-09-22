using CarDealershipManager.Core.DTOs;

namespace CarDealershipManager.Core.Interfaces.External
{
    public interface ICEPService
    {
        Task<EnderecoDTO> BuscarEnderecoPorCEPAsync(string cep);
    }
}
