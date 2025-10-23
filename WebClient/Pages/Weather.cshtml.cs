using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void OnGet()
        {
        }
    }
}
