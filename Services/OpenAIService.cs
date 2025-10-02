using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class CohereService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public CohereService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();

        // Define headers padrão
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var requestBody = new
        {
            model = "command-r-plus-08-2024", // Modelo atual da Cohere
            messages = new[]
            {
                new { role = "user", content = "Sugira local para viagens" }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.cohere.com/v2/chat", content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao chamar a API: {response.StatusCode}\n{errorContent}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(jsonResponse);

        // Novo formato de resposta da API v2
        var text = doc.RootElement
                      .GetProperty("message")
                      .GetProperty("content")[0]
                      .GetProperty("text")
                      .GetString();

        return text ?? "Nenhuma resposta gerada.";
    }
}