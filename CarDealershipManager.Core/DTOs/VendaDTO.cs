using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.DTOs
{
    public class VendaDTO
    {
        public int Id { get; set; }
        public int VeiculoId { get; set; }
        [Display(Name="Veículo")]
        public string VeiculoModelo { get; set; }
        [Display(Name ="Tipo de Veículo")]
        public TipoVeiculo TipoVeiculo { get; set; }
        [Display(Name = "Fabricante")]
        public string FabricanteNome { get; set; }
        public int ConcessionariaId { get; set; }
        [Display(Name = "Concessionária")]
        public string ConcessionariaNome { get; set; }
        public int ClienteId { get; set; }
        [Display(Name = "Cliente")]
        public string ClienteNome { get; set; }
        [Display(Name = "CPF do Cliente")]
        public string CPFCliente { get; set; }
        [Display(Name = "Telefone do Cliente")]
        public string ClienteTelefone { get; set; }
        [ValidateDate(ErrorMessage = "A data da venda não pode ser futura.")]
        [Display(Name = "Data da Venda")]
        public DateTime DataVenda { get; set; }
        [Display(Name = "Preço da Venda")]
        public decimal PrecoVenda { get; set; }
        [Display(Name = "Protocolo")]
        public string ProtocoloVenda { get; set; }
    }

    public class VendaCreateDTO
    {
        public int VeiculoId { get; set; }
        public int ConcessionariaId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteCPF { get; set; }
        public string ClienteTelefone { get; set; }
        [ValidateDate(ErrorMessage = "A data da venda não pode ser futura.")]
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
    }
}