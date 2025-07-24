using RetoTecnico.API.DTOs;
using System.Threading.Tasks;

namespace RetoTecnico.API.Services.Interfaces;
public interface IObtenerPdfService
{
    Task<ObtenerPdfResponse> ObtenerPdfAsync(string uuid); 
}
