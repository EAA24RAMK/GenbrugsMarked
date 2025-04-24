using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp;
using WebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("http://localhost:5036/") }
);

await builder.Build().RunAsync();