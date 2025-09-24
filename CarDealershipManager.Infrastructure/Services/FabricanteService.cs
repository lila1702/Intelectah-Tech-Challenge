using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Interfaces.Services;

namespace CarDealershipManager.Infrastructure.Services
{
    public class FabricanteService : IFabricanteService
    {
        private readonly IFabricanteRepository _fabricanteRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public FabricanteService(
            IFabricanteRepository fabricanteRepository,
            IMapper mapper,
            ICacheService cacheService)
        {
            _fabricanteRepository = fabricanteRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<FabricanteDTO> CreateAsync(FabricanteCreateDTO fabricanteDTO)
        {
            if (!await _fabricanteRepository.IsNomeUniqueAsync(fabricanteDTO.Nome))
            {
                throw new ArgumentException("Já existe um fabricante com este nome");
            }

            var fabricante = _mapper.Map<Fabricante>(fabricanteDTO);
            await _fabricanteRepository.AddAsync(fabricante);

            await _cacheService.RemoveAsync("fabricantes_all");

            return _mapper.Map<FabricanteDTO>(fabricante);
        }

        public async Task DeleteAsync(int id)
        {
            await _fabricanteRepository.DeleteByIdAsync(id);

            await _cacheService.RemoveAsync($"fabricantes_{id}");
            await _cacheService.RemoveAsync("fabricantes_all");
        }

        public async Task<IEnumerable<FabricanteDTO>> GetAllAsync()
        {
            var cacheKey = "fabricantes_all";
            var cached = await _cacheService.GetAsync<IEnumerable<FabricanteDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var fabricantes = await _fabricanteRepository.GetAllActiveAsync();
            var fabricantesDTO = _mapper.Map<IEnumerable<FabricanteDTO>>(fabricantes);

            await _cacheService.SetAsync(cacheKey, fabricantesDTO, TimeSpan.FromMinutes(15));

            return fabricantesDTO;
        }

        public async Task<FabricanteDTO> GetByIdAsync(int id)
        {
            var cacheKey = $"fabricantes_{id}";
            var cached = await _cacheService.GetAsync<FabricanteDTO>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var fabricante = await _fabricanteRepository.GetByIdAsync(id);
            if (fabricante == null)
            {
                throw new ArgumentException("Fabricante não encontrado");
            }

            var fabricanteDTO = _mapper.Map<FabricanteDTO>(fabricante);
            await _cacheService.SetAsync(cacheKey, fabricanteDTO, TimeSpan.FromMinutes(30));

            return fabricanteDTO;
        }

        public async Task<IEnumerable<FabricanteDTO>> GetWithVeiculosAsync()
        {
            var fabricantes = await _fabricanteRepository.GetWithVeiculosAsync();
            return _mapper.Map<IEnumerable<FabricanteDTO>>(fabricantes);
        }

        public async Task<FabricanteDTO> UpdateAsync(int id, FabricanteUpdateDTO fabricanteDTO)
        {
            var fabricante = await _fabricanteRepository.GetByIdAsync(id);
            if (fabricante == null)
            {
                throw new ArgumentException("Fabricante não encontrado");
            }

            if (!await _fabricanteRepository.IsNomeUniqueAsync(fabricanteDTO.Nome, id))
            {
                throw new ArgumentException("Já existe um fabricante com este nome");
            }

            _mapper.Map(fabricanteDTO, fabricante);
            await _fabricanteRepository.UpdateAsync(fabricante);

            await _cacheService.RemoveAsync($"fabricantes_{id}");
            await _cacheService.RemoveAsync("fabricantes_all");

            return _mapper.Map<FabricanteDTO>(fabricante);
        }
    }
}
