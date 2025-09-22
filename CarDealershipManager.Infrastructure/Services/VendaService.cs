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

        public async Task<VendaDTO> CreateAsync(VendaCreateDTO vendaDTO)
        {
            // Validações básicas
            if (vendaDTO.DataVenda > DateTime.UtcNow)
                throw new ArgumentException("A data da venda não pode ser futura.");

            var veiculo = await _veiculoRepository.GetByIdAsync(vendaDTO.VeiculoId);
            if (veiculo == null)
                throw new ArgumentException("Veículo não encontrado.");

            var concessionaria = await _concessionariaRepository.GetByIdAsync(vendaDTO.ConcessionariaId);
            if (concessionaria == null)
                throw new ArgumentException("Concessionária não encontrada.");

            if (vendaDTO.PrecoVenda > veiculo.Preco)
                throw new ArgumentException("O preço de venda não pode ser maior que o preço do veículo.");

            // Buscar ou criar cliente
            var cliente = await _clienteRepository.GetByCpfAsync(vendaDTO.ClienteCPF);
            if (cliente == null)
            {
                cliente = new Cliente
                {
                    Nome = vendaDTO.ClienteNome,
                    CPF = vendaDTO.ClienteCPF,
                    Telefone = vendaDTO.ClienteTelefone
                };
                cliente = await _clienteRepository.AddAsync(cliente);
            }

            // Criar venda
            var protocolo = await _vendaRepository.GenerateUniqueProtocolAsync();
            var venda = new Venda
            {
                VeiculoId = vendaDTO.VeiculoId,
                ConcessionariaId = vendaDTO.ConcessionariaId,
                ClienteId = cliente.Id,
                DataVenda = vendaDTO.DataVenda,
                PrecoVenda = vendaDTO.PrecoVenda,
                ProtocoloVenda = protocolo
            };

            await _vendaRepository.AddAsync(venda);
            return _mapper.Map<VendaDTO>(await _vendaRepository.GetByIdAsync(venda.Id));
        }

        public async Task<VendaDTO> GetByIdAsync(int id)
        {
            var venda = await _vendaRepository.GetByIdAsync(id);
            if (venda == null)
                throw new ArgumentException("Venda não encontrada.");

            return _mapper.Map<VendaDTO>(venda);
        }

        public async Task<IEnumerable<VendaDTO>> GetAllAsync()
        {
            var vendas = await _vendaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VendaDTO>>(vendas);
        }

        public async Task<IEnumerable<VendaDTO>> GetByClienteIdAsync(int clienteId)
        {
            var vendas = await _vendaRepository.GetVendasByClienteIdAsync(clienteId);
            return _mapper.Map<IEnumerable<VendaDTO>>(vendas);
        }

        public async Task<IEnumerable<VendaDTO>> GetByConcessionariaIdAsync(int concessionariaId)
        {
            var vendas = await _vendaRepository.GetVendasByConcessionariaIdAsync(concessionariaId);
            return _mapper.Map<IEnumerable<VendaDTO>>(vendas);
        }

        public async Task<IEnumerable<VendaDTO>> GetByPeriodAsync(DateTime dataInicio, DateTime dataFim)
        {
            var vendas = await _vendaRepository.GetVendasByPeriodAsync(dataInicio, dataFim);
            return _mapper.Map<IEnumerable<VendaDTO>>(vendas);
        }

        public async Task<VendaDTO> GetByProtocolAsync(string protocolo)
        {
            var vendas = await _vendaRepository.GetVendaByProtocolAsync(protocolo);
            return _mapper.Map<VendaDTO>(vendas);
        }
    }
}
