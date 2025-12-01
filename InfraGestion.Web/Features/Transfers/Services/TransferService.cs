using InfraGestion.Web.Features.Transfers.Models;

namespace InfraGestion.Web.Features.Transfers.Services;

public class TransferService
{
    private readonly HttpClient _httpClient;

    // Mock data for transfers
    private static List<Transfer> _mockTransfers = new()
    {
        new Transfer
        {
            Id = 1,
            DeviceId = "DEV001",
            DeviceName = "Router de Agregación ASR 9000",
            Origin = "Reparación y Refabricación",
            Destination = "Plataforma como Servicio",
            TransferDate = new DateTime(2024, 3, 10),
            ReceiverName = "Carlos Méndez",
            CreatedAt = DateTime.Now.AddMonths(-8)
        },
        new Transfer
        {
            Id = 2,
            DeviceId = "DEV001",
            DeviceName = "Router de Agregación ASR 9000",
            Origin = "Diseño y Ingeniería de Red",
            Destination = "Reparación y Refabricación",
            TransferDate = new DateTime(2023, 3, 1),
            ReceiverName = "María González",
            CreatedAt = DateTime.Now.AddMonths(-20)
        },
        new Transfer
        {
            Id = 3,
            DeviceId = "DEV004",
            DeviceName = "Sistema UPS Eaton 20kVA",
            Origin = "Plataforma como Servicio",
            Destination = "Almacenamiento y Backup",
            TransferDate = new DateTime(2023, 1, 4),
            ReceiverName = "Pedro Ramírez",
            CreatedAt = DateTime.Now.AddMonths(-22)
        },
        new Transfer
        {
            Id = 4,
            DeviceId = "DEV001",
            DeviceName = "Router de Agregación ASR 9000",
            Origin = "Almacenamiento y Backup",
            Destination = "Diseño y Ingeniería de Red",
            TransferDate = new DateTime(2022, 12, 30),
            ReceiverName = "Ana Torres",
            CreatedAt = DateTime.Now.AddMonths(-23)
        },
        new Transfer
        {
            Id = 5,
            DeviceId = "DEV001",
            DeviceName = "Router de Agregación ASR 9000",
            Origin = "Servidores y Virtualización",
            Destination = "Almacenamiento y Backup",
            TransferDate = new DateTime(2021, 11, 11),
            ReceiverName = "Luis Fernández",
            CreatedAt = DateTime.Now.AddMonths(-36)
        }
    };

    // Mock data for devices (simplified for dropdown)
    private static List<(string Id, string Name)> _mockDevices = new()
    {
        ("DEV001", "Router de Agregación ASR 9000"),
        ("DEV002", "Switch Core Nexus 9500"),
        ("DEV003", "Firewall Palo Alto PA-5200"),
        ("DEV004", "Sistema UPS Eaton 20kVA"),
        ("DEV005", "Servidor Dell PowerEdge R750"),
        ("DEV006", "Storage NetApp AFF A400")
    };

    // Mock data for sections/departments (locations)
    private static List<string> _mockLocations = new()
    {
        "Reparación y Refabricación",
        "Plataforma como Servicio",
        "Diseño y Ingeniería de Red",
        "Almacenamiento y Backup",
        "Servidores y Virtualización",
        "Seguridad Perimetral y Firewalls",
        "Conmutación y Enrutamiento Avanzado",
        "Instalaciones y Activaciones",
        "Soporte a Nodos Remotos"
    };

    public TransferService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<List<Transfer>> GetAllTransfersAsync()
    {
        return Task.FromResult(_mockTransfers.OrderByDescending(t => t.TransferDate).ToList());
    }

    public Task<Transfer?> GetTransferByIdAsync(int id)
    {
        var transfer = _mockTransfers.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(transfer);
    }

    public Task<Transfer> CreateTransferAsync(CreateTransferRequest request)
    {
        var device = _mockDevices.FirstOrDefault(d => d.Id == request.DeviceId);
        var newTransfer = new Transfer
        {
            Id = _mockTransfers.Any() ? _mockTransfers.Max(t => t.Id) + 1 : 1,
            DeviceId = request.DeviceId,
            DeviceName = device.Name ?? "Equipo desconocido",
            Origin = request.Origin,
            Destination = request.Destination,
            TransferDate = request.TransferDate,
            ReceiverName = request.ReceiverName,
            Notes = request.Notes,
            CreatedAt = DateTime.Now
        };
        _mockTransfers.Add(newTransfer);
        return Task.FromResult(newTransfer);
    }

    public Task<Transfer?> UpdateTransferAsync(UpdateTransferRequest request)
    {
        var transfer = _mockTransfers.FirstOrDefault(t => t.Id == request.Id);
        if (transfer != null)
        {
            var device = _mockDevices.FirstOrDefault(d => d.Id == request.DeviceId);
            transfer.DeviceId = request.DeviceId;
            transfer.DeviceName = device.Name ?? transfer.DeviceName;
            transfer.Origin = request.Origin;
            transfer.Destination = request.Destination;
            transfer.TransferDate = request.TransferDate;
            transfer.ReceiverName = request.ReceiverName;
            transfer.Notes = request.Notes;
        }
        return Task.FromResult(transfer);
    }

    public Task<bool> DeleteTransferAsync(int id)
    {
        var transfer = _mockTransfers.FirstOrDefault(t => t.Id == id);
        if (transfer != null)
        {
            _mockTransfers.Remove(transfer);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<List<(string Id, string Name)>> GetDevicesAsync()
    {
        return Task.FromResult(_mockDevices.ToList());
    }

    public Task<List<string>> GetLocationsAsync()
    {
        return Task.FromResult(_mockLocations.ToList());
    }
}
