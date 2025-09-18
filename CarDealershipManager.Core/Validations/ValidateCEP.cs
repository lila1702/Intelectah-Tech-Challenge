
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Validations
{
    public class ValidateCEP
    {
        public static bool IsValidCEP(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            cep = new string(cep.Where(char.IsDigit).ToArray());
            if (cep.Length != 8)
                return false;
            
            return true;
        }
    }
}
