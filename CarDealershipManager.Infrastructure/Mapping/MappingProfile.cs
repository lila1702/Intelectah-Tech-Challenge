using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;

namespace CarDealershipManager.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Fabricante
            CreateMap<Fabricante, FabricanteDTO>()
                .ForMember(dest => dest.TotalVeiculos, opt => opt.MapFrom(src => src.Veiculos.Count));

            CreateMap<FabricanteCreateDTO, Fabricante>();
            CreateMap<FabricanteUpdateDTO, Fabricante>();

            // Veículo
            CreateMap<Veiculo, VeiculoDTO>()
                .ForMember(dest => dest.FabricanteNome, opt => opt.MapFrom(src => src.Fabricante.Nome))
                .ForMember(dest => dest.TipoVeiculoDescricao, opt => opt.MapFrom(src => src.TipoVeiculo.ToString()));

            CreateMap<VeiculoCreateDTO, Veiculo>();
            CreateMap<VeiculoUpdateDTO, Veiculo>();

            // Concessionária
            CreateMap<Concessionaria, ConcessionariaDTO>()
                .ForMember(dest => dest.TotalVendas, opt => opt.MapFrom(src => src.Vendas.Count));

            CreateMap<ConcessionariaCreateDTO, Concessionaria>();
            CreateMap<ConcessionariaUpdateDTO, Concessionaria>();

            // Cliente
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteCreateDTO, Cliente>();

            // Venda
            CreateMap<Venda, VendaDTO>()
                .ForMember(dest => dest.VeiculoModelo, opt => opt.MapFrom(src => src.Veiculo.Modelo))
                .ForMember(dest => dest.TipoVeiculo, opt => opt.MapFrom(src => src.Veiculo.TipoVeiculo))
                .ForMember(dest => dest.FabricanteNome, opt => opt.MapFrom(src => src.Veiculo.Fabricante.Nome))
                .ForMember(dest => dest.ConcessionariaNome, opt => opt.MapFrom(src => src.Concessionaria.Nome))
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.CPFCliente, opt => opt.MapFrom(src => src.Cliente.CPF))
                .ForMember(dest => dest.ClienteTelefone, opt => opt.MapFrom(src => src.Cliente.Telefone));

            CreateMap<VendaCreateDTO, Venda>()
                .ForMember(dest => dest.Cliente, opt => opt.Ignore()) // Cliente é tratado no serviço
                .ForMember(dest => dest.ProtocoloVenda, opt => opt.Ignore()); // Gerado no serviço
        }
    }
}