using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PManagerFrontend;
using PManagerFrontend.Interfaces.Services;
using PManagerFrontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient("api", client => 
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("API").GetSection("BaseUrl").Get<string>());
});

builder.Services.AddScoped<IDataService, DataService>();

await builder.Build().RunAsync();
