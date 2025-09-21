using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Core.Interfaces;
using CarDealershipManager.Core.Interfaces.Services;

namespace CarDealershipManager.Infrastructure.Services
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IConcessionariaRepository _concessionariaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public VendaService(
            IVendaRepository vendaRepository,
            IVeiculoRepository veiculoRepository,
            IConcessionariaRepository concessionariaRepository,
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _vendaRepository = vendaRepository;
            _veiculoRepository = veiculoRepository;
            _concessionariaRepository = concessionariaRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public Task<VendaDTO> CreateAsync(VendaCreateDTO vendaDTO)
        {
            if (vendaDTO.DataVenda > DateTime.UtcNow)
                throw new ArgumentException("A data da venda não pode ser futura.");

            var veiculo = _veiculoRepository.GetByIdAsync(vendaDTO.VeiculoId);
            if (veiculo == null)
                throw new ArgumentException("Veículo não encontrado.");

            var concessionaria = _concessionariaRepository.GetByIdAsync(vendaDTO.ConcessionariaId);
            if (concessionaria == null)
                throw new ArgumentException("Concessionária não encontrada.");

            if (vendaDTO.PrecoVenda > veiculo.Result.Preco)

        }

        public Task<IEnumerable<VendaDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VendaDTO>> GetByClienteIdAsync(int clienteId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VendaDTO>> GetByConcessionariaIdAsync(int concessionariaId)
        {
            throw new NotImplementedException();
        }

        public Task<VendaDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VendaDTO>> GetByPeriodAsync(DateTime dataInicio, DateTime dataFim)
        {
            throw new NotImplementedException();
        }

        public Task<VendaDTO> GetByProtocolAsync(string protocolo)
        {
            throw new NotImplementedException();
        }
    }
}
