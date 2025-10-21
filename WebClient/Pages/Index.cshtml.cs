using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string? IdToken { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            IdToken = HttpContext.GetTokenAsync("id_token").Result;
            AccessToken = HttpContext.GetTokenAsync("access_token").Result;
            RefreshToken = HttpContext.GetTokenAsync("refresh_token").Result;
        }

        public IActionResult OnPostLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Index")
            };
            return Challenge(props, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync();
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Index")
            };
            return SignOut(props, OpenIdConnectDefaults.AuthenticationScheme, "Cookies");
        }
    }
}
