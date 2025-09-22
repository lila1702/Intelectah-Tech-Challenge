using System.ComponentModel.DataAnnotations;
using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.App.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve estar em formato válido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare("Password", ErrorMessage = "A senha e a confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Nível de acesso é obrigatório")]
        [Display(Name = "Nível de Acesso")]
        public NivelAcesso NivelAcesso { get; set; }
    }
}
