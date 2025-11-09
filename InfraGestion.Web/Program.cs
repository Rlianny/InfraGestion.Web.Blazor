using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InfraGestion.Web;
using InfraGestion.Web.Features.Auth.Services;
using InfraGestion.Web.Features.Users.Services;
using InfraGestion.Web.Features.Inventory.Models;
using InfraGestion.Web.Features.Inventory.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DeviceService>();

await builder.Build().RunAsync();
