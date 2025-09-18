using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Concessionaria : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Endereco { get; set; }

        [Required]
        [StringLength(50)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression(@"^\d{5}-?\d{3}$")]
        public string CEP { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email Inválido")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Capacidade Máxima de Veículos")]
        [Column("Capacidade_Maxima_Veiculos")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacidade deve ser acima de 0")]
        public int CapacidadeMaximaVeiculos { get; set; }


        // Navigation Properties
        public ICollection<Venda>? Vendas { get; set; } = new List<Venda>();
        public ICollection<Veiculo>? Veiculos { get; set; } = new List<Veiculo>();
    }
}