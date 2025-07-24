using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace RetoTecnico.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string? Username { get; set; } 

        [BindProperty]
        public string? Password { get; set; } 

     
        [TempData] 
        public string? ErrorMessage { get; set; } 

        public string? ApiBaseUrl { get; set; }

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            ApiBaseUrl = _configuration["ApiSettings:BaseUrl"];
            ErrorMessage = ""; 
        }

       
    }


    public class AuthResponse
    {
        public string? Token { get; set; } 
    }
}