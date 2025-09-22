using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Core.DTOs
{
    public class RelatorioVendasDTO
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int TotalVendas { get; set; }
        public decimal ValorTotal { get; set; }
        public IEnumerable<VendaPorTipoDTO> VendasPorTipo { get; set; }
        public IEnumerable<VendaPorFabricanteDTO> VendasPorFabricante { get; set; }
        public IEnumerable<VendaPorConcessionariaDTO> VendasPorConcessionaria { get; set; }
    }

    public class VendaPorTipoDTO
    {
        public TipoVeiculo TipoVeiculo { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class VendaPorFabricanteDTO
    {
        public string Fabricante { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class VendaPorConcessionariaDTO
    {
        public string Concessionaria { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
