using Microsoft.AspNetCore.Mvc;
using RetoTecnico.API.Services.Interfaces;
using System.Threading.Tasks;

namespace RetoTecnico.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly IObtenerPdfService _obtenerPdfService;

        public FacturasController(IObtenerPdfService obtenerPdfService)
        {
            _obtenerPdfService = obtenerPdfService;
        }

        [HttpPost("obtenerpdf/{uuid}")] 
        public async Task<IActionResult> ObtenerPdf([FromRoute] string uuid)
        {

            var response = await _obtenerPdfService.ObtenerPdfAsync(uuid);
            
            if (response.Exitoso)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}