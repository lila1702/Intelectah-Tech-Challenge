using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.DTOs
{
    public class ConcessionariaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Capacidade Máxima de Veículos")]
        public int CapacidadeMaximaVeiculos { get; set; }
        [Display(Name = "Total de Vendas")]
        public int TotalVendas { get; set; }
    }

    public class ConcessionariaCreateDTO
    {
        public string Nome { get; set; }
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Capacidade Máxima de Veículos")]
        public int CapacidadeMaximaVeiculos { get; set; }
    }

    public class ConcessionariaUpdateDTO
    {
        public string Nome { get; set; }
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Telefone { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Capacidade Máxima de Veículos")]
        public int CapacidadeMaximaVeiculos { get; set; }
    }
}
