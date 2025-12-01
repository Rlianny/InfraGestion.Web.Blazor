using InfraGestion.Web.Features.Technicians.Models;

namespace InfraGestion.Web.Features.Technicians.Services;

public class TechnicianService
{
    private readonly HttpClient _httpClient;

    // Mock data for technicians
    private static List<Technician> _mockTechnicians = new()
    {
        new Technician
        {
            Id = 1,
            Name = "Ana García",
            Specialty = "Especialista en Redes Informáticas",
            Section = "Diseño y Ingeniería de Red",
            PhotoUrl = "/images/techs/tech_01.png",
            Status = TechnicianStatus.Active,
            Rating = 4.0m,
            Email = "ana.garcia@infracom.com",
            Phone = "+1 234 567 8901",
            HireDate = new DateTime(2020, 3, 15)
        },
        new Technician
        {
            Id = 2,
            Name = "Carlos Rodríguez",
            Specialty = "Especialista en Seguridad",
            Section = "Seguridad Perimetral y Firewalls",
            PhotoUrl = "/images/techs/tech_02.png",
            Status = TechnicianStatus.Active,
            Rating = 2.0m,
            Email = "carlos.rodriguez@infracom.com",
            Phone = "+1 234 567 8902",
            HireDate = new DateTime(2019, 7, 22)
        },
        new Technician
        {
            Id = 3,
            Name = "Manuel Martínez",
            Specialty = "Especialista en Fibra Óptica",
            Section = "Instalaciones y Activaciones",
            PhotoUrl = "/images/techs/tech_03.png",
            Status = TechnicianStatus.Active,
            Rating = 4.0m,
            Email = "manuel.martinez@infracom.com",
            Phone = "+1 234 567 8903",
            HireDate = new DateTime(2021, 1, 10)
        },
        new Technician
        {
            Id = 4,
            Name = "Javier López",
            Specialty = "Especialista en Soporte Nivel 2",
            Section = "Soporte a Nodos Remotos",
            PhotoUrl = "/images/techs/tech_04.png",
            Status = TechnicianStatus.Active,
            Rating = 3.0m,
            Email = "javier.lopez@infracom.com",
            Phone = "+1 234 567 8904",
            HireDate = new DateTime(2022, 5, 8)
        },
        new Technician
        {
            Id = 5,
            Name = "David Pérez",
            Specialty = "Especialista en Administración de Servidores",
            Section = "Servidores y Virtualización",
            PhotoUrl = "/images/techs/tech_05.png",
            Status = TechnicianStatus.Active,
            Rating = 5.0m,
            Email = "david.perez@infracom.com",
            Phone = "+1 234 567 8905",
            HireDate = new DateTime(2018, 11, 20)
        }
    };

    // Mock data for specialties
    private static List<string> _mockSpecialties = new()
    {
        "Especialista en Redes Informáticas",
        "Especialista en Seguridad",
        "Especialista en Fibra Óptica",
        "Especialista en Soporte Nivel 2",
        "Especialista en Administración de Servidores",
        "Especialista en Virtualización",
        "Especialista en Storage",
        "Especialista en Telecomunicaciones"
    };

    // Mock data for sections
    private static List<string> _mockSections = new()
    {
        "Diseño y Ingeniería de Red",
        "Seguridad Perimetral y Firewalls",
        "Instalaciones y Activaciones",
        "Soporte a Nodos Remotos",
        "Servidores y Virtualización",
        "Almacenamiento y Backup",
        "Plataforma como Servicio",
        "Reparación y Refabricación",
        "Conmutación y Enrutamiento Avanzado"
    };

    public TechnicianService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<Technician>> GetAllTechniciansAsync()
    {
        return Task.FromResult(_mockTechnicians.OrderBy(t => t.Name).ToList());
    }

    public Task<Technician?> GetTechnicianByIdAsync(int id)
    {
        var technician = _mockTechnicians.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(technician);
    }

    public Task<Technician> CreateTechnicianAsync(CreateTechnicianRequest request)
    {
        var newTechnician = new Technician
        {
            Id = _mockTechnicians.Any() ? _mockTechnicians.Max(t => t.Id) + 1 : 1,
            Name = request.Name,
            Specialty = request.Specialty,
            Section = request.Section,
            PhotoUrl = string.IsNullOrEmpty(request.PhotoUrl) ? "/images/techs/tech_01.png" : request.PhotoUrl,
            Status = TechnicianStatus.Active,
            Rating = 0,
            Email = request.Email,
            Phone = request.Phone,
            HireDate = request.HireDate,
            CreatedAt = DateTime.Now
        };
        _mockTechnicians.Add(newTechnician);
        return Task.FromResult(newTechnician);
    }

    public Task<Technician?> UpdateTechnicianAsync(UpdateTechnicianRequest request)
    {
        var technician = _mockTechnicians.FirstOrDefault(t => t.Id == request.Id);
        if (technician != null)
        {
            technician.Name = request.Name;
            technician.Specialty = request.Specialty;
            technician.Section = request.Section;
            technician.PhotoUrl = string.IsNullOrEmpty(request.PhotoUrl) ? technician.PhotoUrl : request.PhotoUrl;
            technician.Status = request.Status;
            technician.Rating = request.Rating;
            technician.Email = request.Email;
            technician.Phone = request.Phone;
            technician.HireDate = request.HireDate;
        }
        return Task.FromResult(technician);
    }

    public Task<bool> DeleteTechnicianAsync(int id)
    {
        var technician = _mockTechnicians.FirstOrDefault(t => t.Id == id);
        if (technician != null)
        {
            _mockTechnicians.Remove(technician);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<List<string>> GetSpecialtiesAsync()
    {
        return Task.FromResult(_mockSpecialties.ToList());
    }

    public Task<List<string>> GetSectionsAsync()
    {
        return Task.FromResult(_mockSections.ToList());
    }

    public Task<TechnicianDetails?> GetTechnicianDetailsAsync(int id)
    {
        var technician = _mockTechnicians.FirstOrDefault(t => t.Id == id);
        if (technician == null)
            return Task.FromResult<TechnicianDetails?>(null);

        var currentYear = DateTime.Now.Year;
        var hireYear = technician.HireDate.Year;

        var details = new TechnicianDetails
        {
            Id = technician.Id,
            Name = technician.Name,
            IdentificationNumber = $"USER{technician.Id:D3}",
            Specialty = technician.Specialty.Replace("Especialista en ", ""),
            Section = technician.Section,
            Department = GetDepartmentForSection(technician.Section),
            SectionManager = GetManagerForSection(technician.Section),
            PhotoUrl = technician.PhotoUrl,
            Status = technician.Status,
            Rating = technician.Rating,
            Email = technician.Email,
            Phone = technician.Phone,
            HireDate = technician.HireDate,
            YearsOfExperience = currentYear - hireYear,
            LastInterventionDate = GetLastInterventionDate(technician.Id),
            CreatedAt = technician.CreatedAt,
            MaintenanceHistory = GetMockMaintenanceHistory(technician.Id, technician.Name),
            DecommissionProposals = GetMockDecommissionProposals(technician.Id),
            Ratings = GetMockRatings(technician.Id, technician.Rating)
        };

        return Task.FromResult<TechnicianDetails?>(details);
    }

    private string GetDepartmentForSection(string section)
    {
        return section switch
        {
            "Diseño y Ingeniería de Red" => "Recepción y Diagnóstico Técnico",
            "Seguridad Perimetral y Firewalls" => "Seguridad Informática",
            "Instalaciones y Activaciones" => "Operaciones de Campo",
            "Soporte a Nodos Remotos" => "Soporte Técnico",
            "Servidores y Virtualización" => "Infraestructura TI",
            _ => "Operaciones Generales"
        };
    }

    private string GetManagerForSection(string section)
    {
        return section switch
        {
            "Diseño y Ingeniería de Red" => "Alejandro Torres",
            "Seguridad Perimetral y Firewalls" => "María Fernández",
            "Instalaciones y Activaciones" => "Roberto Sánchez",
            "Soporte a Nodos Remotos" => "Patricia López",
            "Servidores y Virtualización" => "Fernando Díaz",
            _ => "Sin Asignar"
        };
    }

    private DateTime? GetLastInterventionDate(int technicianId)
    {
        // Mock: retorna una fecha basada en el ID del técnico
        return technicianId switch
        {
            1 => new DateTime(2025, 7, 1),
            2 => new DateTime(2025, 6, 15),
            3 => new DateTime(2025, 5, 20),
            4 => new DateTime(2025, 4, 10),
            5 => new DateTime(2025, 8, 5),
            _ => null
        };
    }

    private List<MaintenanceRecord> GetMockMaintenanceHistory(int technicianId, string technicianName)
    {
        // Datos de ejemplo basados en el mockup
        if (technicianId == 1)
        {
            return new List<MaintenanceRecord>
            {
                new MaintenanceRecord
                {
                    Id = 1,
                    Date = new DateTime(2025, 7, 1),
                    Type = "Preventivo",
                    TechnicianName = technicianName,
                    Notes = "Limpieza de ventiladores y componentes",
                    Cost = 24.00m,
                    DeviceId = "DEV001",
                    DeviceName = "Servidor Principal"
                },
                new MaintenanceRecord
                {
                    Id = 2,
                    Date = new DateTime(2024, 1, 5),
                    Type = "Predictivo",
                    TechnicianName = technicianName,
                    Notes = "Actualización de firmware a v2.5.1 sin incidentes",
                    Cost = 120.90m,
                    DeviceId = "DEV002",
                    DeviceName = "Switch Core"
                }
            };
        }

        return new List<MaintenanceRecord>
        {
            new MaintenanceRecord
            {
                Id = 1,
                Date = DateTime.Now.AddMonths(-2),
                Type = "Correctivo",
                TechnicianName = technicianName,
                Notes = "Reparación de componentes dañados",
                Cost = 85.50m,
                DeviceId = "DEV003",
                DeviceName = "Router de Borde"
            }
        };
    }

    private List<DecommissionProposal> GetMockDecommissionProposals(int technicianId)
    {
        if (technicianId == 1)
        {
            return new List<DecommissionProposal>
            {
                new DecommissionProposal
                {
                    Id = 1,
                    Date = new DateTime(2023, 12, 20),
                    DeviceId = "DEV001",
                    Cause = "Fallo Técnico Irreparable",
                    Receiver = "Alejandro Torres",
                    Status = "Aprobado"
                }
            };
        }

        return new List<DecommissionProposal>();
    }

    private List<TechnicianRating> GetMockRatings(int technicianId, decimal rating)
    {
        if (technicianId == 1)
        {
            return new List<TechnicianRating>
            {
                new TechnicianRating
                {
                    Id = 1,
                    Date = new DateTime(2023, 11, 10),
                    Issuer = "Ana García",
                    Score = rating,
                    Description = "Excelente desempeño."
                }
            };
        }

        return new List<TechnicianRating>
        {
            new TechnicianRating
            {
                Id = 1,
                Date = DateTime.Now.AddMonths(-3),
                Issuer = "Supervisor General",
                Score = rating,
                Description = "Buen trabajo en las tareas asignadas."
            }
        };
    }
}
