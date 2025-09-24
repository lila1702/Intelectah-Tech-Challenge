using CarDealershipManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.DTOs
{
    public class VeiculoDTO
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        [Display(Name = "Ano de Fabricação")]
        public int AnoFabricacao { get; set; }
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        [Display(Name = "Nome do Fabricante")]
        public string FabricanteNome { get; set; }
        [Display(Name = "Tipo do Veículo")]
        public TipoVeiculo TipoVeiculo { get; set; }
        public string TipoVeiculoDescricao { get; set; }
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
    }

    public class VeiculoCreateDTO
    {
        public string Modelo { get; set; }
        [Display(Name = "Ano de Fabricação")]
        public int AnoFabricacao { get; set; }
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        [Display(Name = "Tipo de Veículo")]
        public TipoVeiculo TipoVeiculo { get; set; }
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
    }

    public class VeiculoUpdateDTO
    {
        public string Modelo { get; set; }
        [Display(Name = "Ano de Fabricação")]
        public int AnoFabricacao { get; set; }
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }
        public int FabricanteId { get; set; }
        [Display(Name = "Tipo de Veículo")]
        public TipoVeiculo TipoVeiculo { get; set; }
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }
    }
}
