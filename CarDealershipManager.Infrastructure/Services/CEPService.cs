using System.Text.Json;
using CarDealershipManager.Core.DTOs;
using CarDealershipManager.Core.Interfaces.External;

namespace CarDealershipManager.Infrastructure.Services
{
    public class CEPService : ICEPService
    {
        private readonly HttpClient _httpClient;

        public CEPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EnderecoDTO> BuscarEnderecoPorCEPAsync(string cep)
        {
            try
            {
                var cepSanitized = cep.Replace("-", "").Replace(".", "").Trim();

                if (cepSanitized.Length != 8 || !long.TryParse(cepSanitized, out _))
                {
                    return null;
                }

                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cepSanitized}/json/");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var viaCepResponse = JsonSerializer.Deserialize<ViaCepResponse>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (viaCepResponse?.Erro == true)
                {
                    return null;
                }

                return new EnderecoDTO
                {
                    CEP = viaCepResponse.Cep,
                    Logradouro = viaCepResponse.Logradouro,
                    Complemento = viaCepResponse.Complemento,
                    Bairro = viaCepResponse.Bairro,
                    Localidade = viaCepResponse.Localidade,
                    Uf = viaCepResponse.Uf
                };
            }
            catch
            {
                return null;
            }
        }
    }
}

internal class ViaCepResponse
{
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Localidade { get; set; }
    public string? Uf { get; set; }
    public bool Erro { get; set; }
}
