namespace RetoTecnico.API.DTOs
{
    public class ObtenerPdfResponse
    {
        public bool Exitoso { get; set; }
        public string? PDFBase64 { get; set; } // El PDF en Base64
        public string? MensajeError { get; set; }
        public int? CodigoError { get; set; } // Nullable si no siempre viene
    }
}