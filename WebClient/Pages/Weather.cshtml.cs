using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Pages
{
    public class WeatherModel : PageModel
    {
        private readonly ILogger<WeatherModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherModel(ILogger<WeatherModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public List<Item>? Items { get; set; }
        public string? Error { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                Error = "Access token není k dispozici – chybí scope nebo přihlášení.";
                return Page();
            }

            var http = _httpClientFactory.CreateClient("WeatherApi");
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using var res = await http.GetAsync("api/WeatherForecast");
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogError("API call failed with status {StatusCode}", res.StatusCode);
                var body = await res.Content.ReadAsStringAsync();
                Error = $"Chyba API: {(int)res.StatusCode} {res.ReasonPhrase}. Odpověď: {body}";
                return Page();
            }

            var json = await res.Content.ReadAsStringAsync();
            Items = JsonSerializer.Deserialize<List<Item>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Page();
        }
    }

    public record Item(DateOnly Date, int TemperatureC, string Summary);
}
