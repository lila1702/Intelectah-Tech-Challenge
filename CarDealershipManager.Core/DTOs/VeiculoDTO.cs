using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Core.DTOs
{
    public class VeiculoDTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        public string FabricanteNome { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string TipoVeiculoDescricao { get; set; }
        public string? Descricao { get; set; }
    }

    public class CreateVeiculoDTO
    {
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string? Descricao { get; set; }
    }

    public class UpdateVeiculoDTO
    {
        public string Modelo { get; set; }
        public int AnoFabricacao { get; set; }
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string? Descricao { get; set; }
    }
}
