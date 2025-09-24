using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.DTOs
{
    public class FabricanteDTO
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "País de Origem")]
        public string PaisOrigem { get; set; }
        [Display(Name = "Ano de Fundação")]
        public int AnoFundacao { get; set; }
        [Display(Name = "URL do Website")]
        public string Website { get; set; }
        [Display(Name = "Total de Veículos")]
        public int TotalVeiculos { get; set; }
        [Display(Name = "Criado em")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Atualizado em")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class FabricanteCreateDTO
    {
        public string Nome { get; set; }
        [Display(Name = "País de Origem")]
        public string PaisOrigem { get; set; }
        [Display(Name = "Ano de Fundação")]
        public int AnoFundacao { get; set; }
        [Display(Name = "URL do Website")]
        public string Website { get; set; }
    }

    public class FabricanteUpdateDTO
    {
        public string Nome { get; set; }
        [Display(Name = "País de Origem")]
        public string PaisOrigem { get; set; }
        [Display(Name = "Ano de Fundação")]
        public int AnoFundacao { get; set; }
        [Display(Name = "URL do Website")]
        public string Website { get; set; }
    }
}
