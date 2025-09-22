namespace CarDealershipManager.Core.Validations
{
    public class ValidateCPF
    {
        public static bool IsValidCPF(string cpf)
        {
            // Verifica se o CPF é nulo ou vazio
            if (string.IsNullOrEmpty(cpf))
                return false;

            // Remove caracteres não numéricos
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Digitos Iguais
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            string digitosCalculados = CalcularDigitosVerificadores(cpf.Substring(0, 9));

            return cpf.EndsWith(digitosCalculados);
        }

        private static string CalcularDigitosVerificadores(string baseCPF)
        {
            int primeiroDigito = CalcularDigito(baseCPF, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
            int segundoDigito = CalcularDigito(baseCPF + primeiroDigito, new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            return $"{primeiroDigito}{segundoDigito}";
        }

        private static int CalcularDigito(string cpfParcial, int[] pesos)
        {
            int soma = 0;
            for (int i = 0; i < cpfParcial.Length; i++)
            {
                soma += (cpfParcial[i] - '0') * pesos[i];
            }

            int resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }
    }
}
