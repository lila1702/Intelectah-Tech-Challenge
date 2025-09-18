using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Core.Interfaces
{
    public interface IConcessionariaRepository : IBaseRepository<Concessionaria>
    {
        Task<bool> IsNomeUniqueAsync(string nome, int? id = null);
        Task<IEnumerable<Concessionaria>> GetByLocationAsync(string location);
    }
}
