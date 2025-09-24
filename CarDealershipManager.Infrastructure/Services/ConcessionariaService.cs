using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Infrastructure.Services
{
    public class ConcessionariaService : IConcessionariaService
    {
        private readonly IConcessionariaRepository _concessionariaRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public ConcessionariaService(
            IConcessionariaRepository concessionariaRepository,
            IMapper mapper,
            ICacheService cacheService)
        {
            _concessionariaRepository = concessionariaRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ConcessionariaDTO> CreateAsync(ConcessionariaCreateDTO concessionariaDTO)
        {
            if (!await _concessionariaRepository.IsNomeUniqueAsync(concessionariaDTO.Nome))
            {
                throw new ArgumentException("Já existe uma concessionária com este nome");
            }

            var concessionaria = _mapper.Map<Concessionaria>(concessionariaDTO);
            await _concessionariaRepository.AddAsync(concessionaria);

            await _cacheService.RemoveAsync("concessionarias_all");

            return _mapper.Map<ConcessionariaDTO>(concessionaria);
        }

        public async Task DeleteAsync(int id)
        {
            await _concessionariaRepository.DeleteByIdAsync(id);

            await _cacheService.RemoveAsync($"concessionarias_{id}");
            await _cacheService.RemoveAsync("concessionarias_all");
        }

        public async Task<IEnumerable<ConcessionariaDTO>> GetAllAsync()
        {
            var cacheKey = "concessionarias_all";
            var cached = await _cacheService.GetAsync<IEnumerable<ConcessionariaDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var concessionarias = await _concessionariaRepository.GetAllActiveAsync();
            var concessionariasDTO = _mapper.Map<IEnumerable<ConcessionariaDTO>>(concessionarias);

            await _cacheService.SetAsync(cacheKey, concessionariasDTO, TimeSpan.FromMinutes(15));

            return concessionariasDTO;
        }

        public async Task<ConcessionariaDTO> GetByIdAsync(int id)
        {
            var cacheKey = $"concessionarias_{id}";
            var cached = await _cacheService.GetAsync<ConcessionariaDTO>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var concessionaria = await _concessionariaRepository.GetByIdAsync(id);
            if (concessionaria == null)
            {
                throw new ArgumentException("Concessionária não encontrada");
            }

            var concessionariaDTO = _mapper.Map<ConcessionariaDTO>(concessionaria);
            await _cacheService.SetAsync(cacheKey, concessionariaDTO, TimeSpan.FromMinutes(30));

            return concessionariaDTO;
        }

        public async Task<IEnumerable<ConcessionariaDTO>> GetByNomeAsync(string nome)
        {
            var concessionarias = await _concessionariaRepository.GetByNomeAsync(nome);
            return _mapper.Map<IEnumerable<ConcessionariaDTO>>(concessionarias);
        }

        public async Task<ConcessionariaDTO> UpdateAsync(int id, ConcessionariaUpdateDTO concessionariaDTO)
        {
            var concessionaria = await _concessionariaRepository.GetByIdAsync(id);
            if (concessionaria == null)
            {
                throw new ArgumentException("Concessionária não encontrada");
            }

            if (!await _concessionariaRepository.IsNomeUniqueAsync(concessionariaDTO.Nome, id))
            {
                throw new ArgumentException("Já existe uma concessionária com este nome");
            }

            _mapper.Map(concessionariaDTO, concessionaria);
            await _concessionariaRepository.UpdateAsync(concessionaria);

            await _cacheService.RemoveAsync($"concessionarias_{id}");
            await _cacheService.RemoveAsync("concessionarias_all");

            return _mapper.Map<ConcessionariaDTO>(concessionaria);
        }
    }
}
