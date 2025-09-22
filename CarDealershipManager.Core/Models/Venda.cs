using CarDealershipManager.Core.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipManager.Core.Models
{
    public class Venda : BaseModel
    {
        [Required]
        [ForeignKey("Veiculo")]
        [Display(Name = "Id do Veículo")]
        [Column("Veiculo_Id")]
        public int VeiculoId { get; set; }

        [Required]
        [ForeignKey("Concessionaria")]
        [Display(Name = "Id da Concessionária")]
        [Column("Concessionaria_Id")]
        public int ConcessionariaId { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        [Display(Name = "Id do Cliente")]
        [Column("Cliente_Id")]
        public int ClienteId { get; set; }

        [Required]
        [ValidateDate(ErrorMessage = "A venda não pode ser futura")]
        [Display(Name = "Data da Venda")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column("Data_Venda")]
        public DateTime DataVenda { get; set; }

        [Required]
        [Display(Name = "Preço de Venda")]
        [Column("Preco_Venda", TypeName = "decimal(10, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser acima de 0")]
        public decimal PrecoVenda { get; set; }

        [Required]
        [StringLength(20)]
        public string ProtocoloVenda { get; set; }


        // Navigation Properties
        public Veiculo Veiculo { get; set; }
        public Concessionaria Concessionaria { get; set; }
        public Cliente Cliente { get; set; }
    }
}