using CarDealershipManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<bool> IsCpfUniqueAsync(string cpf, int? id = null);
        Task<bool> IsEmailUniqueAsync(string email, int? id = null);
        Task<Cliente?> GetByCpfAsync(string cpf);
        Task<IEnumerable<Cliente>> SearchByNameAsync(string nome);
    }
}
