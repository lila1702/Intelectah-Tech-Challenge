using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManager.Core.Validations
{
    public class ValidadeAno
    {
        public static bool ValidarAno(int ano)
        {
            int anoAtual = DateTime.Now.Year;
            return ano >= 1800 && ano <= anoAtual;
        }
    }
}
