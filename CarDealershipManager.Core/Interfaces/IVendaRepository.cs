using CarDealershipManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IVendaRepository : IBaseRepository<Venda>
    {
        Task<string> GenerateUniqueProtocolAsync();
        Task<IEnumerable<Venda>> GetVendasByPeriodAsync(DateTime dataInicio, DateTime dataFim);
        Task<Venda> GetVendaByProtocolAsync(string protocolo);
    }
}
