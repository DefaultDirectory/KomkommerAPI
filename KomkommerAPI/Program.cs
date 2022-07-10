using KomkommerAPI.EndpointConfigurations;
using KomkommerAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(Station));
var app = builder.Build();
app.UseEndpointDefinitions();
app.Run();