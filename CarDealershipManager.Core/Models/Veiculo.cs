using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Core.Models
{
    public class Veiculo : BaseModel
    {
        [Required]
        [StringLength(100)]
        public required string Modelo { get; set; }

        [Required]
        [Range(1800, 2100, ErrorMessage = "Ano Inválido")]
        [Display(Name = "Ano de Fabricação")]
        [Column("Ano_Fabricacao")]
        public required int AnoFabricacao { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public required decimal Preco { get; set; }

        [Required]
        [ForeignKey("Fabricante")]
        [Column("Fabricante_Id")]
        public required int FabricanteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Veículo")]
        [Column("Tipo_Veiculo")]
        public required TipoVeiculo TipoVeiculo { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }


        // Navigation Properties
        public Fabricante? Fabricante { get; set; }
        public ICollection<Venda>? Vendas { get; set; } = [];

        }
}