using Microsoft.AspNetCore.Identity;
using CarDealershipManager.Core.Enums;

namespace CarDealershipManager.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; }
        public NivelAcesso NivelAcesso { get; set; }
        public DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
