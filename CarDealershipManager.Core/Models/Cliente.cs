using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Models
{
    public class Cliente : BaseModel
    {
        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"\d{11}", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
        public required string CPF { get; set; }

        [Required]
        [StringLength(15)]
        public required string Telefone { get; set; }
    }
}
