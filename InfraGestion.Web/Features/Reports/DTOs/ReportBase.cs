namespace InfraGestion.Web.Features.Reports.DTOs
{
    public class Report<T>
    {
        public List<T> ReportData { get; set; }
        public byte[] PdfBytes { get; set; } 
    }
}
