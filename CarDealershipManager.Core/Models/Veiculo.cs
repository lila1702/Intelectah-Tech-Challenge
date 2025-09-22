using CarDealershipManager.Core.Enums;
using CarDealershipManager.Core.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Veiculo : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        [Required]
        [ValidateAno(ErrorMessage = "O ano de fabricação não pode ser maior que o ano atual")]
        [Display(Name = "Ano de Fabricação")]
        [Column("Ano_Fabricacao")]
        public int AnoFabricacao { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [ForeignKey("Fabricante")]
        [Column("Fabricante_Id")]
        public int FabricanteId { get; set; }

        [Required]
        [Display(Name = "Tipo de Veículo")]
        [Column("Tipo_Veiculo")]
        public TipoVeiculo TipoVeiculo { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }


        // Navigation Properties
        public Fabricante? Fabricante { get; set; }
        public ICollection<Venda>? Vendas { get; set; } = new List<Venda>();

        }
}