using AutoMapper;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Fabricante
            CreateMap<Fabricante, FabricanteDTO>()
                .ForMember(dest => dest.TotalVeiculos, opt => opt.MapFrom(src => src.Veiculos.Count));
            CreateMap<Fabricante, FabricanteCreateDTO>();
            CreateMap<Fabricante, FabricanteUpdateDTO>();

            // Veiculos
            CreateMap<Veiculo, VeiculoDTO>()
                .ForMember(dest => dest.FabricanteNome, opt => opt.MapFrom(src => src.Fabricante.Nome))
                .ForMember(dest => dest.TipoVeiculoDescricao, opt => opt.MapFrom(src => src.TipoVeiculo.ToString()));
            CreateMap<Veiculo, VeiculoCreateDTO>();
            CreateMap<Veiculo, VeiculoUpdateDTO>();

            // Concessionaria
            CreateMap<Concessionaria, ConcessionariaDTO>()
                .ForMember(dest => dest.TotalVendas, opt => opt.MapFrom(src => src.Vendas.Count));
            CreateMap<Concessionaria, ConcessionariaCreateDTO>();
            CreateMap<Concessionaria, ConcessionariaUpdateDTO>();

            // Cliente
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<Cliente, ClienteCreateDTO>();

            // Venda
            CreateMap<Venda, VendaDTO>()
                .ForMember(dest => dest.VeiculoModelo, opt => opt.MapFrom(src => src.Veiculo.Modelo))
                .ForMember(dest => dest.FabricanteNome, opt => opt.MapFrom(src => src.Veiculo.Fabricante.Nome))
                .ForMember(dest => dest.ConcessionariaNome, opt => opt.MapFrom(src => src.Concessionaria.Nome))
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.Nome))
                .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente.CPF));
        }
    }
}
