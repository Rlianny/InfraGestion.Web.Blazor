using InfraGestion.Web.Features.Inventory.Models;

namespace InfraGestion.Web.Features.Inventory.Services;

public class DeviceService
{
    private readonly List<Device> _devices = new()
    {
        new Device
        {
            Id = 1,
            Name = "Router de Agregación ASR 9000",
            Type = DeviceType.ConnectivityAndNetwork,
            State = OperationalState.UnderMaintenance,
            Location = "Mantenimiento Correctivo y Urgencias"
        },
        new Device
        {
            Id = 2,
            Name = "Servidor de Virtualización HP DL380",
            Type = DeviceType.ComputingAndIT,
            State = OperationalState.Operational,
            Location = "Recepción y Diagnóstico Técnico"
        },
        new Device
        {
            Id = 3,
            Name = "Firewall de Próxima Generación PA-5200",
            Type = DeviceType.ConnectivityAndNetwork,
            State = OperationalState.Operational,
            Location = "Análisis Forense y Respuesta a Incidentes"
        },
        new Device
        {
            Id = 4,
            Name = "Sistema UPS Eaton 20kVA",
            Type = DeviceType.ElectricalInfrastructureAndSupport,
            State = OperationalState.Decommissioned,
            Location = "Análisis Forense y Respuesta a Incidentes"
        },
        new Device
        {
            Id = 5,
            Name = "Analizador de Espectro Viavi",
            Type = DeviceType.DiagnosticAndMeasurement,
            State = OperationalState.BeingTransferred,
            Location = "Plataforma como Servicio"
        }
    };

    private int _nextId = 6;

    public Task<List<Device>> GetAllDevicesAsync()
    {
        return Task.FromResult(_devices.ToList());
    }

    public Task<Device?> GetDeviceByIdAsync(int id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        return Task.FromResult(device);
    }

    public Task<Device> CreateDeviceAsync(CreateDeviceRequest request)
    {
        var newDevice = new Device
        {
            Id = _nextId++,
            Name = request.Name,
            Type = request.Type,
            State = OperationalState.UnderMaintenance, // Nuevo dispositivo va a defectación inicial
            Location = "Recepción y Diagnóstico Técnico" // Ubicación inicial
        };

        _devices.Add(newDevice);
        return Task.FromResult(newDevice);
    }

    public Task<Device?> UpdateDeviceAsync(UpdateDeviceRequest request)
    {
        var device = _devices.FirstOrDefault(d => d.Id == request.Id);
        if (device == null) return Task.FromResult<Device?>(null);

        device.Name = request.Name;
        device.Type = request.Type;
        device.State = request.State;
        device.Location = request.Location;

        return Task.FromResult<Device?>(device);
    }

    public Task<bool> DeleteDeviceAsync(int id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null) return Task.FromResult(false);

        _devices.Remove(device);
        return Task.FromResult(true);
    }

    public Task<List<Device>> SearchDevicesAsync(
        string searchTerm,
        DeviceType? type,
        OperationalState? state,
        string location)
    {
        var query = _devices.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(d =>
                d.Id.ToString().Contains(searchTerm) ||
                d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                d.Location.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (type.HasValue)
        {
            query = query.Where(d => d.Type == type.Value);
        }

        if (state.HasValue)
        {
            query = query.Where(d => d.State == state.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(d => d.Location == location);
        }

        return Task.FromResult(query.ToList());
    }

    public Task<Dictionary<string, int>> GetStatisticsAsync()
    {
        var stats = new Dictionary<string, int>
        {
            ["Total"] = _devices.Count,
            ["Operational"] = _devices.Count(d => d.State == OperationalState.Operational),
            ["UnderMaintenance"] = _devices.Count(d => d.State == OperationalState.UnderMaintenance),
            ["Decommissioned"] = _devices.Count(d => d.State == OperationalState.Decommissioned)
        };

        return Task.FromResult(stats);
    }

    public Task<DeviceDetails?> GetDeviceDetailsAsync(int id)
    {
        var device = _devices.FirstOrDefault(d => d.Id == id);
        if (device == null) return Task.FromResult<DeviceDetails?>(null);

        // Simular detalles extendidos (en producción vendría de la base de datos)
        var details = new DeviceDetails
        {
            Id = device.Id,
            Name = device.Name,
            IdentificationNumber = $"DEV{device.Id:D3}",
            Type = device.Type,
            State = device.State,
            PurchaseDate = DateTime.Now.AddMonths(-14), // Simulado
            MaintenanceCount = 2,
            TotalMaintenanceCost = 144.90m,
            LastMaintenanceDate = new DateTime(2025, 7, 1),
            Section = "Taller Central y Logística",
            Department = device.Location,
            SectionManager = "Alejandro Torres",
            MaintenanceHistory = new List<MaintenanceRecord>
        {
            new MaintenanceRecord
            {
                Date = new DateTime(2025, 7, 1),
                Type = "Preventivo",
                Technician = "Carlos Ruiz",
                Notes = "Limpieza de ventiladores y componentes",
                Cost = 24.00m
            },
            new MaintenanceRecord
            {
                Date = new DateTime(2024, 1, 5),
                Type = "Predictivo",
                Technician = "Ana López",
                Notes = "Actualización de firmware a v2.5.1 sin incidentes",
                Cost = 120.90m
            }
        },
            TransferHistory = new List<TransferRecord>
        {
            new TransferRecord
            {
                Date = new DateTime(2023, 12, 20),
                Origin = "Gestión de Activos y Red Local",
                Destination = "Recepción y Diagnóstico Técnico",
                Responsible = "Alejandro Torres",
                Receiver = "Juan Pérez"
            }
        },
            InitialDefect = new InitialDefect
            {
                SubmissionDate = new DateTime(2023, 11, 10),
                Requester = "Ana García",
                Technician = "Carlos Ruiz",
                Status = "Aprobado",
                ResponseDate = new DateTime(2023, 11, 11)
            }
        };

        return Task.FromResult<DeviceDetails?>(details);
    }
}