using CarDealershipManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Interfaces.Services
{
    public interface IConcessionariaService
    {
        Task<ConcessionariaDTO> GetByIdAsync(int id);
        Task<IEnumerable<ConcessionariaDTO>> GetAllAsync();
        Task<ConcessionariaDTO> CreateAsync(ConcessionariaCreateDTO concessionariaDTO);
        Task<ConcessionariaDTO> UpdateAsync(int id, ConcessionariaUpdateDTO concessionariaDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<ConcessionariaDTO>> GetByNomeAsync(string nome);
    }
}
