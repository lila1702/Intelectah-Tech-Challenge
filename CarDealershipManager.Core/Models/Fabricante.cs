using CarDealershipManager.Core.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Fabricante : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Pais_Origem")]
        [Display(Name = "Pa�s de Origem")]
        public string PaisOrigem { get; set; }

        [Required]
        [ValidateAno(ErrorMessage = "O ano de funda��o n�o pode ser maior que o ano atual")]
        [Column("Ano_Fundacao")]
        [Display(Name = "Ano de Funda��o")]
        public int AnoFundacao { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Website_Url")]
        [Display(Name = "URL do Website")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",
            ErrorMessage = "URL Inv�lida")]
        public string Website { get; set; }


        // Navigation Properties
        public ICollection<Veiculo>? Veiculos { get; set; } = new List<Veiculo>();
    }
}