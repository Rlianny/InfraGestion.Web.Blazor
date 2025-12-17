namespace InfraGestion.Web.Features.Reports.DTOs
{
    public class DeviceMantainenceReportDto
    {
        public DateTime Date { get; set; }
        
        public string Type { get; set; }
   
        public string Description { get; set; }
        
        public string Technician { get; set; }
    }
}
