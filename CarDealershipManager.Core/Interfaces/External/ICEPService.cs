using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Interfaces.External
{
    public interface ICEPService
    {
        Task<EnderecoDTO> BuscarEnderecoPorCEPAsync(string cep);
    }
}
