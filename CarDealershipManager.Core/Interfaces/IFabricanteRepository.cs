using CarDealershipManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IFabricanteRepository : IBaseRepository<Fabricante>
    {
        Task<bool> IsNomeUniqueAsync(string nome, int? id = null);
        Task<List<Fabricante>> SearchFabricanteByNameAsync(string nome);
        Task<IEnumerable<Fabricante>> GetWithVeiculosAsync();
    }
}
