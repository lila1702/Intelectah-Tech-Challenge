using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Fabricante : BaseModel
    {
        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Pais_Origem")]
        [Display(Name = "Pa�s de Origem")]
        public required string PaisOrigem { get; set; }

        [Required]
        [Range(1800, 2100, ErrorMessage = "Ano inv�lido")]
        [Column("Ano_Fundacao")]
        [Display(Name = "Ano de Funda��o")]
        public int AnoFundacao { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Website_Url")]
        [Display(Name = "URL do Website")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
            ErrorMessage = "URL Inv�lida")]
        public required string Website { get; set; }


        // Navigation Properties
        public ICollection<Veiculo>? Veiculos { get; set; } = [];
    }
}