using System.ComponentModel.DataAnnotations;

namespace CarDealershipManager.Core.Models
{
    public class Cliente : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
        public string CPF { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }


        // Navigation Properties
        public ICollection<Venda>? Vendas { get; set; } = new List<Venda>();
    }
}
