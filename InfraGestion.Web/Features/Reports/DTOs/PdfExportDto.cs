namespace InfraGestion.Web.Features.Reports.DTOs;

public class PdfExportDto
{
    public PdfExportDto(byte[] pdfContent)
    {
        PdfContent = pdfContent;
    }

    public byte[] PdfContent { get; set; }
}
