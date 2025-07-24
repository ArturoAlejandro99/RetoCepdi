using System.Text;
using System.Xml.Linq;
using RetoTecnico.API.DTOs;
using RetoTecnico.API.Services.Interfaces;

namespace RetoTecnico.API.Services;

public class ObtenerPdfService : IObtenerPdfService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private const string ServiceUrl = "https://timbrador.cepdi.mx:8443/WSDemo/WS";

    public ObtenerPdfService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration; 
    }

    public async Task<ObtenerPdfResponse> ObtenerPdfAsync(string uuid)
    {
        var usuario = _configuration["WebServiceSettings:Usuario"];
        var password = _configuration["WebServiceSettings:Password"];

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
        {
            return new ObtenerPdfResponse
            {
                Exitoso = false,
                MensajeError = "Credenciales de Web Service no configuradas en appsettings.json."
            };
        }

        var client = _httpClientFactory.CreateClient();

        var soapRequest = $@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://WebService/"">
                <soapenv:Header/>
                <soapenv:Body>
                    <web:ObtenerPDF>
                        <!--Optional:-->
                        <Usuario>demo1@mail.com</Usuario>
                        <!--Optional:-->
                        <Password>Demo123#</Password>
                        <!--Optional:-->
                        <uuid>ca7ae63b-32aa-40b9-8bb9-7c500a087ab5</uuid>
                    </web:ObtenerPDF>
                </soapenv:Body>
                </soapenv:Envelope>";

        var content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
        client.DefaultRequestHeaders.Add("SOAPAction", @"""#POST"""); 

        try
        {
            var response = await client.PostAsync(ServiceUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new ObtenerPdfResponse
                {
                    Exitoso = false,
                    MensajeError = $"Error HTTP: {response.StatusCode} - {errorContent}"
                };
            }

            var responseString = await response.Content.ReadAsStringAsync();

            var xDoc = XDocument.Parse(responseString);
            XNamespace soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsNs = "http://WebService/";
            
            var obtenerPdfResponseElement = xDoc.Descendants(wsNs + "ObtenerPDFResponse").FirstOrDefault();
            
            if (obtenerPdfResponseElement == null)
            {
                return new ObtenerPdfResponse
                {
                    Exitoso = false,
                    MensajeError = "No se encontró el nodo ObtenerPDFResponse en la respuesta SOAP."
                };
            }

            var respuestaPdfElement = obtenerPdfResponseElement.Elements().FirstOrDefault(); 

            if (respuestaPdfElement == null)
            {
                return new ObtenerPdfResponse
                {
                    Exitoso = false,
                    MensajeError = "No se encontró el objeto de respuesta (RespuestaPDF) en la respuesta SOAP."
                };
            }

            var exitosoElement = respuestaPdfElement.Element("Exitoso");
            var pdfElement = respuestaPdfElement.Element("PDF");
            var mensajeErrorElement = respuestaPdfElement.Element("MensajeError");
            var codigoErrorElement = respuestaPdfElement.Element("CodigoError");

            return new ObtenerPdfResponse
            {
                Exitoso = (bool?)exitosoElement ?? false,
                PDFBase64 = pdfElement?.Value,
                MensajeError = mensajeErrorElement?.Value,
                CodigoError = (int?)codigoErrorElement
            };
        }
        catch (HttpRequestException ex)
        {
            return new ObtenerPdfResponse
            {
                Exitoso = false,
                MensajeError = $"Error de red al conectar con el servicio: {ex.Message}"
            };
        }
        catch (System.Xml.XmlException ex)
        {
            return new ObtenerPdfResponse
            {
                Exitoso = false,
                MensajeError = $"Error al parsear la respuesta XML del servicio: {ex.Message}"
            };
        }
        catch (System.Exception ex)
        {
            return new ObtenerPdfResponse
            {
                Exitoso = false,
                MensajeError = $"Ocurrió un error inesperado al llamar al servicio: {ex.Message}"
            };
        }
    }
}
