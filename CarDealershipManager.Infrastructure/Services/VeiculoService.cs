using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Interfaces.Repositories;

namespace CarDealershipManager.Infrastructure.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IFabricanteRepository _fabricanteRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public VeiculoService(
            IVeiculoRepository veiculoRepository,
            IFabricanteRepository fabricanteRepository,
            IMapper mapper,
            ICacheService cacheService)
        {
            _veiculoRepository = veiculoRepository;
            _fabricanteRepository = fabricanteRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<VeiculoDTO> CreateAsync(VeiculoCreateDTO veiculoDTO)
        {
            var fabricante = await _fabricanteRepository.GetByIdAsync(veiculoDTO.FabricanteId);
            if (fabricante == null)
            {
                throw new ArgumentException("Fabricante não encontrado");
            }

            if (veiculoDTO.AnoFabricacao > DateTime.Now.Year)
            {
                throw new ArgumentException("Ano de fabricação não pode ser maior que o ano atual");
            }

            var veiculo = _mapper.Map<Veiculo>(veiculoDTO);
            await _veiculoRepository.AddAsync(veiculo);

            await InvalidateCacheAsync(veiculo);

            return _mapper.Map<VeiculoDTO>(await _veiculoRepository.GetByIdAsync(veiculo.Id));
        }

        public async Task<VeiculoDTO> UpdateAsync(int id, VeiculoUpdateDTO veiculoDTO)
        {
            var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo == null)
            {
                throw new ArgumentException("Veículo não encontrado");
            }

            var fabricante = await _fabricanteRepository.GetByIdAsync(veiculoDTO.FabricanteId);
            if (fabricante == null)
            {
                throw new ArgumentException("Fabricante não encontrado");
            }

            if (veiculoDTO.AnoFabricacao > DateTime.Now.Year)
            {
                throw new ArgumentException("Ano de fabricação não pode ser maior que o ano atual");
            }

            _mapper.Map(veiculoDTO, veiculo);
            await _veiculoRepository.UpdateAsync(veiculo);

            await InvalidateCacheAsync(veiculo);

            return _mapper.Map<VeiculoDTO>(await _veiculoRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(int id)
        {
            var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo == null)
            {
                throw new ArgumentException("Veículo não encontrado");
            }

            await _veiculoRepository.DeleteByIdAsync(id);
            await InvalidateCacheAsync(veiculo, id);
        }

        public async Task<IEnumerable<VeiculoDTO>> GetAllAsync()
        {
            var cacheKey = "veiculos_all";
            var cached = await _cacheService.GetAsync<IEnumerable<VeiculoDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var veiculos = await _veiculoRepository.GetAllActiveAsync();
            var veiculosDTO = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            await _cacheService.SetAsync(cacheKey, veiculosDTO, TimeSpan.FromMinutes(15));

            return veiculosDTO;
        }

        public async Task<VeiculoDTO> GetByIdAsync(int id)
        {
            var cacheKey = $"veiculo_{id}";
            var cached = await _cacheService.GetAsync<VeiculoDTO>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo == null)
            {
                throw new ArgumentException("Veículo não encontrado");
            }

            var veiculoDTO = _mapper.Map<VeiculoDTO>(veiculo);
            await _cacheService.SetAsync(cacheKey, veiculoDTO, TimeSpan.FromMinutes(30));

            return veiculoDTO;
        }

        public async Task<IEnumerable<VeiculoDTO>> GetByFabricanteIdAsync(int fabricanteId)
        {
            var cacheKey = $"veiculos_fabricante_{fabricanteId}";
            var cached = await _cacheService.GetAsync<IEnumerable<VeiculoDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var veiculos = await _veiculoRepository.GetByFabricanteIdAsync(fabricanteId);
            var veiculosDTO = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            await _cacheService.SetAsync(cacheKey, veiculosDTO, TimeSpan.FromMinutes(10));

            return veiculosDTO;
        }

        public async Task<IEnumerable<VeiculoDTO>> GetByTipoAsync(TipoVeiculo tipoVeiculo)
        {
            var cacheKey = $"veiculos_tipo_{tipoVeiculo}";
            var cached = await _cacheService.GetAsync<IEnumerable<VeiculoDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var veiculos = await _veiculoRepository.GetByTipoAsync(tipoVeiculo);
            var veiculosDTO = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            await _cacheService.SetAsync(cacheKey, veiculosDTO, TimeSpan.FromMinutes(10));

            return veiculosDTO;
        }

        public async Task<IEnumerable<VeiculoDTO>> SearchByModeloAsync(string modelo)
        {
            var cacheKey = $"veiculos_modelo_{modelo.ToLower()}";
            var cached = await _cacheService.GetAsync<IEnumerable<VeiculoDTO>>(cacheKey);

            if (cached != null)
            {
                return cached;
            }

            var veiculos = await _veiculoRepository.SearchByModeloAsync(modelo);
            var veiculosDTO = _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);

            await _cacheService.SetAsync(cacheKey, veiculosDTO, TimeSpan.FromMinutes(5));

            return veiculosDTO;
        }

        private async Task InvalidateCacheAsync(Veiculo veiculo, int? id = null)
        {
            await _cacheService.RemoveAsync("veiculos_all");
            await _cacheService.RemoveAsync($"veiculo_{id ?? veiculo.Id}");
            await _cacheService.RemoveAsync($"veiculos_fabricante_{veiculo.FabricanteId}");
            await _cacheService.RemoveAsync($"veiculos_tipo_{veiculo.TipoVeiculo}");
        }
    }
}
