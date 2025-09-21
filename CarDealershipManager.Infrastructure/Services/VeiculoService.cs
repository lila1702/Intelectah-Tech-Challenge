using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Interfaces.Services;
using CarDealershipManager.Core.Enums;

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

            await _cacheService.RemoveAsync($"veiculo_{id}");

            return _mapper.Map<VeiculoDTO>(await _veiculoRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(int id)
        {
            await _veiculoRepository.DeleteByIdAsync(id);
            await _cacheService.RemoveAsync($"veiculo_{id}");
        }

        public async Task<IEnumerable<VeiculoDTO>> GetAllAsync()
        {
            var veiculos = await _veiculoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);
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
            var veiculos = await _veiculoRepository.GetByFabricanteIdAsync(fabricanteId);
            return _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);
        }

        public async Task<IEnumerable<VeiculoDTO>> GetByTipoAsync(TipoVeiculo tipoVeiculo)
        {
            var veiculos = await _veiculoRepository.GetByTipoAsync(tipoVeiculo);
            return _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);
        }

        public async Task<IEnumerable<VeiculoDTO>> SearchByModeloAsync(string modelo)
        {
            var veiculos = await _veiculoRepository.SearchByModeloAsync(modelo);
            return _mapper.Map<IEnumerable<VeiculoDTO>>(veiculos);
        }
    }
}
