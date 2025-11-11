> Proyecto de Programación
> Facultad de Matemática y Computación - Universidad de La Habana.
> Curso 2025
![Have you delete the image?](.//MockupSlides/banner.png)

# InfraGestion - Infrastructure Management System

<div align="center">

**Comprehensive system for enterprise technological infrastructure management**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Features •
Requirements •
Installation •
Usage •
Technologies •
Contributing

</div>

---

## Overview

InfraGestion is a modern web application designed to facilitate the management and control of technological infrastructure equipment in enterprise environments. Built with **Blazor WebAssembly** and **.NET 9**, it provides an intuitive interface for inventory tracking, maintenance, transfers, and user management.

## Features

### User Management
- Authentication system with roles (Administrator, Director, Section Manager, Technician, Logistician)
- Role-based access control
- User account administration
- Department assignment for users

### Devices Inventory
- Comprehensive device catalog
- Classification by types:
  - Connectivity and Network
  - Computing and IT
  - Electrical Infrastructure and Support
  - Communications and Transmission
  - Diagnostic and Measurement
- Operational states (Operational, Under Maintenance, Decommissioned, Being Transferred)
- Advanced search filters
- Real-time statistics

### Maintenance History
- Detailed records of preventive, predictive, and corrective maintenance
- Cost tracking
- Technician assignment
- Last maintenance date tracking

### Transfer Records
- Complete equipment movement history
- Origin and destination traceability
- Responsible persons and receivers
- Transfer dates

### Technical Defect Assessment
- Equipment decommission requests
- States: Pending, Approved, Rejected
- Evaluator technician assignment
- Issue and response dates

### Organizational Management
- Dynamic department structure
- Sections and work areas
- Manager assignment

## Technologies

### Frontend
- **Blazor WebAssembly** - Interactive UI framework
- **CSS3** - Custom styles with CSS variables
- **HTML5** - Semantic structure

### Backend (API)
- **.NET 9** - Main runtime and framework
- **ASP.NET Core Web API** - RESTful services
- **Entity Framework Core** - Data access ORM

### Tools
- **Visual Studio Code** - Code editor
- **Git** - Version control
- **Blazored.LocalStorage** - Browser local storage

## Requirements

### Development
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher
- [Visual Studio Code](https://code.visualstudio.com/) with C# extension
- [Git](https://git-scm.com/)

### Runtime
- Modern web browser (Chrome, Firefox, Edge, Safari)
- Backend API (configured at `http://localhost:5147`)

## Installation

### 1. Clone the repository
```bash
git clone https://github.com/your-username/InfraGestion.git
cd InfraGestion
```

### 2. Restore dependencies
```bash
cd InfraGestion.Web
dotnet restore
```

### 3. Configure the API
Edit the API base URL in `Program.cs`:
```csharp
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://localhost:5147") // Change according to your setup
});
```

### 4. Run the application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`

## Usage

### Login

#### Demo Mode (without API)
The application includes demo users:

| Username | Password | Role |
|----------|----------|------|
| admin | admin | Administrator |
| director | director | Director |
| jefe | jefe | Section Manager |
| tecnico | tecnico | Technician |
| logistica | logistica | Logistician |

#### Production Mode (with API)
Use credentials provided by the system administrator.

### Main Navigation

- **Dashboard** - System overview
- **Organization** - Enterprise structure management
- **User Management** - Account administration
- **Inventory** - Equipment catalog
- **Technical Decommissions** - Defect requests
- **Transfers** - Equipment movements
- **Technical Team** - Technical staff
- **Reports** - Reports and statistics

## Project Structure

```
InfraGestion.Web/
├── Features/              # Feature modules
│   ├── Auth/             # Authentication
│   ├── Dashboard/        # Main panel
│   ├── Departments/      # Department management
│   ├── Inventory/        # Inventory management
│   │   ├── Components/   # Reusable components
│   │   ├── DTOs/         # Data Transfer Objects
│   │   ├── Helpers/      # Utilities
│   │   ├── Models/       # Domain models
│   │   ├── Pages/        # Blazor pages
│   │   └── Services/     # Business services
│   └── Users/            # User management
├── Core/                 # Shared functionality
│   ├── Constants/        # Global constants
│   ├── Models/          # Base models
│   └── Services/        # Base services
├── Shared/              # Shared components
│   └── Layout/          # Page layouts
├── wwwroot/             # Static resources
│   ├── css/             # Global styles
│   ├── images/          # Images and logos
│   └── index.html       # HTML entry point
├── _Imports.razor       # Global imports
├── App.razor            # Root component
└── Program.cs           # Application configuration
```

## Workflows

### Add Equipment
1. Navigate to **Inventory**
2. Click **"Add Equipment"**
3. Complete form with:
   - Equipment name
   - Device type
   - Acquisition date
   - Responsible technician
4. Save

### Register Maintenance
1. Select equipment in **Inventory**
2. Access **Equipment Details**
3. In **"Maintenance History"** section
4. View historical records

### Create User
1. Navigate to **User Management**
2. Click **"Add User"**
3. Enter information:
   - Full name
   - Role
   - Department
   - Password
4. Save

## Customization

### Adding New Modules
1. Create folder in `Features/`
2. Implement structure:
   - `Pages/` - Razor pages
   - `Components/` - Reusable components
   - `Services/` - Business logic
   - `Models/` - Data models
   - `DTOs/` - Transfer objects

## Testing

```bash
# Run unit tests (when available)
dotnet test
```

## Contributing

Contributions are welcome. Please:

1. Fork the project
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Contribution Guidelines
- Follow existing code conventions
- Document new features
- Include tests when applicable
- Update documentation


## Reporting Issues

Report bugs and request features at [GitHub Issues](https://github.com/Rlianny/InfraGestion.Web.Blazor/issues).

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Authors

- **Lianny Revé** - *Initial Development* - [@Rlianny](https://github.com/Rlianny/Rlianny)

## Acknowledgments

- Icons from [Phosphor Icons](https://phosphoricons.com/)
- Design inspiration from modern enterprise systems
- Blazor and .NET community

---

<div align="center">

Made with ❤️ using Blazor WebAssembly

</div>
