using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InfraGestion.Web;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Users.Services;
using InfraGestion.Web.Features.Inventory.Services;
using InfraGestion.Web.Features.Departments.Services;
using InfraGestion.Web.Features.Organization.Services;
using InfraGestion.Web.Features.Transfers.Services;
using InfraGestion.Web.Features.Technicians.Services;
using InfraGestion.Web.Core.Services;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5147")
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<InspectionService>(); // v2.1: New inspection service
builder.Services.AddScoped<DecommissioningService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<TransferService>();
builder.Services.AddScoped<TechnicianService>();
builder.Services.AddScoped<NavigationService>();

await builder.Build().RunAsync();