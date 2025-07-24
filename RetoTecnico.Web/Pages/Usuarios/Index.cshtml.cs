using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace RetoTecnico.Web.Pages.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public string ApiBaseUrl { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            ApiBaseUrl = _configuration["ApiSettings:BaseUrl"]!;
        }
    }
}