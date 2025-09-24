using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Core.Interfaces.Repositories
{
    public interface IConcessionariaRepository : IBaseRepository<Concessionaria>
    {
        Task<bool> IsNomeUniqueAsync(string nome, int? id = null);
        Task<IEnumerable<Concessionaria>> GetByNomeAsync(string nome);
    }
}
