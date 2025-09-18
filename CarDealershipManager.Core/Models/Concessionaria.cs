using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Concessionaria : BaseModel
    {
        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public required string Endereco { get; set; }

        [Required]
        [StringLength(50)]
        public required string Cidade { get; set; }

        [Required]
        [StringLength(50)]
        public required string Estado { get; set; }

        [Required]
        [StringLength(10)]
        public required string CEP { get; set; }

        [Required]
        [StringLength(15)]
        public required string Telefone { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email Inválido")]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Capacidade Máxima de Veículos")]
        [Column("Capacidade_Maxima_Veiculos")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacidade deve ser acima de 0")]
        public required int CapacidadeMaximaVeiculos { get; set; }


        // Navigation Properties
        public ICollection<Venda>? Vendas { get; set; } = [];
        public ICollection<Veiculo>? Veiculos { get; set; } = [];
    }
}