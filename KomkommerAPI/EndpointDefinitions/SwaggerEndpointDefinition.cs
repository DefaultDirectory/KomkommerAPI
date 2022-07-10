using KomkommerAPI.EndpointConfigurations;
using Microsoft.OpenApi.Models;

namespace KomkommerAPI.EndpointDefinitions
{
    public class SwaggerEndpointDefinition : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KomkommerAPI v1"));
        }

        public void DefineServices(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Komkommer", Version = "1.0.0" });
            });
        }
    }
}
